using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {
        [DisplayName("ID")]
        public int id { get; set; }

        [DisplayName("CÓDIGO DE ARTÍCULO")]
        public string codigoArticulo { get; set; }

        [DisplayName("NOMBRE")]
        public string nombre { get; set; }

        [DisplayName("MARCA")]
        public Marca Marca { get; set; }

        [DisplayName("CATEGORÍA")]
        public Categoria tipo { get; set; }

        [DisplayName("DESCRIPCIÓN")]
        public string descripcion { get; set; }

        [DisplayName("PRECIO")]
        public decimal precio { get; set; }
        public string UrlImagen { get; set; }

    }
}

