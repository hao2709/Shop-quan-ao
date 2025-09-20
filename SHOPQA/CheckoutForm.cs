using System;
using System.Drawing;
using System.Windows.Forms;

public partial class CheckoutForm : Form
{
    private CartManager cartManager;
    private TextBox customerNameTextBox;
    private TextBox phoneTextBox;
    private TextBox addressTextBox;
    private ComboBox paymentMethodComboBox;
    private Label totalLabel;

    public CheckoutForm(CartManager cartManager)
    {
        this.cartManager = cartManager;
        InitializeComponent();
        CreateControls();
        UpdateTotal();
    }

    private void InitializeComponent()
    {
        this.Text = "Thanh toán";
        this.Size = new Size(500, 400);
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.White;
        this.Font = new Font("Segoe UI", 10F);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
    }

    private void CreateControls()
    {
        // Header
        var headerLabel = new Label
        {
            Text = "💳 THÔNG TIN THANH TOÁN",
            Location = new Point(20, 20),
            Size = new Size(300, 30),
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 58, 64)
        };

        // Customer Name
        var nameLabel = new Label
        {
            Text = "Họ tên khách hàng:",
            Location = new Point(20, 70),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 10F)
        };

        customerNameTextBox = new TextBox
        {
            Location = new Point(180, 70),
            Size = new Size(250, 25),
            Font = new Font("Segoe UI", 10F)
        };

        // Phone
        var phoneLabel = new Label
        {
            Text = "Số điện thoại:",
            Location = new Point(20, 110),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 10F)
        };

        phoneTextBox = new TextBox
        {
            Location = new Point(180, 110),
            Size = new Size(250, 25),
            Font = new Font("Segoe UI", 10F)
        };

        // Address
        var addressLabel = new Label
        {
            Text = "Địa chỉ giao hàng:",
            Location = new Point(20, 150),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 10F)
        };

        addressTextBox = new TextBox
        {
            Location = new Point(180, 150),
            Size = new Size(250, 60),
            Font = new Font("Segoe UI", 10F),
            Multiline = true
        };

        // Payment Method
        var paymentLabel = new Label
        {
            Text = "Phương thức thanh toán:",
            Location = new Point(20, 230),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 10F)
        };

        paymentMethodComboBox = new ComboBox
        {
            Location = new Point(180, 230),
            Size = new Size(250, 25),
            Font = new Font("Segoe UI", 10F),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        paymentMethodComboBox.Items.AddRange(new[] {
            "Thanh toán khi nhận hàng (COD)",
            "Chuyển khoản ngân hàng",
            "Ví điện tử (MoMo, ZaloPay)",
            "Thẻ tín dụng/ghi nợ"
        });
        paymentMethodComboBox.SelectedIndex = 0;

        // Total
        totalLabel = new Label
        {
            Location = new Point(20, 280),
            Size = new Size(400, 30),
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(220, 53, 69)
        };

        // Buttons
        var confirmButton = new Button
        {
            Text = "✅ Xác nhận đặt hàng",
            Location = new Point(180, 320),
            Size = new Size(150, 35),
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold)
        };
        confirmButton.FlatAppearance.BorderSize = 0;
        confirmButton.Click += ConfirmButton_Click;

        var cancelButton = new Button
        {
            Text = "❌ Hủy",
            Location = new Point(340, 320),
            Size = new Size(90, 35),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F)
        };
        cancelButton.FlatAppearance.BorderSize = 0;
        cancelButton.Click += (s, e) => this.Close();

        this.Controls.AddRange(new Control[] {
            headerLabel, nameLabel, customerNameTextBox, phoneLabel, phoneTextBox,
            addressLabel, addressTextBox, paymentLabel, paymentMethodComboBox,
            totalLabel, confirmButton, cancelButton
        });
    }

    private void UpdateTotal()
    {
        var total = cartManager.GetTotalAmount();
        var quantity = cartManager.GetTotalQuantity();
        totalLabel.Text = $"Tổng thanh toán: {total:N0} VNĐ ({quantity} sản phẩm)";
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(customerNameTextBox.Text))
        {
            MessageBox.Show("Vui lòng nhập họ tên khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            customerNameTextBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(phoneTextBox.Text))
        {
            MessageBox.Show("Vui lòng nhập số điện thoại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            phoneTextBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(addressTextBox.Text))
        {
            MessageBox.Show("Vui lòng nhập địa chỉ giao hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            addressTextBox.Focus();
            return;
        }

        // Create order summary
        var orderSummary = CreateOrderSummary();

        var result = MessageBox.Show(orderSummary, "Xác nhận đơn hàng",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            // Process order
            ProcessOrder();
        }
    }

    private string CreateOrderSummary()
    {
        var summary = "THÔNG TIN ĐỌN HÀNG:\n\n";
        summary += $"Khách hàng: {customerNameTextBox.Text}\n";
        summary += $"Số điện thoại: {phoneTextBox.Text}\n";
        summary += $"Địa chỉ: {addressTextBox.Text}\n";
        summary += $"Thanh toán: {paymentMethodComboBox.SelectedItem}\n\n";

        summary += "CHI TIẾT SẢN PHẨM:\n";
        foreach (var item in cartManager.GetCartItems())
        {
            summary += $"- {item.Product.Name} x{item.Quantity} = {item.TotalPrice:N0} VNĐ\n";
        }

        summary += $"\nTONG CỘNG: {cartManager.GetTotalAmount():N0} VNĐ";
        summary += "\n\nBạn có xác nhận đặt hàng không?";

        return summary;
    }

    private void ProcessOrder()
    {
        try
        {
            // Simulate order processing
            var loadingForm = new LoadingForm();
            loadingForm.Show();

            System.Threading.Thread.Sleep(2000); // Simulate processing time

            loadingForm.Close();

            // Clear cart after successful order
            cartManager.ClearCart();

            MessageBox.Show(
                "Đặt hàng thành công!\n\nMã đơn hàng: DH" + DateTime.Now.ToString("yyyyMMddHHmmss") +
                "\n\nCảm ơn bạn đã mua hàng tại cửa hàng!\nChúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.",
                "Đặt hàng thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Có lỗi xảy ra khi xử lý đơn hàng: " + ex.Message,
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}