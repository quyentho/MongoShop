using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.Models.Account
{
    public class UpdateInformationViewModel
    {
        public string AddressNumber { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public DateTimeOffset BirthDay { get; set; }

        public string PhoneNumber { get; set; }
    }
}
