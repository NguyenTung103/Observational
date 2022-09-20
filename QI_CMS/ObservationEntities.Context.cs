﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ES_CapDien
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ObservationEntities : DbContext
    {
        public ObservationEntities()
            : base("name=ObservationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Administrator_Menu> Administrator_Menu { get; set; }
        public virtual DbSet<Administrator_Notifications> Administrator_Notifications { get; set; }
        public virtual DbSet<Administrator_Pages> Administrator_Pages { get; set; }
        public virtual DbSet<Administrator_RoleGroups> Administrator_RoleGroups { get; set; }
        public virtual DbSet<Alarm> Alarms { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<CategoryTypeSite> CategoryTypeSites { get; set; }
        public virtual DbSet<Command> Commands { get; set; }
        public virtual DbSet<DataAlarm> DataAlarms { get; set; }
        public virtual DbSet<DataObservation> DataObservations { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Observation> Observations { get; set; }
        public virtual DbSet<RegionalGroup> RegionalGroups { get; set; }
        public virtual DbSet<RegisterSMS> RegisterSMS { get; set; }
        public virtual DbSet<ReportDailyApSuat> ReportDailyApSuats { get; set; }
        public virtual DbSet<ReportDailyBucXaMatTroi> ReportDailyBucXaMatTrois { get; set; }
        public virtual DbSet<ReportDailyDoAm> ReportDailyDoAms { get; set; }
        public virtual DbSet<ReportDailyHuongGio> ReportDailyHuongGios { get; set; }
        public virtual DbSet<ReportDailyLuongMua> ReportDailyLuongMuas { get; set; }
        public virtual DbSet<ReportDailyLuuLuongDongChay> ReportDailyLuuLuongDongChays { get; set; }
        public virtual DbSet<ReportDailyMucNuoc> ReportDailyMucNuocs { get; set; }
        public virtual DbSet<ReportDailyNhietDo> ReportDailyNhietDoes { get; set; }
        public virtual DbSet<ReportDailyTocDoDongChay> ReportDailyTocDoDongChays { get; set; }
        public virtual DbSet<ReportDailyTocDoGio> ReportDailyTocDoGios { get; set; }
        public virtual DbSet<ReportS10> ReportS10 { get; set; }
        public virtual DbSet<ReportType> ReportTypes { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<SiteAdminLog> SiteAdminLogs { get; set; }
        public virtual DbSet<SmsServer> SmsServers { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<UserWebsite_Category> UserWebsite_Category { get; set; }
        public virtual DbSet<webpages_Membership> webpages_Membership { get; set; }
        public virtual DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public virtual DbSet<webpages_Roles> webpages_Roles { get; set; }
        public virtual DbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
        public virtual DbSet<WT_SystemRules> WT_SystemRules { get; set; }
    }
}