using System;
using System.Collections.Generic;

namespace Dominio.Entidades
{
    public partial class HistorialProducto
    {
        public int? IdProducto { get; set; }
        public int? IdCoproducto { get; set; }

        public virtual Producto? IdCoproductoNavigation { get; set; }
        public virtual Producto? IdProductoNavigation { get; set; }
    }
}
