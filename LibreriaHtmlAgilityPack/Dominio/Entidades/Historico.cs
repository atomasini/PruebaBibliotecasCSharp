using System;
using System.Collections.Generic;

namespace Dominio.Entidades
{
    public partial class Historico
    {
        public int Id { get; set; }
        public int? IdProducto { get; set; }
        public int? IdCoproducto { get; set; }
        public int? Puntaje { get; set; }
    }
}
