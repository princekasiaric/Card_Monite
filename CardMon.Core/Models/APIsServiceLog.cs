using System;

namespace CardMon.Core.Models
{
    public class APIsServiceLog
    {
        public long Id { get; set; }
        public int ClientId { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime? CreatedAt { get; set; } 
    }
}
