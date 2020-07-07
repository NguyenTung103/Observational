using ES_CapDien.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ES_CapDien.Models
{
    ///Model
    /// 
   
    public partial class GroupModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên nhóm không để trống")]
        public string Name { get; set; }
        public string Contact { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string Mobile { get; set; }
        public string NguoiTao { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
    public partial class DataObservationModel
    {
        public int Device_Id { get; set; }
        public double BTI { get; set; }
        public double BHU { get; set; }
        public double BTO { get; set; }
        public double BDR { get; set; }
        public double BFL { get; set; }
        public double BFR { get; set; }
        public double BPS { get; set; }
        public double BAV { get; set; }
        public double BAP { get; set; }
        public double BAC { get; set; }
        public double BAF { get; set; }
        public double BV1 { get; set; }
        public double BC1 { get; set; }
        public double BT1 { get; set; }
        public double BV2 { get; set; }
        public double BC2 { get; set; }
        public double BT2 { get; set; }
        public double BSE { get; set; }
        public double BA1 { get; set; }
        public double BB1 { get; set; }
        public double BA2 { get; set; }
        public double BB2 { get; set; }
        public double BA3 { get; set; }
        public double BB3 { get; set; }
        public double BA4 { get; set; }
        public double BB4 { get; set; }
        public double BFA { get; set; }
        public double BFD { get; set; }
        public double BPW { get; set; }
        public Nullable<bool> IsSEQ { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public double BWS { get; set; }
        public string NameSite { get; set; }
    }
    public partial class AreaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên khu vực không để trống")]
        public string Name { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public int Group_Id { get; set; }
        public bool IsActive { get; set; }
        public string NguoiTao { get; set; }
        public string GroupsName { get; set; }
    }
    public partial class SiteModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Khu vực không được để trống")]
        public int Area_Id { get; set; }
        public int Group_Id { get; set; }
        [Required(ErrorMessage = "Tên trạm không được để trống")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string TimeZone { get; set; }
        public System.DateTime CreateDay { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Id thiết bị không được để trống")]
        public Nullable<int> DeviceId { get; set; }
        public string NameGroups { get; set; }
        public string NameAreas { get; set; }
        public string NguoiTao { get; set; }
        public string GroupsName { get; set; }
        public string AreasName { get; set; }
    }
    public partial class ObservationsModel
    {
        public int Id { get; set; }
        public Nullable<bool> Noti_Alarm { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public string Low_Value { get; set; }
        public string Hight_Value { get; set; }
        public Nullable<bool> IsBieuDo { get; set; }
    }
    public partial class RegisterSMSModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Danh sách số điện thoại không được để trống")]

        public string DanhSachSDT { get; set; }
        [Required(ErrorMessage = "Danh sách cảnh báo đăng ký không được để trống")]
        public string CodeObservation { get; set; }

        public Nullable<System.DateTime> DateCreate { get; set; }

        public Nullable<System.DateTime> DateSend { get; set; }
        [Required(ErrorMessage = "Tên trạm không được để trông")]
        public Nullable<int> DeviceId { get; set; }
        [Required(ErrorMessage = "Nhóm không được để trông")]
        public Nullable<int> GroupId { get; set; }

        public Nullable<bool> IsAll { get; set; }
        [Required(ErrorMessage = "Server SMS không được để trống")]
        public int? SMSServerId { get; set; }

        public string SMSServer{ get; set; }
        public string NameSite { get; set; }
        public string GroupName { get; set; }
    }
    public partial class HomeModel
    {
        public int ThietBiKhongHoatDong { get; set; }
        public int ThietBiHoatDong { get; set; }
        public int ThietBiCanhBao { get; set; }        
    }
    public partial class DataAlarmMongo
    {
        public string Id { get; set; }
        public int Device_Id { get; set; }
        public string AMATI { get; set; }
        public string AMIHU { get; set; }
        public string AMADR { get; set; }
        public string AMAFL { get; set; }
        public string AMAFR { get; set; }
        public string AMIPS { get; set; }
        public string AMIAL { get; set; }
        public string AMIAH { get; set; }
        public string AMIAP { get; set; }
        public string AMIAC { get; set; }
        public string AMIGN { get; set; }
        public string AMIAR { get; set; }
        public string AMIL1 { get; set; }
        public string AMIH1 { get; set; }
        public string AMIT1 { get; set; }
        public string AMIL2 { get; set; }
        public string AMIH2 { get; set; }
        public string AMIT2 { get; set; }
        public System.DateTime TimeSend { get; set; }
        public string NameSite { get; set; }
    }
    public class UserProfileModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không để trống")]
        public string UserName { get; set; }
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email không để trống")]
        public string Email { get; set; }        
        public string PhoneNumber { get; set; }
        public string SkypeID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public Nullable<int> GenderId { get; set; }
        public Nullable<int> Group_Id { get; set; }
        public string CmisUserType { get; set; }       
        public bool? IsActive { get; set; }                
        [Required(ErrorMessage = "Mật khẩu không để trống")]
        public string Password { get; set; }
        //custom
        public bool? Isroot { get; set; }
        public string NameGroup { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không để trống")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class UserModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? Group_Id { get; set; }      

    }
    public class RoleModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string GroupId { get; set; }
        public IEnumerable<TreeItem<Administrator_MenuModel>> administratorMenuTree { get; set; }
    }
    public class Administrator_MenuModel
    {
        public Guid MenuId { get; set; }
        public Guid PageId { get; set; }
        public bool isHas { get; set; }
        public System.Guid ParentId { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
    }
    public class ReportMonthlyModel
    {
        public int Id { get; set; }
        public Nullable<int> DeviceId { get; set; }    
        public double? Distance1 { get; set; }
        public string Name{ get; set; }
        public string Title { get; set; }
        public string Measure { get; set; }
        public double? Distance2 { get; set; }
        public double? Distance3 { get; set; }   
        public int dayInMonth { get; set; }
    }
    public class ReportExcelModel
    {
        public int Id { get; set; }       
        public Nullable<int> NameTram { get; set; }       
        public Nullable<System.DateTime> DateReport { get; set; }   
        public string Measure { get; set; }
        public Nullable<System.DateTime> DateRequestReport { get; set; }
        public double? MaxValue { get; set; }
        public double? MinValue { get; set; }
        public Nullable<System.DateTime> TimeMaxValue { get; set; }
        public Nullable<System.DateTime> TimeMinValue { get; set; }
        public double? Distance1 { get; set; }
        public double? Distance2 { get; set; }
        public double? Distance3 { get; set; }
        public double? Distance4 { get; set; }
        public double? Distance5 { get; set; }
        public double? Distance6 { get; set; }
        public double? Distance7 { get; set; }
        public double? Distance8 { get; set; }
        public double? Distance9 { get; set; }
        public double? Distance10 { get; set; }
        public double? Distance11 { get; set; }
        public double? Distance12 { get; set; }
        public double? Distance13 { get; set; }
        public double? Distance14 { get; set; }
        public double? Distance15 { get; set; }
        public double? Distance16 { get; set; }
        public double? Distance17 { get; set; }
        public double? Distance18 { get; set; }
        public double? Distance19 { get; set; }
        public double? Distance20 { get; set; }
        public double? Distance21 { get; set; }
        public double? Distance22 { get; set; }
        public double? Distance23 { get; set; }
        public double? Distance24 { get; set; }       
    }
    public class Report_NhietDo_DoAm_ApSuat_DailyModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<int> DeviceId { get; set; }

        public string ReportTypeCode { get; set; }
        public string ReportTypeName { get; set; }

        public Nullable<System.DateTime> DateReport { get; set; }
        
        public string ContentReport { get; set; }
        public string Measure { get; set; }

        public Nullable<System.DateTime> DateRequestReport { get; set; }

        public double? MaxValue { get; set; }

        public double? MinValue { get; set; }

        public Nullable<System.DateTime> TimeMaxValue { get; set; }

        public Nullable<System.DateTime> TimeMinValue { get; set; }

        public double? Distance1 { get; set; }              
        public double? Distance2 { get; set; }               
        public double? Distance3 { get; set; }               
        public double? Distance4 { get; set; }               
        public double? Distance5 { get; set; }               
        public double? Distance6 { get; set; }               
        public double? Distance7 { get; set; }               
        public double? Distance8 { get; set; }            
        public double? Distance9 { get; set; }             
        public double? Distance10 { get; set; }               
        public double? Distance11 { get; set; }
        public double? Distance12 { get; set; }               
        public double? Distance13 { get; set; }               
        public double? Distance14 { get; set; }               
        public double? Distance15 { get; set; }               
        public double? Distance16 { get; set; }             
        public double? Distance17 { get; set; }           
        public double? Distance18 { get; set; }            
        public double? Distance19 { get; set; }            
        public double? Distance20 { get; set; }          
        public double? Distance21 { get; set; }             
        public double? Distance22 { get; set; }            
        public double? Distance23 { get; set; }            
        public double? Distance24 { get; set; }
        public string data { get; }
        public string series { get; }
    }
    public class Report_MucNuoc_DailyModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public string ReportTypeCode { get; set; }
        public string ReportTypeName { get; set; }
        public Nullable<System.DateTime> DateReport { get; set; }
        public string ContentReport { get; set; }
        public string Measure { get; set; }              

        public Nullable<System.DateTime> DateRequestReport { get; set; }

        public Nullable<System.DateTime> TimeMaxValue { get; set; }

        public Nullable<System.DateTime> TimeMinValue { get; set; }
        public Nullable<double> MaxValue { get; set; }

        public Nullable<double> MinValue { get; set; }

        public Nullable<double> Distance1 { get; set; }

        public Nullable<double> Distance2 { get; set; }

        public Nullable<double> Distance3 { get; set; }

        public Nullable<double> Distance4 { get; set; }

        public Nullable<double> Distance5 { get; set; }

        public Nullable<double> Distance6 { get; set; }

        public Nullable<double> Distance7 { get; set; }

        public Nullable<double> Distance8 { get; set; }

        public Nullable<double> Distance9 { get; set; }

        public Nullable<double> Distance10 { get; set; }

        public Nullable<double> Distance11 { get; set; }

        public Nullable<double> Distance12 { get; set; }

        public Nullable<double> Distance13 { get; set; }

        public Nullable<double> Distance14 { get; set; }

        public Nullable<double> Distance15 { get; set; }

        public Nullable<double> Distance16 { get; set; }

        public Nullable<double> Distance17 { get; set; }

        public Nullable<double> Distance18 { get; set; }

        public Nullable<double> Distance19 { get; set; }

        public Nullable<double> Distance20 { get; set; }

        public Nullable<double> Distance21 { get; set; }

        public Nullable<double> Distance22 { get; set; }

        public Nullable<double> Distance23 { get; set; }

        public Nullable<double> Distance24 { get; set; }
        public string data { get; }
        public string series { get; }
    }
    public class Report_LuongMuc_DailyModel
    {
        public string Title { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public string ReportTypeCode { get; set; }
        public string ReportTypeName { get; set; }
        public Nullable<System.DateTime> DateReport { get; set; }
        public string ContentReport { get; set; }
        public string Measure { get; set; }

        public Nullable<System.DateTime> DateRequestReport { get; set; }

        public Nullable<double> TongLuongMua { get; set; }

        public Nullable<System.DateTime> TimeMaxValue { get; set; }

        public Nullable<System.DateTime> TimeMinValue { get; set; }

        public Nullable<double> Distance1 { get; set; }

        public Nullable<double> Distance2 { get; set; }

        public Nullable<double> Distance3 { get; set; }

        public Nullable<double> Distance4 { get; set; }

        public Nullable<double> Distance5 { get; set; }

        public Nullable<double> Distance6 { get; set; }

        public Nullable<double> Distance7 { get; set; }

        public Nullable<double> Distance8 { get; set; }

        public Nullable<double> Distance9 { get; set; }

        public Nullable<double> Distance10 { get; set; }

        public Nullable<double> Distance11 { get; set; }

        public Nullable<double> Distance12 { get; set; }

        public Nullable<double> Distance13 { get; set; }

        public Nullable<double> Distance14 { get; set; }

        public Nullable<double> Distance15 { get; set; }

        public Nullable<double> Distance16 { get; set; }

        public Nullable<double> Distance17 { get; set; }

        public Nullable<double> Distance18 { get; set; }

        public Nullable<double> Distance19 { get; set; }

        public Nullable<double> Distance20 { get; set; }

        public Nullable<double> Distance21 { get; set; }

        public Nullable<double> Distance22 { get; set; }

        public Nullable<double> Distance23 { get; set; }

        public Nullable<double> Distance24 { get; set; }
    }
    public class Report_TocDoGio_DailyModel
    {
        public string Title { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public string ReportTypeCode { get; set; }
        public string ReportTypeName { get; set; }
        public string Measure { get; set; }
        public Nullable<System.DateTime> DateReport { get; set; }
        public string ContentReport { get; set; }

        public Nullable<System.DateTime> DateRequestReport { get; set; }

        public Nullable<System.DateTime> TimeMaxValue { get; set; }

        public Nullable<System.DateTime> TimeMinValue { get; set; }

        public Nullable<double> Distance1 { get; set; }

        public Nullable<double> Distance2 { get; set; }

        public Nullable<double> Distance3 { get; set; }

        public Nullable<double> Distance4 { get; set; }

        public Nullable<double> Distance5 { get; set; }

        public Nullable<double> Distance6 { get; set; }

        public Nullable<double> Distance7 { get; set; }

        public Nullable<double> Distance8 { get; set; }

        public Nullable<double> Distance9 { get; set; }

        public Nullable<double> Distance10 { get; set; }

        public Nullable<double> Distance11 { get; set; }

        public Nullable<double> Distance12 { get; set; }

        public Nullable<double> Distance13 { get; set; }

        public Nullable<double> Distance14 { get; set; }

        public Nullable<double> Distance15 { get; set; }

        public Nullable<double> Distance16 { get; set; }

        public Nullable<double> Distance17 { get; set; }

        public Nullable<double> Distance18 { get; set; }

        public Nullable<double> Distance19 { get; set; }

        public Nullable<double> Distance20 { get; set; }

        public Nullable<double> Distance21 { get; set; }

        public Nullable<double> Distance22 { get; set; }

        public Nullable<double> Distance23 { get; set; }

        public Nullable<double> Distance24 { get; set; }

        public double TocDoGioLonNhat { get; set; }

        public Nullable<double> TocDoGioNhoNhat { get; set; }

        public Nullable<double> HuongGioCuaTocDoLonNhat { get; set; }

        public Nullable<double> HuongGioCuarTocDoNhoNhat { get; set; }
    }
    public class Report_HuongGio_DailyModel
    {
        public string Title { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public string ReportTypeCode { get; set; }
        public string ReportTypeName { get; set; }
        public string Measure { get; set; }

        public Nullable<System.DateTime> DateReport { get; set; }

        public string ContentReport { get; set; }

        public Nullable<System.DateTime> DateRequestReport { get; set; }

        public string HuongGioDacTrungNhieuNhat { get; set; }

        public string HuongGioDacTrungNhieuThuHai { get; set; }

        public Nullable<System.DateTime> TimeMaxValue { get; set; }

        public Nullable<System.DateTime> TimeMinValue { get; set; }

        public Nullable<double> Distance1 { get; set; }

        public Nullable<double> Distance2 { get; set; }

        public Nullable<double> Distance3 { get; set; }

        public Nullable<double> Distance4 { get; set; }

        public Nullable<double> Distance5 { get; set; }

        public Nullable<double> Distance6 { get; set; }

        public Nullable<double> Distance7 { get; set; }

        public Nullable<double> Distance8 { get; set; }

        public Nullable<double> Distance9 { get; set; }

        public Nullable<double> Distance10 { get; set; }

        public Nullable<double> Distance11 { get; set; }

        public Nullable<double> Distance12 { get; set; }

        public Nullable<double> Distance13 { get; set; }

        public Nullable<double> Distance14 { get; set; }

        public Nullable<double> Distance15 { get; set; }

        public Nullable<double> Distance16 { get; set; }

        public Nullable<double> Distance17 { get; set; }

        public Nullable<double> Distance18 { get; set; }

        public Nullable<double> Distance19 { get; set; }

        public Nullable<double> Distance20 { get; set; }

        public Nullable<double> Distance21 { get; set; }

        public Nullable<double> Distance22 { get; set; }

        public Nullable<double> Distance23 { get; set; }

        public Nullable<double> Distance24 { get; set; }

        public Nullable<int> HuongGio1 { get; set; }

        public Nullable<int> HuongGio2 { get; set; }

        public Nullable<int> HuongGio3 { get; set; }

        public Nullable<int> HuongGio4 { get; set; }

        public Nullable<int> HuongGio5 { get; set; }

        public Nullable<int> HuongGio6 { get; set; }

        public Nullable<int> HuongGio7 { get; set; }

        public Nullable<int> HuongGio8 { get; set; }
    }
}