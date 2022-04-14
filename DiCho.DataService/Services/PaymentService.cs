using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiCho.Core.BaseConnect;
using DiCho.Core.Custom;
using DiCho.Core.Utilities;
using DiCho.DataService.Repositories;
using DiCho.DataService.Response;
using DiCho.DataService.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.Services
{
    public partial interface IPaymentService
    {
        Task<List<PaymentModel>> Gets();
        Task<PaymentModel> GetById(int id);
    }

    public partial class PaymentService
    {
        private readonly IConfigurationProvider _mapper;
        public PaymentService(IPaymentRepository repository,
            IUnitOfWork unitOfWork, IMapper mapper = null) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        public async Task<List<PaymentModel>> Gets()
        {
            var payments = await Get().ProjectTo<PaymentModel>(_mapper).ToListAsync();
            foreach (var payment in payments)
            {
                payment.Status = payment.Status switch
                {
                    "0" => "Chưa thanh toán",
                    "1" => "Đã thanh toán",
                    "2" => "Đã hủy",
                    _ => ""
                };
            }
            return payments;
        }

        public async Task<PaymentModel> GetById(int id)
        {
            var payment = await Get(x => x.Id == id).ProjectTo<PaymentModel>(_mapper).FirstOrDefaultAsync();
            if (payment == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            payment.Status = payment.Status switch
            {
                "0" => "Chưa thanh toán",
                "1" => "Đã thanh toán",
                "2" => "Đã hủy",
                _ => ""
            };
            return payment;
        }
    }
}
