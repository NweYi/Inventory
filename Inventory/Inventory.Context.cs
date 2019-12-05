﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class InventoryDBEntities : DbContext
    {
        public InventoryDBEntities()
            : base("name=InventoryDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<S_AdjustType> S_AdjustType { get; set; }
        public virtual DbSet<S_Branch> S_Branch { get; set; }
        public virtual DbSet<S_CompanySetting> S_CompanySetting { get; set; }
        public virtual DbSet<S_Customer> S_Customer { get; set; }
        public virtual DbSet<S_Location> S_Location { get; set; }
        public virtual DbSet<S_MainMenu> S_MainMenu { get; set; }
        public virtual DbSet<S_Payment> S_Payment { get; set; }
        public virtual DbSet<S_Product> S_Product { get; set; }
        public virtual DbSet<S_ProductBranch> S_ProductBranch { get; set; }
        public virtual DbSet<S_ProductUnit> S_ProductUnit { get; set; }
        public virtual DbSet<S_SubMenu> S_SubMenu { get; set; }
        public virtual DbSet<S_Supplier> S_Supplier { get; set; }
        public virtual DbSet<S_Township> S_Township { get; set; }
        public virtual DbSet<S_Unit> S_Unit { get; set; }
        public virtual DbSet<S_User> S_User { get; set; }
        public virtual DbSet<S_UserRightEntry> S_UserRightEntry { get; set; }
        public virtual DbSet<S_UserRightReport> S_UserRightReport { get; set; }
        public virtual DbSet<S_UserRightSetup> S_UserRightSetup { get; set; }
        public virtual DbSet<S_VoucherFormat> S_VoucherFormat { get; set; }
        public virtual DbSet<S_VoucherSetting> S_VoucherSetting { get; set; }
        public virtual DbSet<Sys_Admin> Sys_Admin { get; set; }
        public virtual DbSet<Sys_AppWord> Sys_AppWord { get; set; }
        public virtual DbSet<Sys_Currency> Sys_Currency { get; set; }
        public virtual DbSet<Sys_EntryModule> Sys_EntryModule { get; set; }
        public virtual DbSet<Sys_Language> Sys_Language { get; set; }
        public virtual DbSet<Sys_MainLanguage> Sys_MainLanguage { get; set; }
        public virtual DbSet<Sys_MainModule> Sys_MainModule { get; set; }
        public virtual DbSet<Sys_ProductNature> Sys_ProductNature { get; set; }
        public virtual DbSet<Sys_ReportModule> Sys_ReportModule { get; set; }
        public virtual DbSet<Sys_SetupModule> Sys_SetupModule { get; set; }
        public virtual DbSet<Sys_TranID> Sys_TranID { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<T_Delivery> T_Delivery { get; set; }
        public virtual DbSet<T_MasterSale> T_MasterSale { get; set; }
        public virtual DbSet<T_TranSale> T_TranSale { get; set; }
    
        public virtual ObjectResult<Nullable<int>> PrcValidateUser(Nullable<int> userID, string userPassword)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var userPasswordParameter = userPassword != null ?
                new ObjectParameter("UserPassword", userPassword) :
                new ObjectParameter("UserPassword", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("PrcValidateUser", userIDParameter, userPasswordParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> PrcValidateAdmin(string adminName, string adminPassword)
        {
            var adminNameParameter = adminName != null ?
                new ObjectParameter("AdminName", adminName) :
                new ObjectParameter("AdminName", typeof(string));
    
            var adminPasswordParameter = adminPassword != null ?
                new ObjectParameter("AdminPassword", adminPassword) :
                new ObjectParameter("AdminPassword", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("PrcValidateAdmin", adminNameParameter, adminPasswordParameter);
        }
    }
}
