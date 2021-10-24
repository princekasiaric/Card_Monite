using System;

namespace CardMon.Core.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string IV { get; set; } 
        public string ApiKey { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? LastUpdated { get; set; }
    }
}
