using System.ComponentModel.DataAnnotations;

namespace MongoShop.Models.Account
{

    public class UpdateInformationViewModel
    {
        public string? AddressNumber { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string BirthDay { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
