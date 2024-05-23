using Shopping.Web.Models;
using Shopping.Web.Services;

namespace Shopping.Web.Pages;
public class IndexModel
    (ICatalogService catalogService, IBasketService basketService, ILogger<IndexModel> logger)
    : PageModel
{
    public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Index page visited");
        ProductList = await catalogService.GetCatalog();
       
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string productId)
    {
        logger.LogInformation("Add to cart button clicked");

        var productResponse = await catalogService.GetCatalog(productId);
        var userName = "swn";
        var basket = await basketService.GetBasket(userName);

        basket.Items.ToList().Add(new BasketItemModel
        {
            ProductId = productId,
            ProductName = productResponse.Name,
            Price = productResponse.Price,
            Quantity = 1,
            Color = "Black"
        });

        var basketUpdated = await basketService.UpdateBasket(basket);
        
        return RedirectToPage("Cart");
    }
}
