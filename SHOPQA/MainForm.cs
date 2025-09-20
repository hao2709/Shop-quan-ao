using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public partial class MainForm : Form
{
    private ProductManager productManager;
    private CartManager cartManager;

    // Header Controls
    private Panel headerPanel;
    private Label titleLabel;
    private TextBox searchTextBox;
    private Button searchButton;
    private ComboBox categoryComboBox;
    private Button cartButton;
    private Label cartCountLabel;

    // Content Controls
    private FlowLayoutPanel productsPanel;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;

    public MainForm()
    {
        InitializeComponent();
        productManager = new ProductManager();
        cartManager = new CartManager();
        cartManager.CartChanged += UpdateCartDisplay;

        CreateControls();
        LoadProducts();
        LoadCategories();
    }

    private void InitializeComponent()
    {
        // Form properties
        this.Text = "SHOP QUẦN ÁO THỜI TRANG";
        this.Size = new Size(1200, 800);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 240, 240);
        this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        this.MinimumSize = new Size(1000, 600);
    }

    private void CreateControls()
    {
        CreateHeaderPanel();
        CreateProductsPanel();
        CreateStatusStrip();
    }

    private void CreateHeaderPanel()
    {
        headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 120,
            BackColor = Color.FromArgb(52, 58, 64),
            Padding = new Padding(20, 10, 20, 10)
        };

        // Title
        titleLabel = new Label
        {
            Text = "SHOP QUẦN ÁO THỜI TRANG",
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = Color.White,
            Location = new Point(20, 15),
            Size = new Size(400, 40),
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Search TextBox
        searchTextBox = new TextBox
        {
            Location = new Point(20, 70),
            Size = new Size(300, 30),
            Font = new Font("Segoe UI", 10F),
            Text = "Tìm kiếm sản phẩm...",  // ← THAY ĐỔI: Dùng Text thay vì PlaceholderText
            ForeColor = Color.Gray
        };
        searchTextBox.Enter += (s, e) =>
        {
            if (searchTextBox.Text == "Tìm kiếm sản phẩm..." && searchTextBox.ForeColor == Color.Gray)
            {
                searchTextBox.Text = "";
                searchTextBox.ForeColor = Color.Black;
            }
        };

        searchTextBox.Leave += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Text = "Tìm kiếm sản phẩm...";
                searchTextBox.ForeColor = Color.Gray;
            }
        };
        searchTextBox.KeyPress += SearchTextBox_KeyPress;

        // Search Button
        searchButton = new Button
        {
            Text = "Tìm kiếm",
            Location = new Point(330, 70),
            Size = new Size(80, 30),
            BackColor = Color.FromArgb(0, 123, 255),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F)
        };
        searchButton.FlatAppearance.BorderSize = 0;
        searchButton.Click += SearchButton_Click;

        // Category ComboBox
        categoryComboBox = new ComboBox
        {
            Location = new Point(430, 70),
            Size = new Size(150, 30),
            Font = new Font("Segoe UI", 10F),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        categoryComboBox.SelectedIndexChanged += CategoryComboBox_SelectedIndexChanged;

        // Cart Button
        cartButton = new Button
        {
            Text = "🛒 Giỏ hàng",
            Location = new Point(headerPanel.Width - 150, 70),
            Size = new Size(120, 30),
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        cartButton.FlatAppearance.BorderSize = 0;
        cartButton.Click += CartButton_Click;

        // Cart Count Label
        cartCountLabel = new Label
        {
            Text = "0",
            Location = new Point(headerPanel.Width - 25, 65),
            Size = new Size(20, 20),
            BackColor = Color.Red,
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 8F, FontStyle.Bold),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Visible = false
        };

        headerPanel.Controls.AddRange(new Control[] {
            titleLabel, searchTextBox, searchButton, categoryComboBox, cartButton, cartCountLabel
        });

        this.Controls.Add(headerPanel);
    }

    private void CreateProductsPanel()
    {
        productsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(20),
            BackColor = Color.FromArgb(248, 249, 250)
        };

        this.Controls.Add(productsPanel);
    }

    private void CreateStatusStrip()
    {
        statusStrip = new StatusStrip
        {
            BackColor = Color.FromArgb(52, 58, 64)
        };

        statusLabel = new ToolStripStatusLabel
        {
            Text = "Sẵn sàng",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9F)
        };

        statusStrip.Items.Add(statusLabel);
        this.Controls.Add(statusStrip);
    }

    private void LoadCategories()
    {
        categoryComboBox.Items.Clear();
        categoryComboBox.Items.AddRange(new[] { "Tất cả", "Áo", "Quần", "Váy" });
        categoryComboBox.SelectedIndex = 0;
    }

    private void LoadProducts(List<Product> products = null)
    {
        productsPanel.Controls.Clear();
        var productsToShow = products ?? productManager.GetAllProducts();

        foreach (var product in productsToShow)
        {
            var productCard = CreateProductCard(product);
            productsPanel.Controls.Add(productCard);
        }

        statusLabel.Text = $"Hiển thị {productsToShow.Count} sản phẩm";
    }

    private Panel CreateProductCard(Product product)
    {
        var cardPanel = new Panel
        {
            Size = new Size(250, 350),
            Margin = new Padding(10),
            BackColor = Color.White,
            BorderStyle = BorderStyle.None
        };

        // Add shadow effect
        cardPanel.Paint += (s, e) =>
        {
            var rect = cardPanel.ClientRectangle;
            using (var brush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(brush, rect.X + 2, rect.Y + 2, rect.Width, rect.Height);
            }
            e.Graphics.FillRectangle(Brushes.White, rect);
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(230, 230, 230)), rect);
        };

        // Product Image Placeholder
        var imagePanel = new Panel
        {
            Location = new Point(10, 10),
            Size = new Size(230, 150),
            BackColor = Color.FromArgb(240, 240, 240),
            BorderStyle = BorderStyle.FixedSingle
        };

        var imageLabel = new Label
        {
            Text = "📷\n" + product.Category,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 12F),
            ForeColor = Color.Gray
        };
        imagePanel.Controls.Add(imageLabel);

        // Product Name
        var nameLabel = new Label
        {
            Text = product.Name,
            Location = new Point(10, 170),
            Size = new Size(230, 30),
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(52, 58, 64)
        };

        // Product Description
        var descLabel = new Label
        {
            Text = product.Description,
            Location = new Point(10, 200),
            Size = new Size(230, 20),
            Font = new Font("Segoe UI", 8F),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.Gray
        };

        // Product Info
        var infoLabel = new Label
        {
            Text = $"Size: {product.Size} | Màu: {product.Color}",
            Location = new Point(10, 225),
            Size = new Size(230, 20),
            Font = new Font("Segoe UI", 9F),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(108, 117, 125)
        };

        // Price Label
        var priceLabel = new Label
        {
            Text = product.Price.ToString("N0") + " VNĐ",
            Location = new Point(10, 250),
            Size = new Size(230, 25),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(220, 53, 69)
        };

        // Stock Label
        var stockLabel = new Label
        {
            Text = $"Còn lại: {product.Stock}",
            Location = new Point(10, 275),
            Size = new Size(230, 15),
            Font = new Font("Segoe UI", 8F),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = product.Stock > 5 ? Color.Green : Color.Orange
        };

        // Add to Cart Button
        var addButton = new Button
        {
            Text = "🛒 Thêm vào giỏ",
            Location = new Point(50, 300),
            Size = new Size(150, 35),
            BackColor = Color.FromArgb(0, 123, 255),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Enabled = product.Stock > 0
        };
        addButton.FlatAppearance.BorderSize = 0;
        addButton.Click += (s, e) => AddToCart(product);

        if (product.Stock == 0)
        {
            addButton.Text = "Hết hàng";
            addButton.BackColor = Color.Gray;
        }

        cardPanel.Controls.AddRange(new Control[] {
            imagePanel, nameLabel, descLabel, infoLabel, priceLabel, stockLabel, addButton
        });

        return cardPanel;
    }

    private void AddToCart(Product product)
    {
        cartManager.AddToCart(product);

        // Show notification
        var notification = new ToolTip();
        notification.Show($"Đã thêm '{product.Name}' vào giỏ hàng!", this, 100, 100, 2000);
    }

    private void UpdateCartDisplay()
    {
        var quantity = cartManager.GetTotalQuantity();
        cartCountLabel.Text = quantity.ToString();
        cartCountLabel.Visible = quantity > 0;

        var total = cartManager.GetTotalAmount();
        statusLabel.Text = $"Giỏ hàng: {quantity} sản phẩm - Tổng: {total:N0} VNĐ";
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        PerformSearch();
    }

    private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            PerformSearch();
        }
    }


    private void PerformSearch()
    {
        var keyword = searchTextBox.Text.Trim();

        // Bỏ qua nếu đang hiển thị placeholder
        if (keyword == "Tìm kiếm sản phẩm..." || searchTextBox.ForeColor == Color.Gray)
        {
            keyword = "";
        }

        var results = productManager.SearchProducts(keyword);
        LoadProducts(results);
    }

    private void CategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedCategory = categoryComboBox.SelectedItem?.ToString();
        var filteredProducts = productManager.FilterByCategory(selectedCategory);
        LoadProducts(filteredProducts);
    }

    private void CartButton_Click(object sender, EventArgs e)
    {
        var cartForm = new CartForm(cartManager);
        cartForm.ShowDialog();
    }
}