using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoShop.Models.Account
{
    public class AccountProfileViewModel
    {
        [Required]
        public string  Email { get; set; }

        [Required]
        [DisplayName("Full Name")]
        public string Name { get; set; }

        
        [DisplayName("Address number")]
        public string AddressNumber { get; set; }
        
        
        public string Street { get; set; }
        
        
        public string City { get; set; }
        
        public DateTime BirthDay { get; set; }
        
        
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }
    }
}
