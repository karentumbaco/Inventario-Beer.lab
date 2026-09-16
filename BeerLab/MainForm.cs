using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BeerLab
{
    public sealed class MainForm : Form
    {
        private readonly Panel _content;
        private readonly Button[] _nav;
        private readonly string[] _pages = { "Panel", "Inventario", "Movimientos", "Alertas" };
        private string _page = "Panel";

        public MainForm()
        {
            Text = "beer.lab  ·  Inventario";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1100, 700);
            Size = new Size(1280, 800);
            BackColor = Theme.Bg;
            ForeColor = Theme.Fg;
            Font = Theme.Ui;

            var side = new Panel { Dock = DockStyle.Left, Width = 200, BackColor = Theme.Bg };
            var brand = new Label
            {
                Text = "beer.lab",
                Font = Theme.Display,
                ForeColor = Theme.Fg,
                Dock = DockStyle.Top,
                Height = 70,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(16, 0, 0, 8)
            };
            var sub = new Label
            {
                Text = "INVENTARIO",
                Font = Theme.Small,
                ForeColor = Theme.Muted,
                Dock = DockStyle.Top,
                Height = 24,
                Padding = new Padding(18, 0, 0, 0)
            };

            var navHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8, 16, 8, 8) };
            _nav = new Button[_pages.Length];
            for (var i = _pages.Length - 1; i >= 0; i--)
            {
                var name = _pages[i];
                var b = Theme.NavButton(name, name == _page);
                b.Click += (_, __) => ShowPage(name);
                navHost.Controls.Add(b);
                _nav[i] = b;
            }

            var reset = new LinkLabel
            {
                Text = "Restaurar demo",
                LinkColor = Theme.Subtle,
                ActiveLinkColor = Theme.Fg,
                Dock = DockStyle.Bottom,
                Height = 36,
                TextAlign = ContentAlignment.MiddleCenter
            };
            reset.LinkClicked += (_, __) =>
            {
                if (MessageBox.Show(this, "¿Volver al catálogo de ejemplo? Se pierde lo que hayas cargado.",
                        "beer.lab", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Store.Current.ResetDemo();
                    ShowPage(_page);
                }
            };

            side.Controls.Add(navHost);
            side.Controls.Add(sub);
            side.Controls.Add(brand);
            side.Controls.Add(reset);

            _content = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Bg, Padding = new Padding(28, 20, 28, 20) };
            Controls.Add(_content);
            Controls.Add(side);

            Store.Current.Changed += () =>
            {
                if (IsHandleCreated) BeginInvoke((Action)(() => ShowPage(_page)));
            };

            ShowPage("Panel");
        }

        private void ShowPage(string page)
        {
            _page = page;
            for (var i = 0; i < _pages.Length; i++)
            {
                _nav[i].BackColor = _pages[i] == page ? Theme.Elevated : Theme.Bg;
                _nav[i].ForeColor = _pages[i] == page ? Theme.Fg : Theme.Muted;
            }
            _content.Controls.Clear();
            switch (page)
            {
                case "Inventario": RenderInventario(); break;
                case "Movimientos": RenderMovimientos(); break;
                case "Alertas": RenderAlertas(); break;
                default: RenderPanel(); break;
            }
        }

        private void RenderPanel()
        {
            var products = Store.Current.ActiveProducts().ToList();
            var moves = Store.Current.Data.Movements.Take(10).ToList();
            var units = products.Sum(p => p.Stock);
            var cost = products.Sum(p => p.ValorCosto);
            var low = products.Count(p => p.Estado == "Bajo mínimo");
            var agot = products.Count(p => p.Estado == "Agotado");

            var header = Header("Bodega", DateTime.Now.ToString("dddd d 'de' MMMM", new System.Globalization.CultureInfo("es-EC")));
            var actions = new FlowLayoutPanel { AutoSize = true, WrapContents = false, BackColor = Theme.Bg, Dock = DockStyle.Right };
            actions.Controls.Add(MakeBtn("Salida", false, () => OpenMove(null, "Salida")));
            actions.Controls.Add(MakeBtn("Entrada", false, () => OpenMove(null, "Entrada")));
            actions.Controls.Add(MakeBtn("Producto", true, () => OpenProduct(null)));
            header.Controls.Add(actions);

            var kpis = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 110,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Theme.Bg,
                Padding = new Padding(0, 8, 0, 8)
            };
            for (var i = 0; i < 4; i++) kpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            kpis.Controls.Add(Kpi("SKUs ACTIVOS", products.Count.ToString("N0")), 0, 0);
            kpis.Controls.Add(Kpi("UNIDADES", units.ToString("N0")), 1, 0);
            kpis.Controls.Add(Kpi("COSTO EN BODEGA", cost.ToString("C2")), 2, 0);
            kpis.Controls.Add(Kpi("ALERTAS", (low + agot).ToString("N0"), agot + " agotados · " + low + " bajo mínimo"), 3, 0);

            var split = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Theme.Bg };
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            split.Controls.Add(ListCard("Reponer", products.Where(p => p.Stock <= p.MinStock).OrderBy(p => p.Stock).Take(12).ToList(), true), 0, 0);
            split.Controls.Add(MoveCard("Kardex reciente", moves), 1, 0);

            _content.Controls.Add(split);
            _content.Controls.Add(kpis);
            _content.Controls.Add(header);
        }

        private void RenderInventario()
        {
            var header = Header("Inventario", "Catálogo de bodega");
            var nuevo = MakeBtn("Nuevo producto", true, () => OpenProduct(null));
            nuevo.Dock = DockStyle.Right;
            header.Controls.Add(nuevo);

            var search = Theme.Input();
            search.Width = 280;
            search.Text = "Buscar nombre, marca o SKU";
            search.ForeColor = Theme.Subtle;
            search.GotFocus += (_, __) =>
            {
                if (search.Text == "Buscar nombre, marca o SKU")
                {
                    search.Text = "";
                    search.ForeColor = Theme.Fg;
                }
            };

            var cat = Theme.Combo();
            cat.Width = 180;
            cat.Items.Add("Todas las categorías");
            cat.Items.AddRange(Catalog.Categories);
            cat.SelectedIndex = 0;

            var stock = Theme.Combo();
            stock.Width = 150;
            stock.Items.AddRange(new object[] { "Todo el stock", "En nivel", "Bajo mínimo", "Agotado" });
            stock.SelectedIndex = 0;

            var filters = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = Theme.Bg,
                WrapContents = false
            };
            filters.Controls.Add(search);
            filters.Controls.Add(cat);
            filters.Controls.Add(stock);

            var grid = MakeGrid();
            grid.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0 || !grid.Columns.Contains("Id")) return;
                OpenProduct((int)grid.Rows[e.RowIndex].Cells["Id"].Value);
            };

            Action bind = () =>
            {
                var q = (search.Text ?? "").Trim();
                if (q == "Buscar nombre, marca o SKU") q = "";
                q = q.ToLowerInvariant();
                var catv = cat.SelectedItem as string;
                var st = stock.SelectedItem as string;
                var rows = Store.Current.ActiveProducts().Where(p =>
                {
                    if (q.Length > 0 &&
                        p.Name.ToLowerInvariant().IndexOf(q) < 0 &&
                        p.Sku.ToLowerInvariant().IndexOf(q) < 0 &&
                        p.Brand.ToLowerInvariant().IndexOf(q) < 0)
                        return false;
                    if (!string.IsNullOrEmpty(catv) && catv != "Todas las categorías" && p.Category != catv)
                        return false;
                    if (st == "En nivel" && p.Estado != "En nivel") return false;
                    if (st == "Bajo mínimo" && p.Estado != "Bajo mínimo") return false;
                    if (st == "Agotado" && p.Estado != "Agotado") return false;
                    return true;
                }).Select(p => new
                {
                    p.Id,
                    p.Sku,
                    Producto = p.Name,
                    Categoría = p.Category,
                    p.Stock,
                    Mín = p.MinStock,
                    Costo = p.Cost,
                    Precio = p.Price,
                    p.Estado
                }).ToList();
                grid.DataSource = rows;
                if (grid.Columns.Contains("Id")) grid.Columns["Id"].Visible = false;
                PaintEstado(grid, "Estado");
            };

            search.TextChanged += (_, __) => bind();
            cat.SelectedIndexChanged += (_, __) => bind();
            stock.SelectedIndexChanged += (_, __) => bind();

            _content.Controls.Add(grid);
            _content.Controls.Add(filters);
            _content.Controls.Add(header);
            bind();
        }

        private void RenderMovimientos()
        {
            var header = Header("Movimientos", "Kardex de bodega");
            var btn = MakeBtn("Registrar", true, () => OpenMove(null, "Salida"));
            btn.Dock = DockStyle.Right;
            header.Controls.Add(btn);
            var grid = MakeGrid();
            grid.DataSource = Store.Current.Data.Movements.Take(200).Select(m => new
            {
                Fecha = m.CreatedAt,
                SKU = m.ProductSku,
                Producto = m.ProductName,
                Tipo = m.Type,
                Motivo = m.Reason,
                Cantidad = m.Quantity,
                Stock = m.StockAfter,
                Nota = m.Note
            }).ToList();
            grid.CellFormatting += (s, e) =>
            {
                if (grid.Columns[e.ColumnIndex].Name == "Cantidad" && e.Value is int n)
                    e.CellStyle.ForeColor = n < 0 ? Theme.Danger : Theme.Ok;
            };
            _content.Controls.Add(grid);
            _content.Controls.Add(header);
        }

        private void RenderAlertas()
        {
            var products = Store.Current.ActiveProducts().ToList();
            var agot = products.Where(p => p.Estado == "Agotado").ToList();
            var low = products.Where(p => p.Estado == "Bajo mínimo").ToList();
            var header = Header("Alertas", agot.Count + " agotados · " + low.Count + " bajo mínimo");
            var split = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1, BackColor = Theme.Bg };
            split.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
            split.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
            split.Controls.Add(ListCard("Agotados", agot, true), 0, 0);
            split.Controls.Add(ListCard("Bajo mínimo", low, true), 0, 1);
            _content.Controls.Add(split);
            _content.Controls.Add(header);
        }

        private Panel Header(string title, string subtitle)
        {
            var p = new Panel { Dock = DockStyle.Top, Height = 88, BackColor = Theme.Bg };
            p.Controls.Add(new Label
            {
                Text = subtitle,
                Font = Theme.Small,
                ForeColor = Theme.Muted,
                Location = new Point(0, 8),
                AutoSize = true
            });
            p.Controls.Add(new Label
            {
                Text = title,
                Font = Theme.Display,
                ForeColor = Theme.Fg,
                Location = new Point(0, 28),
                AutoSize = true
            });
            return p;
        }

        private static Panel Kpi(string label, string value, string hint = null)
        {
            var p = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface, Margin = new Padding(0, 0, 10, 0), Padding = new Padding(16, 12, 16, 12) };
            p.Controls.Add(new Label { Text = label, Font = Theme.Small, ForeColor = Theme.Muted, Dock = DockStyle.Top, Height = 18 });
            p.Controls.Add(new Label { Text = value, Font = Theme.Kpi, ForeColor = Theme.Fg, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft });
            if (hint != null)
                p.Controls.Add(new Label { Text = hint, Font = Theme.Small, ForeColor = Theme.Subtle, Dock = DockStyle.Bottom, Height = 18 });
            return p;
        }

        private Control ListCard(string title, System.Collections.Generic.List<Product> items, bool openOnClick)
        {
            var p = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface, Margin = new Padding(0, 8, 10, 0) };
            p.Controls.Add(new Label
            {
                Text = title,
                Font = Theme.DisplaySm,
                ForeColor = Theme.Fg,
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(16, 10, 16, 0)
            });
            var grid = MakeGrid();
            grid.DataSource = items.Select(x => new
            {
                x.Id,
                x.Sku,
                Producto = x.Name,
                x.Stock,
                Mín = x.MinStock,
                x.Estado
            }).ToList();
            if (grid.Columns.Contains("Id")) grid.Columns["Id"].Visible = false;
            PaintEstado(grid, "Estado");
            if (openOnClick)
            {
                grid.CellDoubleClick += (s, e) =>
                {
                    if (e.RowIndex < 0) return;
                    OpenProduct((int)grid.Rows[e.RowIndex].Cells["Id"].Value);
                };
            }
            p.Controls.Add(grid);
            grid.BringToFront();
            return p;
        }

        private Control MoveCard(string title, System.Collections.Generic.List<Movement> items)
        {
            var p = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface, Margin = new Padding(0, 8, 0, 0) };
            p.Controls.Add(new Label
            {
                Text = title,
                Font = Theme.DisplaySm,
                ForeColor = Theme.Fg,
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(16, 10, 16, 0)
            });
            var grid = MakeGrid();
            grid.DataSource = items.Select(m => new
            {
                Producto = m.ProductName,
                Tipo = m.Type,
                Cantidad = m.Quantity,
                Fecha = m.CreatedAt.ToString("dd MMM HH:mm")
            }).ToList();
            p.Controls.Add(grid);
            grid.BringToFront();
            return p;
        }

        private static DataGridView MakeGrid()
        {
            var g = new DataGridView { Dock = DockStyle.Fill };
            Theme.StyleGrid(g);
            return g;
        }

        private static void PaintEstado(DataGridView grid, string col)
        {
            if (!grid.Columns.Contains(col)) return;
            grid.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || grid.Columns[e.ColumnIndex].Name != col) return;
                var v = Convert.ToString(e.Value);
                if (v == "Agotado") e.CellStyle.ForeColor = Theme.Danger;
                else if (v == "Bajo mínimo") e.CellStyle.ForeColor = Theme.Warn;
                else if (v == "En nivel") e.CellStyle.ForeColor = Theme.Ok;
            };
        }

        private Button MakeBtn(string text, bool primary, Action click)
        {
            var b = primary ? Theme.PrimaryButton(text) : Theme.GhostButton(text);
            b.AutoSize = true;
            b.Margin = new Padding(8, 8, 0, 0);
            b.Click += (_, __) => click();
            return b;
        }

        private void OpenProduct(int? id)
        {
            Product p = null;
            if (id.HasValue) p = Store.Current.GetProduct(id.Value);
            using (var d = new ProductDialog(p))
                d.ShowDialog(this);
        }

        private void OpenMove(int? productId, string type)
        {
            using (var d = new MovementDialog(productId, type))
                d.ShowDialog(this);
        }
    }
}
