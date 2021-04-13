using Microsoft.AspNetCore.Components;
using MongoShop.SharedModels.Product;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace MongoShop.UI.Pages.Product
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var response = await HttpClient.GetFromJsonAsync<List<ProductViewModel>>("api/Product/GetAll");

            Products = response;
        }
    }
}