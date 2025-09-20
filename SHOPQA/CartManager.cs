using System;
using System.Collections.Generic;
using System.Linq;

public class CartManager
{
    private List<CartItem> cartItems;

    public event Action CartChanged;

    public CartManager()
    {
        cartItems = new List<CartItem>();
    }

    public void AddToCart(Product product)
    {
        var existingItem = cartItems.FirstOrDefault(item => item.Product.Id == product.Id);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            cartItems.Add(new CartItem { Product = product, Quantity = 1 });
        }

        CartChanged?.Invoke();
    }

    public void RemoveFromCart(int productId)
    {
        var item = cartItems.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            if (item.Quantity > 1)
                item.Quantity--;
            else
                cartItems.Remove(item);

            CartChanged?.Invoke();
        }
    }

    public void ClearCart()
    {
        cartItems.Clear();
        CartChanged?.Invoke();
    }

    public List<CartItem> GetCartItems() => cartItems.ToList();

    public decimal GetTotalAmount() => cartItems.Sum(item => item.TotalPrice);

    public int GetTotalQuantity() => cartItems.Sum(item => item.Quantity);
}