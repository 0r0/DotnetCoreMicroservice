﻿using Shopping.Web.Models;
using Shopping.Web.Services;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;

        public IndexModel(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _catalogService.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var product = await _catalogService.GetCatalog(productId);

            var userName = "swn";
            var basket = await _basketService.GetBasket(userName);

            basket.Items.ToList().Add(new BasketItemModel
            {
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                Color = "Black"
            });

            var basketUpdated = await _basketService.UpdateBasket(basket);
            return RedirectToPage("Cart");
        }
    }
}