using System.ComponentModel.DataAnnotations;

namespace MongoShop.Models.Account
{

    public class GetMyInformationViewModel
    {
        public string? AddressNumber { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Name { get; set; }
    }
}
