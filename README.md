# beer.lab — Inventario para Visual Studio Community

Aplicación de escritorio Windows (C# WinForms). **No usa Node.js ni npm.**

## Cómo abrirla

1. Instala [Visual Studio Community](https://visualstudio.microsoft.com/es/vs/community/) (gratis).
2. En el instalador marca la carga de trabajo **Desarrollo para el escritorio con .NET**.
3. Doble clic en **`beer-lab.sln`**.
4. Pulsa **F5** (o el botón verde Iniciar).

Se abre la ventana de beer.lab: Panel, Inventario, Movimientos y Alertas.

## Uso

- **Doble clic** en un producto para editarlo o registrar stock.
- **Producto** / **Nuevo producto** para dar de alta un SKU.
- **Entrada** / **Salida** para compras y ventas. El stock se actualiza solo.
- Los datos se guardan en `%AppData%\beer.lab\inventario.xml`.
- **Restaurar demo** (abajo a la izquierda) vuelve al catálogo de ejemplo.

## Requisitos

- Windows 10 u 11
- Visual Studio Community 2019 o 2022
- .NET Framework 4.8 (viene con Windows)
