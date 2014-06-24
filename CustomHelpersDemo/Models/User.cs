using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomHelpersDemo.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }

        public string LastName { get; set; }
        
        [CustomHelper.Attributes.Number]
        public int Age { get; set; }

        [CustomHelper.Attributes.Email]
        public string Email { get; set; }

        [CustomHelper.Attributes.Date]
        public DateTime DateBorn { get; set; }

        public string Desciption { get; set; }
    }
}