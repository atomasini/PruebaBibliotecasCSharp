using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades;
namespace ProductRecommendation
{
   
    public class ProductosSQL
    { private PW3TiendaContext context;
        public ProductosSQL() {

            // Crea una instancia del contexto del Entity Framework
            context = new PW3TiendaContext();
        }

        public List<Producto> GetProductos() {

            // Obtiene la lista de productos desde el contexto y la devuelve
            return context.Productos.ToList();
        }

        public List<HistorialProducto> GetHistorial()
        {

            // Obtiene la lista de Historial-producto desde el contexto y la devuelve
            return context.HistorialProductos.ToList();
        }

    }
}
