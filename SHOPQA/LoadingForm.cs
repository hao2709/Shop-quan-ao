using System;
using System.Drawing;
using System.Windows.Forms;

public partial class LoadingForm : Form
{
    private Label loadingLabel;
    private ProgressBar progressBar;
    private Timer timer;
    private int progress = 0;

    public LoadingForm()
    {
        InitializeComponent();
        CreateControls();
        StartProgress();
    }

    private void InitializeComponent()
    {
        this.Text = "Đang xử lý...";
        this.Size = new Size(350, 150);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = Color.White;
        this.ShowInTaskbar = false;
        this.TopMost = true;
    }

    private void CreateControls()
    {
        // Border
        this.Paint += (s, e) =>
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(52, 58, 64), 2),
                new Rectangle(0, 0, this.Width - 1, this.Height - 1));
        };

        loadingLabel = new Label
        {
            Text = "Đang xử lý đơn hàng...",
            Location = new Point(20, 30),
            Size = new Size(300, 30),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(52, 58, 64)
        };

        progressBar = new ProgressBar
        {
            Location = new Point(30, 70),
            Size = new Size(280, 25),
            Style = ProgressBarStyle.Continuous
        };

        this.Controls.AddRange(new Control[] { loadingLabel, progressBar });
    }

    private void StartProgress()
    {
        timer = new Timer();
        timer.Interval = 50;
        timer.Tick += (s, e) =>
        {
            progress += 2;
            progressBar.Value = Math.Min(progress, 100);

            if (progress >= 100)
            {
                timer.Stop();
            }
        };
        timer.Start();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        timer?.Stop();
        timer?.Dispose();
        base.OnFormClosing(e);
    }
}
