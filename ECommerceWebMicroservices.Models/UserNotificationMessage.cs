using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebMicroservices.Models
{
    public class UserNotificationMessage
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
