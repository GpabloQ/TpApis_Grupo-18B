using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIproducto.Models;
using Dominio;
using Negocio;
using APIproducto.Models;

namespace APIproducto.Controllers
{
    public class ProductoController : ApiController
    {
        // GET: api/Producto
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            return negocio.listar2();
        }

        // GET: api/Producto/5
        public Articulo Get(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> lista = negocio.listar2();
            return lista.Find(x => x.id == id);
        }
        // GET: api/Producto/Buscar/nombre

        [HttpGet]
        [Route("api/Producto/{nombre}")]
        public IHttpActionResult Buscar(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El parámetro 'nombre' es requerido.");
            }
            ArticuloNegocio negocio = new ArticuloNegocio();
            var articulos = negocio.BuscarProducto(nombre);
            if (articulos == null || !articulos.Any())
            {
                return NotFound();
            }
            return Ok(articulos);
        }
       

        // POST: api/Producto
        public void Post([FromBody]ProductoDTO prod)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo nuevo = new Articulo();

            nuevo.codigoArticulo = prod.codigoArticulo;
            nuevo.nombre = prod.nombre;
            nuevo.Marca = new Marca { Id = prod.idMarca };
            nuevo.tipo = new Categoria { Id = prod.idCategoria };
            nuevo.descripcion = prod.descripcion;
            nuevo.precio = prod.precio;
            nuevo.UrlImagen = prod.UrlImagen;

            negocio.agregarArticulo(nuevo);

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
