using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Razor.Generator;
using APIproducto.Models;
using Negocio;

namespace APIproducto.Controllers
{
    public class imagenesController : ApiController
    {
        // GET: api/imagenes
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/imagenes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/imagenes
        public HttpResponseMessage Post(int id, [FromBody]ImagenDto imagen)
        {
            if (imagen == null || imagen.imagenes == null || imagen.imagenes.Count == 0)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Debe enviar una lista de imágenes válida.");

            var ArticuloNegocio = new ArticuloNegocio();

            bool existe = ArticuloNegocio.Existe(id);
            if (!existe)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "el producto no existe");
            
            int agregadas = 0;
            List<string> errores = new List<string>();

            foreach (var url in imagen.imagenes)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        ArticuloNegocio.AgregarImagen(id, url);
                        agregadas++;
                    }
                    else
                    {
                        errores.Add("URL vacía o inválida.");
                    }
                }
                catch (Exception ex)
                {
                    errores.Add(url + "("+ ex.Message + ")");
                }
            }

            // 3️⃣ Respuesta final
            var result = new
            {
                idProducto = id,
                agregadas = agregadas,
                errores = errores
            };

            return Request.CreateResponse(HttpStatusCode.Created, result);


        }

        // PUT: api/imagenes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/imagenes/5
        public void Delete(int id)
        {
        }
    }
}
