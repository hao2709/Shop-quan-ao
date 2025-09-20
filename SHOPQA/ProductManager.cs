using System;
using System.Collections.Generic;
using System.Linq;

public class ProductManager
{
    private List<Product> products;

    public ProductManager()
    {
        InitializeProducts();
    }

    private void InitializeProducts()
    {
        products = new List<Product>
        {
            new Product { Id = 1, Name = "Áo Thun Nam Basic", Price = 250000, Category = "Áo", Size = "M", Color = "Đen", Stock = 15, Description = "Áo thun cotton 100% cao cấp" },
            new Product { Id = 2, Name = "Quần Jean Nữ Skinny", Price = 450000, Category = "Quần", Size = "S", Color = "Xanh", Stock = 12, Description = "Quần jean co giãn thoải mái" },
            new Product { Id = 3, Name = "Áo Sơ Mi Trắng", Price = 350000, Category = "Áo", Size = "L", Color = "Trắng", Stock = 20, Description = "Áo sơ mi công sở lịch sự" },
            new Product { Id = 4, Name = "Váy Đầm Dự Tiệc", Price = 680000, Category = "Váy", Size = "M", Color = "Đỏ", Stock = 8, Description = "Váy đầm sang trọng cho buổi tối" },
            new Product { Id = 5, Name = "Áo Khoác Bomber", Price = 550000, Category = "Áo", Size = "XL", Color = "Đen", Stock = 10, Description = "Áo khoác thời trang unisex" },
            new Product { Id = 6, Name = "Quần Short Thể Thao", Price = 180000, Category = "Quần", Size = "M", Color = "Xanh", Stock = 25, Description = "Quần short thoáng mát" },
            new Product { Id = 7, Name = "Áo Len Cổ Tròn", Price = 420000, Category = "Áo", Size = "L", Color = "Xám", Stock = 14, Description = "Áo len ấm áp mùa đông" },
            new Product { Id = 8, Name = "Chân Váy Mini", Price = 320000, Category = "Váy", Size = "S", Color = "Hồng", Stock = 18, Description = "Chân váy trẻ trung năng động" },
            new Product { Id = 9, Name = "Quần Âu Nam", Price = 380000, Category = "Quần", Size = "L", Color = "Đen", Stock = 16, Description = "Quần âu lịch sự công sở" }
        };
    }

    public List<Product> GetAllProducts() => products;

    public List<Product> SearchProducts(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return products;

        keyword = keyword.ToLower();
        return products.Where(p =>
            p.Name.ToLower().Contains(keyword) ||
            p.Category.ToLower().Contains(keyword) ||
            p.Color.ToLower().Contains(keyword)
        ).ToList();
    }

    public List<Product> FilterByCategory(string category)
    {
        if (category == "Tất cả" || string.IsNullOrEmpty(category))
            return products;

        return products.Where(p => p.Category == category).ToList();
    }

    public Product GetProductById(int id)
    {
        return products.FirstOrDefault(p => p.Id == id);
    }
}
