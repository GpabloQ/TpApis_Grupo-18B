using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace APIproducto.Models
{
    public class ProductoDTO
    {
       public string codigoArticulo { get; set; }
        public string nombre { get; set; }
        public int idMarca { get; set; }
        public int idCategoria { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public string UrlImagen { get; set; }       

    }
}