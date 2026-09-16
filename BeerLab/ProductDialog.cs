using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace BeerLab
{
    internal sealed class ProductDialog : Form
    {
        private readonly Product _existing;
        private readonly TextBox _sku, _name, _brand, _ml, _cost, _price, _priceSix, _priceCaja, _min, _stock, _loc, _notes;
        private readonly ComboBox _cat, _unit;
        private readonly Panel _beerPack;

        public ProductDialog(Product existing = null)
        {
            _existing = existing;
            Text = existing == null ? "Nuevo producto" : "Editar producto";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(580, 700);
            MinimumSize = new Size(520, 420);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = false;
            BackColor = Theme.Bg;
            ForeColor = Theme.Fg;
            Font = Theme.Ui;
            AutoScaleMode = AutoScaleMode.Dpi;

            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Theme.Bg,
                Padding = new Padding(16, 12, 16, 12)
            };

            var cancel = Theme.GhostButton("Cancelar");
            cancel.Dock = DockStyle.Right;
            cancel.Width = 110;
            cancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

            var save = Theme.PrimaryButton("Guardar");
            save.Dock = DockStyle.Right;
            save.Width = 110;
            save.Click += (_, __) => Accept();

            footer.Controls.Add(cancel);
            footer.Controls.Add(save);

            if (existing != null)
            {
                var del = Theme.GhostButton("Eliminar");
                del.ForeColor = Theme.Danger;
                del.Dock = DockStyle.Left;
                del.Width = 120;
                del.Click += (_, __) => Delete();
                footer.Controls.Add(del);
            }

            var body = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Theme.Bg,
                Padding = new Padding(8)
            };

            const int left = 20;
            const int right = 290;
            const int colW = 250;
            const int fullW = 520;
            var y = 16;

            _stock = AddField(body, "Cantidad disponible", left, y, colW);
            _min = AddField(body, "Stock mínimo", right, y, colW);
            y += 62;
            _sku = AddField(body, "SKU", left, y, colW);
            _name = AddField(body, "Nombre", right, y, colW);
            y += 62;
            _brand = AddField(body, "Marca", left, y, colW);
            _cat = AddCombo(body, "Categoría", Catalog.Categories, right, y, colW);
            y += 62;
            _ml = AddField(body, "Presentación (ml)", left, y, colW);
            _unit = AddCombo(body, "Unidad", Catalog.Units, right, y, colW);
            y += 62;
            _cost = AddField(body, "Costo USD", left, y, colW);
            _price = AddField(body, "Precio unidad", right, y, colW);
            y += 62;

            _beerPack = new Panel
            {
                Location = new Point(8, y),
                Size = new Size(560, 70),
                BackColor = Theme.Bg
            };
            _priceSix = AddField(_beerPack, "Precio six pack", 12, 0, colW);
            _priceCaja = AddField(_beerPack, "Precio caja", 282, 0, colW);
            body.Controls.Add(_beerPack);
            y += 78;

            _loc = AddField(body, "Ubicación", left, y, fullW);
            y += 62;
            _notes = AddField(body, "Notas", left, y, fullW, multiline: true);
            y += 110;
            body.AutoScrollMinSize = new Size(540, y + 20);

            Controls.Add(body);
            Controls.Add(footer);

            if (existing != null)
            {
                _stock.Text = existing.Stock.ToString();
                _min.Text = existing.MinStock.ToString();
                _sku.Text = existing.Sku;
                _name.Text = existing.Name;
                _brand.Text = existing.Brand;
                _cat.SelectedItem = existing.Category;
                _ml.Text = existing.SizeMl.ToString();
                _unit.SelectedItem = existing.Unit;
                _cost.Text = existing.Cost.ToString("0.00", CultureInfo.InvariantCulture);
                _price.Text = existing.Price.ToString("0.00", CultureInfo.InvariantCulture);
                _priceSix.Text = existing.PriceSix.ToString("0.00", CultureInfo.InvariantCulture);
                _priceCaja.Text = existing.PriceCaja.ToString("0.00", CultureInfo.InvariantCulture);
                _loc.Text = existing.Location;
                _notes.Text = existing.Notes;
            }
            else
            {
                _stock.Text = "0";
                _min.Text = "4";
                _ml.Text = "330";
                _unit.SelectedItem = "botella";
                _cat.SelectedIndex = 0;
                _priceSix.Text = "0.00";
                _priceCaja.Text = "0.00";
            }

            _cat.SelectedIndexChanged += (_, __) => ToggleBeer();
            ToggleBeer();

            AcceptButton = save;
            CancelButton = cancel;
        }

        private void ToggleBeer()
        {
            var beer = string.Equals(Convert.ToString(_cat.SelectedItem), "Cerveza", StringComparison.OrdinalIgnoreCase);
            _beerPack.Visible = beer;
        }

        private void Delete()
        {
            if (_existing == null) return;
            var ok = MessageBox.Show(
                this,
                "¿Eliminar \"" + _existing.Name + "\" del inventario?\nTambién se borran sus movimientos del kardex.",
                "Eliminar producto",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            if (ok != DialogResult.Yes) return;
            try
            {
                Store.Current.DeleteProduct(_existing.Id);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "beer.lab", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Accept()
        {
            try
            {
                var p = _existing ?? new Product();
                p.Sku = _sku.Text;
                p.Name = _name.Text.Trim();
                p.Brand = _brand.Text.Trim();
                p.Category = Convert.ToString(_cat.SelectedItem) ?? "";
                p.SizeMl = ParseInt(_ml.Text, "presentación");
                p.Unit = Convert.ToString(_unit.SelectedItem) ?? "botella";
                p.Cost = ParseMoney(_cost.Text, "costo");
                p.Price = ParseMoney(_price.Text, "precio unidad");
                p.MinStock = ParseInt(_min.Text, "stock mínimo");
                p.Location = _loc.Text.Trim();
                p.Notes = _notes.Text.Trim();
                if (p.EsCerveza)
                {
                    p.PriceSix = ParseMoney(_priceSix.Text, "precio six pack");
                    p.PriceCaja = ParseMoney(_priceCaja.Text, "precio caja");
                }
                else
                {
                    p.PriceSix = 0;
                    p.PriceCaja = 0;
                }
                if (string.IsNullOrWhiteSpace(p.Sku) || string.IsNullOrWhiteSpace(p.Name) || string.IsNullOrWhiteSpace(p.Brand))
                    throw new InvalidOperationException("SKU, nombre y marca son obligatorios.");

                var qty = ParseInt(_stock.Text, "cantidad disponible");

                if (_existing == null)
                {
                    p.Stock = qty;
                    Store.Current.CreateProduct(p);
                }
                else
                {
                    var oldStock = _existing.Stock;
                    Store.Current.UpdateProduct(p);
                    if (qty != oldStock)
                    {
                        Store.Current.CreateMovement(
                            _existing.Id,
                            "Ajuste",
                            "Conteo físico",
                            qty - oldStock,
                            "Edición de ficha");
                    }
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "beer.lab", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static int ParseInt(string s, string field)
        {
            if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) || n < 0)
                throw new InvalidOperationException("Valor inválido en " + field + ".");
            return n;
        }

        private static decimal ParseMoney(string s, string field)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            if (!decimal.TryParse(s.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out var n) || n < 0)
                throw new InvalidOperationException("Valor inválido en " + field + ".");
            return n;
        }

        private static TextBox AddField(Control parent, string label, int x, int y, int w, bool multiline = false)
        {
            var l = Theme.MutedLabel(label);
            l.Location = new Point(x, y);
            parent.Controls.Add(l);
            var t = Theme.Input();
            t.SetBounds(x, y + 18, w, multiline ? 72 : 28);
            if (multiline) t.Multiline = true;
            parent.Controls.Add(t);
            return t;
        }

        private static ComboBox AddCombo(Control parent, string label, string[] items, int x, int y, int w)
        {
            var l = Theme.MutedLabel(label);
            l.Location = new Point(x, y);
            parent.Controls.Add(l);
            var c = Theme.Combo();
            c.SetBounds(x, y + 18, w, 28);
            c.Items.AddRange(items);
            parent.Controls.Add(c);
            return c;
        }
    }
}