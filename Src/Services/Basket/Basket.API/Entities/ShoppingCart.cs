namespace Basket.API.Entities;

public class ShoppingCart
{
    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    public ShoppingCart()
    {
    }

    public string UserName { get; set; }
    public IReadOnlyCollection<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();


    public decimal TotalPrice => Items.Aggregate(0m, (current, shoppingCartItem) => current + shoppingCartItem.Price * shoppingCartItem.Quantity);
}