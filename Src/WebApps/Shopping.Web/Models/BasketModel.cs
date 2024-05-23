namespace Shopping.Web.Models;

public class BasketModel
{
    public string UserName { get; set; }
    public IReadOnlyCollection<BasketItemModel> Items { get; set; } = new List<BasketItemModel>();


    public decimal TotalPrice { get; set; }
}