﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TravelPro.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TraverBDEntities : DbContext
    {
        public TraverBDEntities()
            : base("name=TraverBDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Admin> Admins { get; set; }
        public DbSet<food> foods { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Scenery> Sceneries { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<TopBanner> TopBanners { get; set; }
    }
}
