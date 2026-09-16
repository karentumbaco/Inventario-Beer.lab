using System;
using System.Collections.Generic;
using System.Linq;

namespace BeerLab
{
    internal static class Seed
    {
        public static InventoryData Create()
        {
            var data = new InventoryData();
            Add(data, "CER-PIL-330", "Pilsener Original", "Pilsener", "Cerveza", 330, "botella", 0.80m, 1.50m, 144, 48, "F-01");
            Add(data, "CER-CLB-330", "Club Premium", "Club", "Cerveza", 330, "botella", 0.90m, 1.70m, 96, 36, "F-01");
            Add(data, "CER-COR-355", "Corona Extra", "Corona", "Cerveza", 355, "botella", 1.40m, 2.50m, 72, 24, "F-02");
            Add(data, "CER-HEI-330", "Heineken", "Heineken", "Cerveza", 330, "botella", 1.30m, 2.40m, 48, 24, "F-02");
            Add(data, "CER-STL-330", "Stella Artois", "Stella Artois", "Cerveza", 330, "botella", 1.50m, 2.70m, 36, 18, "F-02");
            Add(data, "CER-GUI-440", "Guinness Draught", "Guinness", "Cerveza", 440, "lata", 2.20m, 3.80m, 18, 12, "F-03");
            Add(data, "CER-BUD-355", "Budweiser", "Budweiser", "Cerveza", 355, "lata", 1.10m, 2.00m, 8, 24, "F-03");
            Add(data, "CER-BIE-330", "Biela IPA", "Biela", "Cerveza", 330, "botella", 1.80m, 3.50m, 12, 12, "F-03");
            Add(data, "WSK-BUC-750", "Buchanan's Deluxe 12 Year", "Buchanan's", "Whisky", 750, "botella", 28.00m, 42.00m, 18, 6, "A-01");
            Add(data, "WSK-JWB-750", "Johnnie Walker Black Label", "Johnnie Walker", "Whisky", 750, "botella", 32.00m, 48.00m, 12, 4, "A-01");
            Add(data, "WSK-JWR-750", "Johnnie Walker Red Label", "Johnnie Walker", "Whisky", 750, "botella", 18.00m, 28.00m, 20, 8, "A-01");
            Add(data, "WSK-JAD-750", "Jack Daniel's Old No. 7", "Jack Daniel's", "Whisky", 750, "botella", 24.00m, 36.00m, 14, 6, "A-02");
            Add(data, "WSK-CHV-750", "Chivas Regal 12", "Chivas", "Whisky", 750, "botella", 30.00m, 45.00m, 8, 4, "A-02");
            Add(data, "WSK-OLP-750", "Old Parr 12", "Old Parr", "Whisky", 750, "botella", 26.00m, 39.00m, 10, 4, "A-02");
            Add(data, "WSK-JWG-750", "Johnnie Walker Green Label", "Johnnie Walker", "Whisky", 750, "botella", 55.00m, 79.00m, 3, 2, "A-03");
            Add(data, "WSK-MAC-750", "The Macallan 12 Double Cask", "Macallan", "Whisky", 750, "botella", 89.00m, 125.00m, 2, 2, "A-03");
            Add(data, "WSK-BSR-750", "Buchanan's Special Reserve", "Buchanan's", "Whisky", 750, "botella", 42.00m, 62.00m, 6, 3, "A-01");
            Add(data, "VDK-ABS-750", "Absolut Blue", "Absolut", "Vodka", 750, "botella", 14.00m, 22.00m, 16, 6, "B-01");
            Add(data, "VDK-SMI-750", "Smirnoff Red", "Smirnoff", "Vodka", 750, "botella", 11.00m, 18.00m, 22, 8, "B-01");
            Add(data, "VDK-GRG-750", "Grey Goose", "Grey Goose", "Vodka", 750, "botella", 32.00m, 48.00m, 5, 3, "B-02");
            Add(data, "VDK-BEL-750", "Belvedere", "Belvedere", "Vodka", 750, "botella", 34.00m, 51.00m, 0, 2, "B-02");
            Add(data, "RON-BAC-750", "Bacardi Carta Blanca", "Bacardi", "Ron", 750, "botella", 12.00m, 19.00m, 18, 6, "C-01");
            Add(data, "RON-HAV-750", "Havana Club 7 Años", "Havana Club", "Ron", 750, "botella", 18.00m, 28.00m, 10, 4, "C-01");
            Add(data, "RON-ZAC-750", "Ron Zacapa 23", "Zacapa", "Ron", 750, "botella", 45.00m, 68.00m, 4, 2, "C-02");
            Add(data, "RON-FLO-750", "Flor de Caña 7", "Flor de Caña", "Ron", 750, "botella", 20.00m, 31.00m, 7, 3, "C-01");
            Add(data, "TEQ-JCE-750", "José Cuervo Especial", "José Cuervo", "Tequila", 750, "botella", 14.00m, 22.00m, 14, 6, "D-01");
            Add(data, "TEQ-DJL-750", "Don Julio Blanco", "Don Julio", "Tequila", 750, "botella", 38.00m, 56.00m, 6, 3, "D-01");
            Add(data, "TEQ-PAT-750", "Patrón Silver", "Patrón", "Tequila", 750, "botella", 42.00m, 64.00m, 4, 2, "D-02");
            Add(data, "TEQ-180-750", "1800 Reposado", "1800", "Tequila", 750, "botella", 22.00m, 34.00m, 1, 4, "D-01");
            Add(data, "GIN-TAN-750", "Tanqueray London Dry", "Tanqueray", "Gin", 750, "botella", 18.00m, 28.00m, 12, 4, "E-01");
            Add(data, "GIN-BOM-750", "Bombay Sapphire", "Bombay", "Gin", 750, "botella", 19.00m, 29.00m, 9, 4, "E-01");
            Add(data, "GIN-BEE-750", "Beefeater", "Beefeater", "Gin", 750, "botella", 14.00m, 22.00m, 11, 4, "E-01");
            Add(data, "VIN-CAS-750", "Casillero del Diablo Cabernet", "Concha y Toro", "Vino", 750, "botella", 9.00m, 16.00m, 24, 8, "G-01");
            Add(data, "VIN-RSV-750", "Reservado Merlot", "Concha y Toro", "Vino", 750, "botella", 7.00m, 13.00m, 30, 12, "G-01");
            Add(data, "VIN-TRP-750", "Trapiche Malbec", "Trapiche", "Vino", 750, "botella", 8.50m, 15.00m, 16, 6, "G-01");
            Add(data, "VIN-MNT-750", "Mouton Cadet Rouge", "Mouton Cadet", "Vino", 750, "botella", 16.00m, 28.00m, 8, 4, "G-02");
            Add(data, "ESP-MOI-750", "Moët & Chandon Impérial", "Moët & Chandon", "Espumante", 750, "botella", 45.00m, 68.00m, 6, 3, "G-03");
            Add(data, "ESP-FRE-750", "Freixenet Cordon Negro", "Freixenet", "Espumante", 750, "botella", 14.00m, 24.00m, 10, 4, "G-03");
            Add(data, "LIC-BAI-750", "Baileys Original", "Baileys", "Licores", 750, "botella", 18.00m, 28.00m, 10, 4, "H-01");
            Add(data, "LIC-JAG-700", "Jägermeister", "Jägermeister", "Licores", 700, "botella", 16.00m, 26.00m, 8, 4, "H-01");
            Add(data, "LIC-APE-750", "Aperol", "Aperol", "Licores", 750, "botella", 14.00m, 23.00m, 7, 3, "H-01");
            Add(data, "LIC-CAM-750", "Campari", "Campari", "Licores", 750, "botella", 15.00m, 24.00m, 5, 3, "H-01");
            Add(data, "MIX-TON-200", "Schweppes Tónica", "Schweppes", "Mixers", 200, "botella", 0.60m, 1.20m, 48, 24, "M-01");
            Add(data, "MIX-COL-355", "Coca-Cola", "Coca-Cola", "Mixers", 355, "lata", 0.50m, 1.00m, 4, 36, "M-01");
            Add(data, "MIX-SOD-355", "Soda Club", "Club", "Mixers", 355, "lata", 0.40m, 0.80m, 60, 24, "M-01");

            Move(data, "CER-PIL-330", "Salida", "Venta", -24, "Pedido mesa terraza", 2);
            Move(data, "CER-COR-355", "Salida", "Venta", -12, "Venta mostrador", 3);
            Move(data, "WSK-BUC-750", "Salida", "Venta", -2, "Caja de whisky", 5);
            Move(data, "MIX-COL-355", "Salida", "Venta", -18, "Combos de cubas", 6);
            Move(data, "TEQ-180-750", "Salida", "Venta", -3, "Tequila shots evento", 8);
            Move(data, "VDK-BEL-750", "Salida", "Venta", -2, "Últimas dos botellas", 10);
            Move(data, "GIN-TAN-750", "Entrada", "Compra a proveedor", 8, "Pedido Destilería Andina", 24);
            Move(data, "CER-CLB-330", "Entrada", "Compra a proveedor", 48, "Camión Cervecería Nacional", 26);
            Move(data, "VIN-CAS-750", "Salida", "Venta", -6, "Cena privada", 28);
            Move(data, "RON-ZAC-750", "Salida", "Venta", -1, "Regalo corporativo", 32);
            Move(data, "WSK-MAC-750", "Ajuste", "Conteo físico", -1, "Conteo góndola A-03", 48);
            Move(data, "LIC-BAI-750", "Entrada", "Compra a proveedor", 6, "Reposición licores", 51);
            Move(data, "CER-BUD-355", "Salida", "Venta", -16, "After office", 54);
            Move(data, "ESP-MOI-750", "Salida", "Venta", -2, "Brindis aniversario", 72);
            Move(data, "RON-BAC-750", "Entrada", "Compra a proveedor", 12, "Pedido ron blanco", 74);
            Move(data, "TEQ-JCE-750", "Salida", "Venta", -4, "Margaritas", 77);
            Move(data, "VDK-ABS-750", "Salida", "Venta", -3, "Vodka tonic", 96);
            Move(data, "CER-HEI-330", "Entrada", "Compra a proveedor", 24, "Reposición importadas", 100);
            Move(data, "WSK-JWB-750", "Salida", "Venta", -1, "Cliente frecuente", 120);
            Move(data, "MIX-TON-200", "Entrada", "Compra a proveedor", 24, "Tónicas Schweppes", 122);
            Move(data, "GIN-BOM-750", "Salida", "Merma", -1, "Botella fisurada", 144);
            Move(data, "VIN-TRP-750", "Entrada", "Compra a proveedor", 12, "Vinos Argentina", 147);

            data.Movements = data.Movements.OrderByDescending(m => m.CreatedAt).ToList();
            return data;
        }

        private static void Add(InventoryData d, string sku, string name, string brand, string cat,
            int ml, string unit, decimal cost, decimal price, int stock, int min, string loc)
        {
            d.Products.Add(new Product
            {
                Id = d.NextProductId++,
                Sku = sku,
                Name = name,
                Brand = brand,
                Category = cat,
                SizeMl = ml,
                Unit = unit,
                Cost = cost,
                Price = price,
                Stock = stock,
                MinStock = min,
                Location = loc,
                Active = true,
                UpdatedAt = DateTime.Now
            });
        }

        private static void Move(InventoryData d, string sku, string type, string reason, int qty, string note, int hoursAgo)
        {
            var p = d.Products.First(x => x.Sku == sku);
            d.Movements.Add(new Movement
            {
                Id = d.NextMovementId++,
                ProductId = p.Id,
                ProductName = p.Name,
                ProductSku = p.Sku,
                Type = type,
                Reason = reason,
                Quantity = qty,
                StockAfter = p.Stock,
                Note = note,
                CreatedAt = DateTime.Now.AddHours(-hoursAgo)
            });
        }
    }
}
