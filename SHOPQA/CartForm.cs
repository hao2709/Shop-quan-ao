using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public partial class CartForm : Form
{
    private CartManager cartManager;
    private ListView cartListView;
    private Label totalLabel;
    private Button checkoutButton;
    private Button clearButton;
    private Button closeButton;

    public CartForm(CartManager cartManager)
    {
        this.cartManager = cartManager;
        InitializeComponent();
        CreateControls();
        LoadCartItems();
    }

    private void InitializeComponent()
    {
        this.Text = "Giỏ hàng";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.White;
        this.Font = new Font("Segoe UI", 9F);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
    }

    private void CreateControls()
    {
        // Header
        var headerLabel = new Label
        {
            Text = "🛒 GIỎ HÀNG CỦA BẠN",
            Location = new Point(20, 20),
            Size = new Size(300, 30),
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 58, 64)
        };

        // Cart ListView
        cartListView = new ListView
        {
            Location = new Point(20, 60),
            Size = new Size(750, 400),
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            Font = new Font("Segoe UI", 10F)
        };

        cartListView.Columns.Add("Sản phẩm", 200);
        cartListView.Columns.Add("Giá", 100);
        cartListView.Columns.Add("Số lượng", 80);
        cartListView.Columns.Add("Thành tiền", 120);
        cartListView.Columns.Add("Size", 60);
        cartListView.Columns.Add("Màu", 80);
        cartListView.Columns.Add("Tồn kho", 80);

        // Total Label
        totalLabel = new Label
        {
            Location = new Point(20, 480),
            Size = new Size(400, 40),
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.FromArgb(220, 53, 69),
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Buttons Panel
        var buttonPanel = new Panel
        {
            Location = new Point(20, 520),
            Size = new Size(750, 50),
            BackColor = Color.Transparent
        };

        clearButton = new Button
        {
            Text = "🗑️ Xóa tất cả",
            Location = new Point(0, 10),
            Size = new Size(120, 35),
            BackColor = Color.FromArgb(220, 53, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F)
        };
        clearButton.FlatAppearance.BorderSize = 0;
        clearButton.Click += ClearButton_Click;

        checkoutButton = new Button
        {
            Text = "💳 Thanh toán",
            Location = new Point(500, 10),
            Size = new Size(120, 35),
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold)
        };
        checkoutButton.FlatAppearance.BorderSize = 0;
        checkoutButton.Click += CheckoutButton_Click;

        closeButton = new Button
        {
            Text = "❌ Đóng",
            Location = new Point(630, 10),
            Size = new Size(120, 35),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F)
        };
        closeButton.FlatAppearance.BorderSize = 0;
        closeButton.Click += (s, e) => this.Close();

        buttonPanel.Controls.AddRange(new Control[] { clearButton, checkoutButton, closeButton });

        this.Controls.AddRange(new Control[] { headerLabel, cartListView, totalLabel, buttonPanel });

        // Context menu for cart items
        var contextMenu = new ContextMenuStrip();
        var removeItem = new ToolStripMenuItem("Xóa sản phẩm");
        removeItem.Click += RemoveItem_Click;
        contextMenu.Items.Add(removeItem);
        cartListView.ContextMenuStrip = contextMenu;
    }

    private void LoadCartItems()
    {
        cartListView.Items.Clear();
        var cartItems = cartManager.GetCartItems();

        foreach (var item in cartItems)
        {
            var listItem = new ListViewItem(item.Product.Name);
            listItem.SubItems.Add(item.Product.Price.ToString("N0") + " VNĐ");
            listItem.SubItems.Add(item.Quantity.ToString());
            listItem.SubItems.Add(item.TotalPrice.ToString("N0") + " VNĐ");
            listItem.SubItems.Add(item.Product.Size);
            listItem.SubItems.Add(item.Product.Color);
            listItem.SubItems.Add(item.Product.Stock.ToString());
            listItem.Tag = item.Product.Id;

            cartListView.Items.Add(listItem);
        }

        var total = cartManager.GetTotalAmount();
        var quantity = cartManager.GetTotalQuantity();
        totalLabel.Text = $"TỔNG CỘNG: {total:N0} VNĐ ({quantity} sản phẩm)";

        checkoutButton.Enabled = cartItems.Count > 0;
        clearButton.Enabled = cartItems.Count > 0;
    }

    private void RemoveItem_Click(object sender, EventArgs e)
    {
        if (cartListView.SelectedItems.Count > 0)
        {
            var selectedItem = cartListView.SelectedItems[0];
            var productId = (int)selectedItem.Tag;

            var result = MessageBox.Show(
                $"Bạn có muốn xóa '{selectedItem.Text}' khỏi giỏ hàng?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                cartManager.RemoveFromCart(productId);
                LoadCartItems();
            }
        }
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Bạn có chắc chắn muốn xóa tất cả sản phẩm khỏi giỏ hàng?",
            "Xác nhận xóa tất cả",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );

        if (result == DialogResult.Yes)
        {
            cartManager.ClearCart();
            LoadCartItems();
        }
    }

    private void CheckoutButton_Click(object sender, EventArgs e)
    {
        var checkoutForm = new CheckoutForm(cartManager);
        if (checkoutForm.ShowDialog() == DialogResult.OK)
        {
            this.Close();
        }
    }
}
