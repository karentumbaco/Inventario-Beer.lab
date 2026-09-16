using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BeerLab
{
    public sealed class Store
    {
        public static readonly Store Current = new Store();

        private readonly string _path;
        public InventoryData Data { get; private set; } = new InventoryData();

        public event Action Changed;

        private Store()
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "beer.lab");
            Directory.CreateDirectory(dir);
            _path = Path.Combine(dir, "inventario.xml");
            Load();
        }

        public IEnumerable<Product> ActiveProducts()
        {
            return Data.Products.Where(p => p.Active).OrderBy(p => p.Name);
        }

        public void Save()
        {
            var ser = new XmlSerializer(typeof(InventoryData));
            using (var fs = File.Create(_path))
                ser.Serialize(fs, Data);
            Changed?.Invoke();
        }

        public void Load()
        {
            if (!File.Exists(_path))
            {
                Data = Seed.Create();
                Save();
                return;
            }

            try
            {
                var ser = new XmlSerializer(typeof(InventoryData));
                using (var fs = File.OpenRead(_path))
                    Data = (InventoryData)ser.Deserialize(fs) ?? Seed.Create();
            }
            catch
            {
                Data = Seed.Create();
            }
        }

        public void ResetDemo()
        {
            Data = Seed.Create();
            Save();
        }

        public Product GetProduct(int id) => Data.Products.FirstOrDefault(p => p.Id == id);

        public Product CreateProduct(Product p)
        {
            var sku = (p.Sku ?? "").Trim().ToUpperInvariant();
            if (Data.Products.Any(x => x.Active && x.Sku == sku))
                throw new InvalidOperationException("Ya existe un producto con ese SKU.");
            p.Id = Data.NextProductId++;
            p.Sku = sku;
            p.Active = true;
            p.UpdatedAt = DateTime.Now;
            Data.Products.Add(p);
            if (p.Stock > 0)
            {
                Data.Movements.Insert(0, new Movement
                {
                    Id = Data.NextMovementId++,
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductSku = p.Sku,
                    Type = "Entrada",
                    Reason = "Stock inicial",
                    Quantity = p.Stock,
                    StockAfter = p.Stock,
                    Note = "Alta de producto",
                    CreatedAt = DateTime.Now
                });
            }
            Save();
            return p;
        }

        public void UpdateProduct(Product p)
        {
            var sku = (p.Sku ?? "").Trim().ToUpperInvariant();
            if (Data.Products.Any(x => x.Active && x.Sku == sku && x.Id != p.Id))
                throw new InvalidOperationException("Ya existe un producto con ese SKU.");
            var cur = GetProduct(p.Id);
            if (cur == null) throw new InvalidOperationException("Producto no encontrado.");
            cur.Sku = sku;
            cur.Name = p.Name;
            cur.Brand = p.Brand;
            cur.Category = p.Category;
            cur.SizeMl = p.SizeMl;
            cur.Unit = p.Unit;
            cur.Cost = p.Cost;
            cur.Price = p.Price;
            cur.PriceSix = p.PriceSix;
            cur.PriceCaja = p.PriceCaja;
            cur.MinStock = p.MinStock;
            cur.Location = p.Location;
            cur.Notes = p.Notes;
            cur.UpdatedAt = DateTime.Now;
            Save();
        }

        public void Archive(int id)
        {
            var p = GetProduct(id);
            if (p == null) return;
            p.Active = false;
            p.UpdatedAt = DateTime.Now;
            Save();
        }

        public void DeleteProduct(int id)
        {
            var p = GetProduct(id);
            if (p == null) throw new InvalidOperationException("Producto no encontrado.");
            Data.Movements.RemoveAll(m => m.ProductId == id);
            Data.Products.Remove(p);
            Save();
        }

        public Movement CreateMovement(int productId, string type, string reason, int quantity, string note)
        {
            var p = GetProduct(productId);
            if (p == null || !p.Active) throw new InvalidOperationException("Producto no encontrado.");
            if (quantity == 0) throw new InvalidOperationException("La cantidad no puede ser 0.");

            int delta;
            if (type == "Entrada") delta = Math.Abs(quantity);
            else if (type == "Salida") delta = -Math.Abs(quantity);
            else delta = quantity;

            var next = p.Stock + delta;
            if (next < 0)
                throw new InvalidOperationException("Stock insuficiente. Hay " + p.Stock + " unidades.");

            p.Stock = next;
            p.UpdatedAt = DateTime.Now;
            var m = new Movement
            {
                Id = Data.NextMovementId++,
                ProductId = p.Id,
                ProductName = p.Name,
                ProductSku = p.Sku,
                Type = type,
                Reason = reason,
                Quantity = delta,
                StockAfter = next,
                Note = note ?? "",
                CreatedAt = DateTime.Now
            };
            Data.Movements.Insert(0, m);
            Save();
            return m;
        }
    }
}