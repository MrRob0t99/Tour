using System;
using System.ComponentModel.DataAnnotations;

namespace TourServer.Models
{
    public class EmailToken: BaseEntity
    {

        public Guid Token { get; set; }

        public string UserId { get; set; }
    }
}
