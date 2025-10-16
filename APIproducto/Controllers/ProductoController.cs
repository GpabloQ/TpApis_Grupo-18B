using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIproducto.Models;
using Dominio;
using Negocio;

namespace APIproducto.Controllers
{
    public class ProductoController : ApiController
    {
        // GET: api/Producto
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Producto/5
        public Articulo Get(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> lista = negocio.listar();
            return lista.Find(x => x.id == id);
        }


        // POST: api/Producto
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Producto/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Producto/5
        public void Delete(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            negocio.EliminarArticulo(id);
        }
    }
}
