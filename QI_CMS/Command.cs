//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Command
    {
        public int Id { get; set; }
        public Nullable<int> Device_Id { get; set; }
        public string Command_Content { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public Nullable<int> GroupId { get; set; }
    }
}
