//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Scenery
    {
        public int Id { get; set; }
        public Nullable<int> MenuId { get; set; }
        public string SceneryName { get; set; }
        public string SceneryImgUrl { get; set; }
        public string Scenerydetails { get; set; }
        public Nullable<int> CreateUserId { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<int> ModifyUserId { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public Nullable<int> IsDele { get; set; }
        public Nullable<int> OrderBy { get; set; }
        public string MenuName { get; set; }
    
        public virtual Menu Menu { get; set; }
    }
}
