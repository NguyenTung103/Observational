using ES_CapDien.Helpers;
using HelperLibrary;
using PagedList;
using System;
using System.Collections.Generic;

namespace ES_CapDien.Models
{
    /// <summary>
    /// Hiển thị dữ liệu và phân trang
    /// </summary>
    public class GroupsViewModel
    {
        public StaticPagedList<GroupModel> Groups { get; set; }           
        public PagingInfo PagingInfo { get; set; }
    }
    /// <summary>
    /// Hiển thị dữ liệu và phân trang
    /// </summary>
    public class ReportDailyViewModel
    {        
        public DateTime Date { get; set; }
        public int? DeviceId { get; set; }
        public int? AreaId { get; set; }
        public int? GroupId { get; set; }
        public Report_NhietDo_DoAm_ApSuat_DailyModel ApSuat { get; set; }
        public Report_NhietDo_DoAm_ApSuat_DailyModel DoAm { get; set; }
        public Report_HuongGio_DailyModel HuongGio { get; set; }
        public Report_LuongMuc_DailyModel LuongMua { get; set; }
        public Report_MucNuoc_DailyModel MucNuoc { get; set; }
        public Report_TocDoGio_DailyModel TocDoGio { get; set; }
        public Report_NhietDo_DoAm_ApSuat_DailyModel NhietDo { get; set; }
    }
    /// <summary>
    /// Hiển thị dữ liệu và phân trang
    /// </summary>
    public class ReportMonthlyViewModel
    {
        public DateTime Date { get; set; }
        public int? DeviceId { get; set; }
        public int? AreaId { get; set; }
        public int? GroupId { get; set; }   
        public ReportMonthlyModel data { get; set; }
    }
    /// <summary>
    /// Hiển thị dữ liệu và phân trang
    /// </summary>
    public class AreaViewModel
    {
        public StaticPagedList<AreaModel> Areas { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class ObservationsViewModel
    {
        public StaticPagedList<ObservationsModel> Observations { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    /// <summary>
    /// Hiển thị dữ liệu và phân trang
    /// </summary>
    public class SitesViewModel
    {
        public StaticPagedList<SiteModel> Sites { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class RegisterSMSViewModel
    {
        public StaticPagedList<RegisterSMSModel> RG { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class UserProfileViewModel
    {
        public StaticPagedList<UserProfileModel> UsP { get; set; }
        public PagingInfo PagingInfo { get; set; }        
    }
    public class RoleViewModel
    {
        public StaticPagedList<RoleModel> Role { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class DataObservationViewModel
    {
        public StaticPagedList<DataObservationModel> DataO { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

}