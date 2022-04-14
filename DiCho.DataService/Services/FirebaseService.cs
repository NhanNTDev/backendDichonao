using DiCho.DataService.ViewModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using FirebaseAdmin;
using Newtonsoft.Json.Linq;
using StackExchange.Redis.Extensions.Core.Abstractions;
using DiCho.Core.Custom;
using System.Net;

namespace DiCho.DataService.Services
{
    public partial interface IFirebaseService
    {
        Task<List<string>> UploadFilesToFirebase(IFormFileCollection files, string folder);
        Task<string> UploadFileToFirebase(IFormFile file, string folder);
        Task SendNotification(string title, string body, string customerId);
        Task AddNotiToRedis(NotificationModel model);
        Task<List<NotificationViewModel>> GetNotiRedis(string userId);
        Task DeleteNotiRedis(int id);
    }

    public partial class FirebaseService : IFirebaseService
    {
        private readonly IConfiguration _configuration;
        private readonly IRedisCacheClient _redisCacheClient;

        public FirebaseService(IConfiguration configuration, IRedisCacheClient redisCacheClient)
        {
            _configuration = configuration;
            _redisCacheClient = redisCacheClient;
        }

        public async Task SendNotification(string title, string body, string campaignId)
        {
            var message = new FirebaseAdmin.Messaging.Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>()
                {
                    {"CampaignId", campaignId },
                },
                Condition = "'all' in topics"
            };
            await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task<List<NotificationViewModel>> GetNotiRedis(string userId)
        {
            var notifications = new List<NotificationViewModel>();
            var notificationsReids = await _redisCacheClient.Db1.GetAsync<List<NotificationModel>>("Notifications");

            var notiUser = notificationsReids.Where(x => x.UserId == userId).ToList();

            if ((notiUser?.Count ?? 0) <= 0)
                return new List<NotificationViewModel>{ };
            else
            {
                
                foreach (var item in notiUser)
                {
                    notifications.Add(new NotificationViewModel{Title = item.Title, Body = item.Body , Time = item.Time});
                }
            }

            var result = notifications.OrderByDescending(x => x.Time).ToList();
            return result;
        }

        public async Task AddNotiToRedis(NotificationModel model)
        {
            var notifications = new List<NotificationModel>();
            var notificationsRedis = await _redisCacheClient.Db1.GetAsync<List<NotificationModel>>("Notifications");

            var time = DateTime.Now.ToString("HH:mm dd/MM/yyyy");
            if ((notificationsRedis?.Count ?? 0) > 0)
            {
                var id = 0;
                foreach (var notificationRedis in notificationsRedis)
                {
                    id += 1;
                    notifications.Add(new NotificationModel
                    {
                        Id = id,
                        UserId = notificationRedis.UserId,
                        Title = notificationRedis.Title,
                        Body = notificationRedis.Body,
                        Time = notificationRedis.Time
                    });;
                }
                notifications.Add(new NotificationModel
                {
                    UserId = model.UserId,
                    Title = model.Title,
                    Body = model.Body,
                    Id = id + 1,
                    Time = time
                });

                await _redisCacheClient.Db1.AddAsync<List<NotificationModel>>("Notifications", notifications);
            }
            else
            {
                notifications.Add(new NotificationModel
                {
                    Id = 1,
                    UserId = model.UserId,
                    Title = model.Title,
                    Body = model.Body,
                    Time = time
                });
                await _redisCacheClient.Db1.AddAsync("Notifications", notifications);
            }
        }

        public async Task DeleteNotiRedis(int id)
        {
            var notifications = new List<NotificationModel>();
            var notificationsRedis = await _redisCacheClient.Db1.GetAsync<List<NotificationModel>>("Notifications");

            var noti = notificationsRedis.Where(x => x.Id == id).ToList();
            if (noti == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");

            var idCount = 0;
            if ((noti?.Count ?? 0) > 0)
            {
                foreach (var item in noti)
                {
                    notificationsRedis.Remove(item);
                    foreach (var notificationRedis in notificationsRedis)
                    {
                        idCount += 1;

                        if (item.Id > idCount)
                        {
                            notifications.Add(new NotificationModel
                            {
                                Id = idCount,
                                UserId = notificationRedis.UserId,
                                Title = notificationRedis.Title,
                                Body = notificationRedis.Body,
                                Time = notificationRedis.Time
                            });
                        }
                        else
                        {
                            var value = idCount + 1;
                            notifications.Add(new NotificationModel
                            {
                                Id = value,
                                UserId = notificationRedis.UserId,
                                Title = notificationRedis.Title,
                                Body = notificationRedis.Body,
                                Time = notificationRedis.Time
                            });
                        }
                    }
                }

                await _redisCacheClient.Db1.AddAsync<List<NotificationModel>>("Notifications", notifications);
            }
        }

        public async Task<List<string>> UploadFilesToFirebase(IFormFileCollection files, string folder)
        {
            var listLink = new List<string>();
            if (files != null)
            {
                var firebaseAutProvider = new FirebaseAuthProvider(new FirebaseConfig(_configuration["Firebase:ApiKey"]));
                var firebaseAuthLink = await firebaseAutProvider.SignInWithEmailAndPasswordAsync(_configuration["Firebase:AuthEmail"], _configuration["Firebase:AuthPassword"]);
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var cancellation = new CancellationTokenSource();

                        using (var fileStream = file.OpenReadStream())
                        {
                            var task = new FirebaseStorage(
                                _configuration["Firebase:Bucket"],
                                new FirebaseStorageOptions
                                {
                                    AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken),
                                    ThrowOnCancel = true
                                })
                                .Child("Images")
                                .Child($"{folder}")
                                .Child($"{file.FileName}")
                                .PutAsync(fileStream, cancellation.Token);

                            var link = await task;
                            listLink.Add(link);
                            fileStream.Dispose();
                        }
                    }
                }
            }
            return listLink;
        }

        public async Task<string> UploadFileToFirebase(IFormFile file, string folder)
        {
            var link = "";
            if (file != null)
            {
                var firebaseAutProvider = new FirebaseAuthProvider(new FirebaseConfig(_configuration["Firebase:ApiKey"]));
                var firebaseAuthLink = await firebaseAutProvider.SignInWithEmailAndPasswordAsync(_configuration["Firebase:AuthEmail"], _configuration["Firebase:AuthPassword"]);
                if (file.Length > 0)
                {
                    var cancellation = new CancellationTokenSource();
                    var a = new FirebaseMetaData();

                    using (var fileStream = file.OpenReadStream())
                    {
                        var task = new FirebaseStorage(
                            _configuration["Firebase:Bucket"],
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken),
                                ThrowOnCancel = true
                            })
                            .Child("Images")
                            .Child($"{folder}")
                            .Child($"{file.FileName}")
                            .PutAsync(fileStream, cancellation.Token);

                        link = await task;
                        fileStream.Dispose();
                    }
                }
            }
            return link;
        }
    }
}
