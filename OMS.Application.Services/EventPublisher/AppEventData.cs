using System.ComponentModel.DataAnnotations;

namespace OMS.Application.Services.EventPublisher
{
    public class AppOutBox
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public int RetryCount { get; set; }
        public bool Published { get; set; }
        public DateTime Date { get; set; }
    }
}
