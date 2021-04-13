using Microsoft.AspNetCore.Components;
using MongoShop.SharedModels.Category;
using MongoShop.SharedModels.Product;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MongoShop.UI.Pages.Product
{
    public class EditProductBase : ComponentBase
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public HttpClient HttpClient{ get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public ProductViewModel Product { get; set; }

        public CategoryViewModel SelectedCategory { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Categories = await HttpClient.GetFromJsonAsync<List<CategoryViewModel>>("api/Category/GetAll");
            Product = await HttpClient.GetFromJsonAsync<ProductViewModel>($"api/Product/GetById/{Id}");
        }

        public async Task HandleValidSubmit()
        {
            var response = await HttpClient.PutAsJsonAsync<ProductViewModel>($"api/Product/Edit/{Id}" ,Product);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
