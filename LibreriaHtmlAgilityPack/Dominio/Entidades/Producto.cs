using System;
using System.Collections.Generic;

namespace Dominio.Entidades
{
    public partial class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Imagen { get; set; }
        public decimal? Precio { get; set; }
    }
}
