using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

namespace ES_CapDien.Repository
{   
    public class UserActionLogInfo
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Actor { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
}