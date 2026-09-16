using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace BeerLab
{
    internal sealed class MovementDialog : Form
    {
        private readonly ComboBox _product, _type, _reason;
        private readonly TextBox _qty, _note;
        private readonly Label _stock;

        public MovementDialog(int? productId = null, string type = "Salida")
        {
            Text = "Registrar movimiento";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(480, 460);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Theme.Bg;
            ForeColor = Theme.Fg;
            Font = Theme.Ui;

            var y = 20;
            _product = AddCombo("Producto", 20, ref y);
            foreach (var p in Store.Current.ActiveProducts())
                _product.Items.Add(new Item(p.Id, p.Sku + "  ·  " + p.Name));
            if (productId.HasValue)
            {
                foreach (Item it in _product.Items)
                    if (it.Id == productId.Value) { _product.SelectedItem = it; break; }
                _product.Enabled = !productId.HasValue || _product.SelectedItem != null;
            }

            _stock = new Label { ForeColor = Theme.Muted, Font = Theme.Small, AutoSize = true, Location = new Point(20, y - 4) };
            Controls.Add(_stock);
            y += 16;

            _type = AddCombo("Tipo", 20, ref y);
            _type.Items.AddRange(Catalog.Types);
            _type.SelectedItem = type;
            _reason = AddCombo("Motivo", 20, ref y);
            FillReasons();
            _qty = AddField(type == "Ajuste" ? "Cantidad (+ o −)" : "Cantidad", 20, ref y);
            _qty.Text = "1";
            _note = AddField("Nota (opcional)", 20, ref y, true);

            _type.SelectedIndexChanged += (_, __) => FillReasons();
            _product.SelectedIndexChanged += (_, __) => UpdateStock();
            UpdateStock();

            var save = Theme.PrimaryButton("Registrar");
            save.SetBounds(Width - 240, Height - 90, 110, 40);
            save.Click += (_, __) => Accept();
            var cancel = Theme.GhostButton("Cancelar");
            cancel.SetBounds(Width - 120, Height - 90, 90, 40);
            cancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(save);
            Controls.Add(cancel);
            AcceptButton = save;
            CancelButton = cancel;
        }

        private void FillReasons()
        {
            var t = Convert.ToString(_type.SelectedItem) ?? "Salida";
            _reason.Items.Clear();
            _reason.Items.AddRange(Catalog.ReasonsFor(t));
            if (_reason.Items.Count > 0) _reason.SelectedIndex = 0;
        }

        private void UpdateStock()
        {
            var it = _product.SelectedItem as Item;
            if (it == null) { _stock.Text = ""; return; }
            var p = Store.Current.GetProduct(it.Id);
            _stock.Text = p == null ? "" : "Stock actual: " + p.Stock + "  ·  " + p.Estado;
        }

        private void Accept()
        {
            try
            {
                var it = _product.SelectedItem as Item;
                if (it == null) throw new InvalidOperationException("Elige un producto.");
                var type = Convert.ToString(_type.SelectedItem) ?? "Salida";
                if (!int.TryParse(_qty.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var qty))
                    throw new InvalidOperationException("Cantidad inválida.");
                Store.Current.CreateMovement(it.Id, type, Convert.ToString(_reason.SelectedItem) ?? "", qty, _note.Text.Trim());
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "beer.lab", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private ComboBox AddCombo(string label, int x, ref int y)
        {
            var l = Theme.MutedLabel(label);
            l.Location = new Point(x, y);
            Controls.Add(l);
            y += 18;
            var c = Theme.Combo();
            c.SetBounds(x, y, 420, 28);
            Controls.Add(c);
            y += 40;
            return c;
        }

        private TextBox AddField(string label, int x, ref int y, bool multi = false)
        {
            var l = Theme.MutedLabel(label);
            l.Location = new Point(x, y);
            Controls.Add(l);
            y += 18;
            var t = Theme.Input();
            t.SetBounds(x, y, 420, multi ? 56 : 28);
            if (multi) t.Multiline = true;
            Controls.Add(t);
            y += t.Height + 12;
            return t;
        }

        private sealed class Item
        {
            public int Id { get; }
            public string Label { get; }
            public Item(int id, string label) { Id = id; Label = label; }
            public override string ToString() => Label;
        }
    }
}
