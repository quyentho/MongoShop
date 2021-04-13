using Microsoft.AspNetCore.Components;
using MongoShop.SharedModels.Product;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MongoShop.UI.Pages.Product
{
    public class ProductDetailBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        public ProductViewModel Product { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Product = await HttpClient.GetFromJsonAsync<ProductViewModel>($"api/Product/GetById/{Id}");
        }
    }
}
