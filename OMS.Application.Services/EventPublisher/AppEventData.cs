using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
