using System.Drawing;
using System.Windows.Forms;

namespace BeerLab
{
    internal static class Theme
    {
        public static readonly Color Bg = Color.FromArgb(12, 11, 10);
        public static readonly Color Surface = Color.FromArgb(20, 19, 17);
        public static readonly Color Elevated = Color.FromArgb(28, 26, 23);
        public static readonly Color Fg = Color.FromArgb(242, 238, 230);
        public static readonly Color Muted = Color.FromArgb(154, 147, 136);
        public static readonly Color Subtle = Color.FromArgb(110, 105, 96);
        public static readonly Color Primary = Color.FromArgb(232, 224, 212);
        public static readonly Color PrimaryFg = Color.FromArgb(12, 11, 10);
        public static readonly Color Danger = Color.FromArgb(196, 92, 74);
        public static readonly Color Warn = Color.FromArgb(196, 160, 106);
        public static readonly Color Ok = Color.FromArgb(125, 154, 126);
        public static readonly Color Border = Color.FromArgb(42, 39, 34);

        public static readonly Font Ui = new Font("Segoe UI", 10f);
        public static readonly Font UiBold = new Font("Segoe UI Semibold", 10f);
        public static readonly Font Small = new Font("Segoe UI", 8.5f);
        public static readonly Font Display = new Font("Georgia", 22f, FontStyle.Italic);
        public static readonly Font DisplaySm = new Font("Georgia", 16f, FontStyle.Italic);
        public static readonly Font Kpi = new Font("Georgia", 26f, FontStyle.Regular);
        public static readonly Font Mono = new Font("Consolas", 10f);

        public static void StyleGrid(DataGridView g)
        {
            g.BackgroundColor = Surface;
            g.BorderStyle = BorderStyle.None;
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.GridColor = Border;
            g.RowHeadersVisible = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.ReadOnly = true;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowTemplate.Height = 36;
            g.ColumnHeadersHeight = 38;
            g.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Elevated,
                ForeColor = Muted,
                Font = Small,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 8, 0)
            };
            g.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Surface,
                ForeColor = Fg,
                Font = Ui,
                SelectionBackColor = Elevated,
                SelectionForeColor = Fg,
                Padding = new Padding(8, 0, 8, 0)
            };
            g.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(24, 22, 20),
                ForeColor = Fg,
                Font = Ui,
                SelectionBackColor = Elevated,
                SelectionForeColor = Fg,
                Padding = new Padding(8, 0, 8, 0)
            };
        }

        public static Button NavButton(string text, bool active)
        {
            var b = new Button
            {
                Text = "   " + text,
                Dock = DockStyle.Top,
                Height = 44,
                FlatStyle = FlatStyle.Flat,
                Font = Ui,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                ForeColor = active ? Fg : Muted,
                BackColor = active ? Elevated : Bg
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Elevated;
            return b;
        }

        public static Button PrimaryButton(string text)
        {
            var b = new Button
            {
                Text = text,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Font = UiBold,
                ForeColor = PrimaryFg,
                BackColor = Primary,
                Cursor = Cursors.Hand,
                Padding = new Padding(12, 0, 12, 0)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        public static Button GhostButton(string text)
        {
            var b = new Button
            {
                Text = text,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Font = Ui,
                ForeColor = Fg,
                BackColor = Elevated,
                Cursor = Cursors.Hand,
                Padding = new Padding(12, 0, 12, 0)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        public static TextBox Input()
        {
            return new TextBox
            {
                Font = Ui,
                BackColor = Elevated,
                ForeColor = Fg,
                BorderStyle = BorderStyle.FixedSingle,
                Height = 32
            };
        }

        public static ComboBox Combo()
        {
            return new ComboBox
            {
                Font = Ui,
                BackColor = Elevated,
                ForeColor = Fg,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
        }

        public static Label MutedLabel(string text)
        {
            return new Label
            {
                Text = text.ToUpperInvariant(),
                ForeColor = Muted,
                Font = Small,
                AutoSize = true
            };
        }
    }
}
