using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using APIproducto.Models;
using Dominio;
using Negocio;


namespace APIproducto.Controllers
{
    public class ProductoController : ApiController
    {
        // GET: api/Producto
        [HttpGet]
        [Route("api/Producto/")]
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            return negocio.listar2();
        }

        // GET: api/Producto/5
        [HttpGet]
        [Route("api/Producto/{id}")]
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
        [HttpPost]
        [Route("api/Producto/")]
        public HttpResponseMessage Post([FromBody]ProductoDTO prod)

        {
            var varnegocio = new ArticuloNegocio();
            var MarcaNegocio = new MarcaNegocio();
            var CategoriaNegocio = new CategoriaNegocio();

            if (prod == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Body requerido.");
            
            if (prod.precio <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "El precio debe ser un número positivo.");
            }
            if (string.IsNullOrWhiteSpace(prod.codigoArticulo) ||
    string.IsNullOrWhiteSpace(prod.nombre) ||
    string.IsNullOrWhiteSpace(prod.descripcion))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Todos los campos obligatorios deben estar completos.");
            }

            // Validar que existan la marca y la categoría
            Marca marca = MarcaNegocio.listar().Find(x => x.Id == prod.idMarca);
            Categoria categoria = CategoriaNegocio.listar().Find(x => x.Id == prod.idCategoria);

            if (marca == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "La Marca no existe.");

            if (categoria == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "La Categoria no existe.");

          //  ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo nuevo = new Articulo();

            nuevo.codigoArticulo = prod.codigoArticulo;
            nuevo.nombre = prod.nombre;
            nuevo.Marca = new Marca { Id = prod.idMarca };
            nuevo.tipo = new Categoria { Id = prod.idCategoria };
            nuevo.descripcion = prod.descripcion;
            nuevo.precio = prod.precio;
            nuevo.UrlImagen = prod.UrlImagen;

            varnegocio.agregarArticulo(nuevo);

            return Request.CreateResponse(HttpStatusCode.OK, "Artículo agregado correctamente.");

        }

        // PUT: api/Producto/5
        [HttpPut]
        [Route("api/Producto/{id}")]
        public HttpResponseMessage Put(int id, [FromBody] ProductoDTO producto)

        {
            if (producto == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Body requerido.");

            if (producto.precio <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "El precio debe ser un número positivo.");
            }
            if (string.IsNullOrWhiteSpace(producto.codigoArticulo) ||
    string.IsNullOrWhiteSpace(producto.nombre) ||
    string.IsNullOrWhiteSpace(producto.descripcion))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Todos los campos obligatorios deben estar completos.");
            }
            ArticuloNegocio negocio = new ArticuloNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            
            // Validar existencia de la Marca y la Categoría
            Marca marca = marcaNegocio.listar().Find(x => x.Id == producto.idMarca);
            Categoria categoria = categoriaNegocio.listar().Find(x => x.Id == producto.idCategoria);

            if (marca == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La marca seleccionada no existe.");

            if (categoria == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La categoría seleccionada no existe.");


            try
            {
          
                Articulo modificar = new Articulo();

                modificar.id = id;
                modificar.codigoArticulo = producto.codigoArticulo;
                modificar.nombre = producto.nombre;
                modificar.Marca = new Marca { Id = producto.idMarca };
                modificar.tipo = new Categoria { Id = producto.idCategoria };
                modificar.descripcion = producto.descripcion;
                modificar.precio = producto.precio;
                modificar.UrlImagen = producto.UrlImagen;


                negocio.modificarProducto(modificar);
                
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    mensaje = "Producto actualizado correctamente.",
                    productoActualizado = modificar
                });

            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }


        }

        // DELETE: api/Producto/5
        [HttpDelete]
        [Route("api/Producto/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            // Validar que exista el id articulo
            Articulo articulo = negocio.listar().Find(x => x.id == id);
           
            if (articulo == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "El id artículo no es válido.");

            negocio.EliminarArticulo(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Artículo eliminado correctamente.");
        }
    }
}
