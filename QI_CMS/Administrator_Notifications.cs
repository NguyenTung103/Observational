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
    
    public partial class Administrator_Notifications
    {
        public int NotificationId { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
    }
}
