using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiCho.Core.Constants;
using DiCho.Core.Custom;
using DiCho.Core.Utilities;
using DiCho.DataService.Commons;
using DiCho.DataService.Enums;
using DiCho.DataService.Models;
using DiCho.DataService.Response;
using DiCho.DataService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiCho.DataService.Services
{
    public interface IJWTService
    {
        Task<IdentityResult> CreateUser(AspNetUsersCreateModel model);
        Task<IdentityResult> UpdateUser(string id, AspNetUsersUpdateModel model);
        Task<string> UpdateImage(string id, AspNetUsersUpdateImageInputModel modelInput);
        Task UpdateImageForCustomer(string id, AspNetUsersUpdateImageModel model);
        Task<TokenModel> Login(AspNetUserLoginModel model);
        Task<IdentityResult> ChangePassword(AspNetUsersChangePasswordModel model);
        Task<IdentityResult> ConfirmForgotPassword(string username, string newPassword);
        Task<bool> CheckDuplicateUser(string username);
        Task<AspNetUsersViewModel> GetUserId(string id);
        UserCountModel CountUser();
        Task<DynamicModelsResponse<UserDataMapModel>> GetUserByRole(string role, UserDataMapModel model, int page, int size);
        Task BanAndUnBanUser(string id);
        Task<string> GetNameOfUser(string id);
        Task<IdentityResult> CreateUserForAdmin(CreateUserForAdmin model);
        Task<IdentityResult> CreateUserForDelivery(CreateUserForDelivery model);
        Task<TokenModel> LoginUserZalo(UserDataZalo model);
    }
    public class JWTService : IJWTService
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly SignInManager<AspNetUsers> _signInManager;
        private readonly RoleManager<AspNetRoles> _roleManager;
        private readonly IConfigurationProvider _mapper;
        private readonly ISmsService _smsService;
        private readonly IFirebaseService _firebaseService;
        private readonly ITradeZoneMapService _tradeZoneMapService;

        public JWTService(UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager, RoleManager<AspNetRoles> roleManager,
            ITradeZoneMapService tradeZoneMapService, ISmsService smsService, IFirebaseService firebaseService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper.ConfigurationProvider;
            _smsService = smsService;
            _firebaseService = firebaseService;
            _tradeZoneMapService = tradeZoneMapService;
        }

        private async Task<TokenModel> GenerateToken(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("UserId",user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            string secret = AppCoreConstants.SECRECT_KEY;
            string issuer = AppCoreConstants.ISSUE_KEY;
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                expires: DateTime.Now.AddDays(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new TokenModel
            {
                TokenType = "Bearer",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = token.ValidTo,
                User = new AspNetUsersModel
                {
                    Id = user.Id,
                    UserName = username,
                    Role = role,
                    Email = user.Email,
                    Name = user.Name,
                    Address = user.Address,
                    Image = user.Image,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Type = user.Type
                },
            };
        }

        public async Task<IdentityResult> CreateUser(AspNetUsersCreateModel model)
        {
            if (model.PhoneNumber.Contains(" "))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Không được có khoảng trắng!");
            if (model.Role == null)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cần chọn vai trò của tài khoản!");
            if (_userManager.Users.Any(x => x.PhoneNumber == model.PhoneNumber))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"SDT: {model.PhoneNumber} đã tồn tại!");

            var user = new AspNetUsers
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.PhoneNumber,
                PhoneNumber = model.PhoneNumber,
                CreateAt = DateTime.Now,
                Active = true,
                Image = "https://firebasestorage.googleapis.com/v0/b/dichonao-f5871.appspot.com/o/Images%2FAvatar%2Fdichonao_default.png?alt=media&token=a63e1bcf-6d00-47b6-94fd-84667446dd21"
            };
            AspNetRolesModel role = model.Role.FirstOrDefault();
            List<AspNetRoles> allRoles = _roleManager.Roles.ToList();
            if (allRoles.Where(x => x.Name == role.Name).FirstOrDefault() == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Vai trò: {role.Name} không tồn tại!");
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                //var codeToken = Convert.ToInt32(token);
                //while (codeToken.ToString().Length < 6 || codeToken.ToString().Length > 6)
                //{
                //    token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                //    codeToken = Convert.ToInt32(token);
                //}
                //await _smsService.SendSmsAsync(user.PhoneNumber, "Your DiChoNao verification code is: " + $"{codeToken}");
                //await _mailService.SendEmailAsync("user.Email", $"{codeToken}", "Ok");

                var newUser = await _userManager.FindByNameAsync(user.UserName);

                var tmp = await _roleManager.FindByNameAsync(role.Name);
                var result2 = await _userManager.AddToRoleAsync(newUser, tmp.Name);
                await _userManager.IsInRoleAsync(user, role.Name);
                await _userManager.AddToRoleAsync(user, role.Name);
                return IdentityResult.Success;
            }
            else
            {
                var error = result.Errors.FirstOrDefault();
                if (error.Description == "Passwords must have at least one uppercase ('A'-'Z').")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải có một ký tự in hoa!");
                if (error.Description == "Passwords must have at least one lowercase ('a'-'z').")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải có một ký tự thường!");
                if (error.Description == "Passwords must be at least 8 characters.")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải ít nhất 8 ký tự!");
                if (error.Description == $"Username '{model.PhoneNumber}' is invalid, can only contain letters or digits.")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Tài khoản {model.PhoneNumber} không đúng, cần phải có ký tự hoặc số!");
            }
            return null;
        }

        public async Task<IdentityResult> CreateUserForAdmin(CreateUserForAdmin model)
        {
            if (model.PhoneNumber.Contains(" "))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Không được có khoảng trắng!");
            if (model.Role == null)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cần chọn vai trò của tài khoản!");
            if (_userManager.Users.Any(x => x.UserName == model.UserName))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Tài Khoản đã tồn tại!");
            if (_userManager.Users.Any(x => x.PhoneNumber == model.PhoneNumber))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "SDT đã tồn tại!");
            DateTime? datetime = new DateTime();
            if (model.DateOfBirth != null)
                datetime = DateTime.ParseExact(model.DateOfBirth, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            else
                datetime = null;
            var user = new AspNetUsers
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                CreateAt = DateTime.Now,
                Address = model.Address,
                DateOfBirth = datetime,
                Gender = model.Gender,
                Active = true,
                Name = model.Name,
                Email = model.Email,
                Image = "https://firebasestorage.googleapis.com/v0/b/dichonao-f5871.appspot.com/o/Images%2FAvatar%2Fdichonao_default.png?alt=media&token=a63e1bcf-6d00-47b6-94fd-84667446dd21"
            };
            AspNetRolesModel role = model.Role.FirstOrDefault();
            List<AspNetRoles> allRoles = _roleManager.Roles.ToList();
            if (allRoles.Where(x => x.Name == role.Name).FirstOrDefault() == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Vai trò: {role.Name} không tồn tại!");
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var newUser = await _userManager.FindByNameAsync(user.UserName);

                var tmp = await _roleManager.FindByNameAsync(role.Name);
                var result2 = await _userManager.AddToRoleAsync(newUser, tmp.Name);
                await _userManager.IsInRoleAsync(user, role.Name);
                await _userManager.AddToRoleAsync(user, role.Name);
                return IdentityResult.Success;
            }
            else
            {
                var error = result.Errors.FirstOrDefault();
                if (error.Description == "Passwords must have at least one uppercase ('A'-'Z').")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải có một ký tự in hoa!");
                if (error.Description == "Passwords must have at least one lowercase ('a'-'z').")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải có một ký tự thường!");
                if (error.Description == "Passwords must be at least 8 characters.")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải ít nhất 8 ký tự!");
                if (error.Description == $"Username '{model.UserName}' is invalid, can only contain letters or digits.")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Tài khoản {model.UserName} không đúng, cần phải có ký tự hoặc số!");
            }

            return null;
        }
        
        public async Task<IdentityResult> CreateUserForDelivery(CreateUserForDelivery model)
        {
            if (model.UserName.Contains(" "))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Không được có khoảng trắng!");
            if (model.Role == null)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Cần chọn vai trò của tài khoản!");
            if (_userManager.Users.Any(x => x.UserName == model.UserName))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Tài Khoản đã tồn tại!");
            if (_userManager.Users.Any(x => x.PhoneNumber == model.PhoneNumber))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "SDT đã tồn tại!");

            var user = new AspNetUsers
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                CreateAt = DateTime.Now,
                Address = model.Address,
                Gender = model.Gender,
                Active = true,
                Email = model.Email,
                Type = model.Type,
                Name = model.Name,
                WareHouseId = model.WarehouseId,
                Image = "https://firebasestorage.googleapis.com/v0/b/dichonao-f5871.appspot.com/o/Images%2FAvatar%2Fdichonao_default.png?alt=media&token=a63e1bcf-6d00-47b6-94fd-84667446dd21"
            };
            AspNetRolesModel role = model.Role.FirstOrDefault();
            List<AspNetRoles> allRoles = _roleManager.Roles.ToList();
            if (allRoles.Where(x => x.Name == role.Name).FirstOrDefault() == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Vai trò: {role.Name} không tồn tại!");
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var newUser = await _userManager.FindByNameAsync(user.UserName);

                var tmp = await _roleManager.FindByNameAsync(role.Name);
                var result2 = await _userManager.AddToRoleAsync(newUser, tmp.Name);
                await _userManager.IsInRoleAsync(user, role.Name);
                await _userManager.AddToRoleAsync(user, role.Name);
                return IdentityResult.Success;
            }
            else
            {
                var error = result.Errors.FirstOrDefault();
                if (error.Description == "Passwords must have at least one uppercase ('A'-'Z').")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải có một ký tự in hoa!");
                if (error.Description == "Passwords must have at least one lowercase ('a'-'z').")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải có một ký tự thường!");
                if (error.Description == "Passwords must be at least 8 characters.")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải ít nhất 8 ký tự!");
                if (error.Description == $"Username '{model.UserName}' is invalid, can only contain letters or digits.")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Tài khoản {model.UserName} không đúng, cần phải có ký tự hoặc số!");
            }
            return null;
        }

        public async Task<TokenModel> LoginUserZalo(UserDataZalo model)
        {
            var getUser = _userManager.Users.FirstOrDefault(x => x.AspNetUserLogins.Any(y => y.ProviderKey == model.Id));

            if (getUser != null)
            {
                var roles = await _userManager.GetRolesAsync(getUser);
                var role = roles.FirstOrDefault();
                var token = await _userManager.GetAuthenticationTokenAsync(getUser, "Zalo", getUser.UserName);
                if (token == null)
                {
                    var newToken = await GenerateToken(getUser.UserName, role);
                    var result = await _userManager.SetAuthenticationTokenAsync(getUser, "Zalo", getUser.UserName, newToken.Token);
                    if (result.Succeeded)
                        return newToken;
                }
                var expires = new JwtSecurityTokenHandler().ReadToken(token).ValidTo;
                if (DateTime.Now < expires)
                    return new TokenModel
                    {
                        TokenType = "Bearer",
                        Token = token,
                        Expires = expires,
                        User = new AspNetUsersModel
                        {
                            Id = getUser.Id,
                            UserName = getUser.UserName,
                            Role = role,
                            Email = getUser.Email,
                            Name = getUser.Name,
                            Address = model.Address,
                            Image = getUser.Image,
                            Gender = getUser.Gender,
                            PhoneNumber = getUser.PhoneNumber,
                            DateOfBirth = getUser.DateOfBirth,
                            Type = getUser.Type,
                        },
                    };
                var createToken = await GenerateToken(getUser.UserName, role);
                var createResult = await _userManager.SetAuthenticationTokenAsync(getUser, "Zalo", getUser.UserName, createToken.Token);
                if (createResult.Succeeded)
                    return createToken;
                return null;
            }
            else
            {
                var userName = model.Id;
                var user = new AspNetUsers
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userName,
                    Address = model.Address,
                    CreateAt = DateTime.Now,
                    Active = true,
                    Image = model.Url,
                    Name = model.Name,
                };
                user.AspNetUserLogins.Add(new AspNetUserLogins { UserId = user.Id, LoginProvider = "Zalo", ProviderKey = model.Id, ProviderDisplayName = "Zalo" });
                user.AspNetUserRoles.Add(new AspNetUserRoles { RoleId = "2", UserId = user.Id });
                await _userManager.CreateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                var token = await _userManager.GetAuthenticationTokenAsync(user, "Zalo", user.UserName);
                if (token == null)
                {
                    var newToken = await GenerateToken(user.UserName, role);
                    var result = await _userManager.SetAuthenticationTokenAsync(user, "Zalo", user.UserName, newToken.Token);
                    if (result.Succeeded)
                        return newToken;
                }
                var expires = new JwtSecurityTokenHandler().ReadToken(token).ValidTo;
                if (DateTime.Now < expires)
                    return new TokenModel
                    {
                        TokenType = "Bearer",
                        Token = token,
                        Expires = expires,
                        User = new AspNetUsersModel
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Role = role,
                            Email = user.Email,
                            Name = user.Name,
                            Address = user.Address,
                            Image = user.Image,
                            Gender = user.Gender,
                            PhoneNumber = user.PhoneNumber,
                            DateOfBirth = user.DateOfBirth,
                            Type = user.Type,
                        },
                    };
                var createToken = await GenerateToken(user.UserName, role);
                var createResult = await _userManager.SetAuthenticationTokenAsync(user, "Zalo", user.UserName, createToken.Token);
                if (createResult.Succeeded)
                    return createToken;
                return null;
            }
        }

        public async Task<bool> CheckDuplicateUser(string username)
        {
            var entity = await _userManager.FindByIdAsync(username);
            if (username.Contains(" "))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Không được có khoảng trắng!");
            if (_userManager.Users.Any(x => x.UserName == username))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"SDT: {username} đã tồn tại!");
            var num = -1;
            if (!int.TryParse(username, out num))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Tên tài khoản phải là chữ số!");
            return true;
        }

        public async Task<IdentityResult> UpdateUser(string id, AspNetUsersUpdateModel modelInput)
        {
            var entity = await _userManager.FindByIdAsync(id);
            if (modelInput.Id != id|| entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!!");

            if (modelInput.Address != null)
                modelInput.Address = await _tradeZoneMapService.GetAddress(modelInput.Address);

            var model = new AspNetUsersUpdateModel
            {
                Address = modelInput.Address,
                DateOfBirth = modelInput.DateOfBirth,
                Email = modelInput.Email,
                Gender = modelInput.Gender,
                Name = modelInput.Name,
                Id = modelInput.Id
            };

            var updateEntity = _mapper.CreateMapper().Map(model, entity);
            var result = await _userManager.UpdateAsync(updateEntity);
            if (result.Succeeded)
                return IdentityResult.Success;
            return IdentityResult.Failed(errors: result.Errors.ToArray());
        }

        public async Task<string> UpdateImage(string id, AspNetUsersUpdateImageInputModel modelInput)
        {
            var entity = await _userManager.FindByIdAsync(id);
            if (entity.Id != id || modelInput.Id != entity.Id || entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");

            var folderUpload = "Avatar";
            var avatar = await _firebaseService.UploadFileToFirebase(modelInput.Image, folderUpload);

            if (avatar == null)
                avatar = entity.Image;

            var model = new AspNetUsersUpdateImageModel
            {
                Id = id,
                Image = avatar
            };

            var updateEntity = _mapper.CreateMapper().Map(model, entity);
            var result = await _userManager.UpdateAsync(updateEntity);
            if (result.Succeeded)
                return avatar;
            return null;
        }

        public async Task UpdateImageForCustomer(string id, AspNetUsersUpdateImageModel model)
        {
            var entity = await _userManager.FindByIdAsync(id);
            if (entity.Id != id || model.Id != entity.Id || entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            var updateEntity = _mapper.CreateMapper().Map(model, entity);
            await _userManager.UpdateAsync(updateEntity);
        }
        public async Task<TokenModel> Login(AspNetUserLoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                user = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == model.Username);

            var valid = await _signInManager.UserManager.CheckPasswordAsync(user, model.Password);
            if (!valid || user == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Tài khoản hoặc mật khẩu không đúng!");

            var token = await _userManager.GetAuthenticationTokenAsync(user, "Login", user.UserName);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (token == null)
            {
                var newToken = await GenerateToken(user.UserName, role);
                var result = await _userManager.SetAuthenticationTokenAsync(user, "Login", user.UserName, newToken.Token);
                if (result.Succeeded)
                    return newToken;
            }
            var expires = new JwtSecurityTokenHandler().ReadToken(token).ValidTo;
            if (DateTime.Now < expires)
                return new TokenModel
                {
                    TokenType = "Bearer",
                    Token = token,
                    Expires = expires,
                    User = new AspNetUsersModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Role = role,
                        Email = user.Email,
                        Name = user.Name,
                        Address = user.Address,
                        Image = user.Image,
                        Gender = user.Gender,
                        PhoneNumber = user.PhoneNumber,
                        DateOfBirth = user.DateOfBirth,
                        Type = user.Type,
                    },
                };
            var createToken = await GenerateToken(user.UserName, role);
            var createResult = await _userManager.SetAuthenticationTokenAsync(user, "Login", user.UserName, createToken.Token);
            if (createResult.Succeeded)
                return createToken;
            return null;
        }

        public async Task<IdentityResult> ChangePassword(AspNetUsersChangePasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (model.Password.Contains(" "))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Không được có khoảng trắng!");
            if (user == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            if (model.CurrentPassword == model.Password)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu trùng!");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
            if (result.Succeeded)
                return IdentityResult.Success;
            else
            {
                var error = result.Errors.FirstOrDefault();
                if (error.Description == "Passwords must have at least one uppercase ('A'-'Z').")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải có một ký tự in hoa!");
                if (error.Description == "Passwords must have at least one lowercase ('a'-'z').")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải có một ký tự thường!");
                if (error.Description == "Passwords must be at least 8 characters.")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu phải ít nhất 8 ký tự!");
                if (error.Description == "Incorrect password.")
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Mật khẩu hiện tại không đúng!");
            }
            return null;
        }

        public async Task<IdentityResult> ConfirmForgotPassword(string username, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                user = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == username);
            if (user == null)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Tài khoản: {username} không tồn tại!");

            if (newPassword.Contains(" "))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Không được có khoảng trắng!");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
                return IdentityResult.Success;
            else
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Mật khẩu phải bao gồm ít nhất 1 chữ hoa, 1 chữ thường và chứa ít nhất 8 ký tự!");
        }

        public async Task<string> GetNameOfUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user.Name;
        }

        public async Task<AspNetUsersViewModel> GetUserId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy");
            return new AspNetUsersViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Image = user.Image,
                Phone = user.PhoneNumber,
                Type = user.Type,
                Active = user.Active
            };
        }

        public UserCountModel CountUser()
        {
            var users = _userManager.Users;

            return new UserCountModel
            {
                Customer = users.Where(x => x.AspNetUserRoles.Any(y => y.Role.Name == "customer")).Count(),
                Farmer = users.Where(x => x.AspNetUserRoles.Any(y => y.Role.Name == "farmer")).Count(),
                WarehouseManager = users.Where(x => x.AspNetUserRoles.Any(y => y.Role.Name == "warehouseManager")).Count(),
                Driver = users.Where(x => x.AspNetUserRoles.Any(y => y.Role.Name == "driver")).Count()
            };
        }

        public async Task<DynamicModelsResponse<UserDataMapModel>> GetUserByRole(string role, UserDataMapModel model, int page, int size)
        {
            var users = await _userManager.Users.Where(x => x.AspNetUserRoles.Any(y => y.Role.Name == role)).ProjectTo<UserDataMapModel>(_mapper)
                .DynamicFilter(model).Select<UserDataMapModel>(UserDataMapModel.Fields.ToArray().ToDynamicSelector<UserDataMapModel>()).ToListAsync();
            
            var listPaging = users.PagingList(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            var result = new DynamicModelsResponse<UserDataMapModel>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = listPaging.Item1 },
                Data = listPaging.Item2
            };
            return result;
        }

        public async Task BanAndUnBanUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user.Active == true)
                user.Active = false;
            else
                user.Active = true;

            await _userManager.UpdateAsync(user);
        }
    }
}
