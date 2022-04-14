using DiCho.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class AspNetUsersModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Type { get; set; }
    }

    public class AspNetUserCreateManager
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
        public virtual ICollection<AspNetRolesModel> Role { get; set; }
    }

    public class AspNetUsersCreateModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public virtual ICollection<AspNetRolesModel> Role { get; set; }
    }

    public class AspNetUsersUpdateModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
    
    public class AspNetUsersViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Type { get; set; }
        public bool Active { get; set; }
    }

    public class AspNetUsersUpdateImageModel
    {
        public string Id { get; set; }
        public string Image { get; set; }
    }

    public class AspNetUsersUpdateImageInputModel
    {
        public string Id { get; set; }
        public IFormFile Image { get; set; }
    }

    public class AspNetUsersConfirmPasswordModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }

    public class AspNetUsersChangePasswordModel
    {
        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
    }

    public class UserModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class UserMappingModel
    {
        [BindNever]
        public string Name { get; set; }
        [BindNever]
        public string Image { get; set; }
    }

    public class UserCountModel
    {
        public int Customer { get; set; }
        public int Farmer { get; set; }
        public int WarehouseManager { get; set; }
        public int Driver { get; set; }
    }

    public class UserDataMapModel
    {
        public static string[] Fields = {
            "Id", "Name", "PhoneNumber", "Image", "Gender", "DateOfBirth", "Address", "CreateAt", "Active", "Type"
        };
        [BindNever]
        public string Id { get; set; }
        [StringAttribute]
        public string Name { get; set; }
        [BindNever]
        public string PhoneNumber { get; set; }
        [BindNever]
        public string Image { get; set; }
        [BindNever]
        public string Gender { get; set; }
        [BindNever]
        public DateTime? DateOfBirth { get; set; }
        [BindNever]
        public string Address { get; set; }
        [BindNever]
        public DateTime? CreateAt { get; set; }
        public bool? Active { get; set; }
        [BindNever]
        public int? Type { get; set; }
    }

    public class UserDriverModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool Active { get; set; }
        public int? Type { get; set; }
        public string Status { get; set; }
    }

    public class CustomerOrder
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class DashBoardOfFarmer
    {
        public int Farms { get; set; }
        public int Harvests { get; set; }
        public int OrderConfirms { get; set; }
        public int CustomerOrder { get; set; }
    }

    public class DashBoardOfAdmin
    {
        public int Customers { get; set; }
        public int Campaigns { get; set; }
        public int Farmers { get; set; }
        public int Orders { get; set; }
        public int Products { get; set; }
        public int WarehouseManagers { get; set; }
        public int Warehouses { get; set; }
    }

    public class CreateUserForAdmin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public virtual ICollection<AspNetRolesModel> Role { get; set; }
    }

    public class CreateUserForDelivery
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Type { get; set; }
        public int WarehouseId { get; set; }
        public virtual ICollection<AspNetRolesModel> Role { get; set; }
    }

    public class StatisticalOfFarmer
    {
        public double TotalRevenues { get; set; }
        public int Customers { get; set; }
        public int FarmOrders { get; set; }
    }

    public class RevenuseOfFarmer
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public double TotalRevenues { get; set; }
        public int CountFarmOrder { get; set; }
        public List<FarmOrderRevenuseModel> FarmOrders { get; set; }
    }

    public class UserSearchModel
    {
        public string Name { get; set; }
        public bool? Active { get; set; }
    }

    public class InfomationOfUserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int CountFarmOrders { get; set; }
    }
}
