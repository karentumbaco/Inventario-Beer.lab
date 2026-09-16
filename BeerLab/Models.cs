using System;
using System.Collections.Generic;

namespace BeerLab
{
    public class InventoryData
    {
        public int NextProductId { get; set; } = 1;
        public int NextMovementId { get; set; } = 1;
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Movement> Movements { get; set; } = new List<Movement>();
    }

    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } = "";
        public string Name { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Category { get; set; } = "";
        public int SizeMl { get; set; }
        public string Unit { get; set; } = "botella";
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSix { get; set; }
        public decimal PriceCaja { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        public string Location { get; set; } = "";
        public string Notes { get; set; } = "";
        public bool Active { get; set; } = true;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool EsCerveza => string.Equals(Category, "Cerveza", StringComparison.OrdinalIgnoreCase);
        public string Estado
        {
            get
            {
                if (!Active) return "Archivado";
                if (Stock <= 0) return "Agotado";
                if (Stock <= MinStock) return "Bajo mínimo";
                return "En nivel";
            }
        }

        public decimal ValorCosto => Stock * Cost;
        public decimal ValorVenta => Stock * Price;
    }

    public class Movement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public string ProductSku { get; set; } = "";
        public string Type { get; set; } = "Salida";
        public string Reason { get; set; } = "";
        public int Quantity { get; set; }
        public int StockAfter { get; set; }
        public string Note { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public static class Catalog
    {
        public static readonly string[] Categories =
        {
            "Cerveza", "Whisky", "Vodka", "Ron", "Tequila",
            "Gin", "Vino", "Espumante", "Licores", "Mixers"
        };

        public static readonly string[] Units = { "botella", "lata", "pack", "caja" };

        public static readonly string[] Types = { "Entrada", "Salida", "Ajuste" };

        public static string[] ReasonsFor(string type)
        {
            switch (type)
            {
                case "Entrada":
                    return new[] { "Compra a proveedor", "Devolución de cliente", "Traslado / ingreso", "Stock inicial" };
                case "Salida":
                    return new[] { "Venta", "Merma", "Caducado", "Devolución a proveedor" };
                default:
                    return new[] { "Conteo físico", "Corrección" };
            }
        }
    }
}
