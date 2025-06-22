using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedLabel : Label
{
    public int CornerRadius { get; set; } = 10;

    public RoundedLabel()
    {
        this.BackColor = Color.White;
        this.ForeColor = Color.Black;
        this.AutoSize = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        using (GraphicsPath path = new GraphicsPath())
        {
            Rectangle rect = this.ClientRectangle;
            int radius = CornerRadius;
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Brush b = new SolidBrush(this.BackColor))
                e.Graphics.FillPath(b, path);

            TextRenderer.DrawText(
                e.Graphics,
                this.Text,
                this.Font,
                rect,
                this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
            );
        }
    }
}