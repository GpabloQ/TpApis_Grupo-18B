using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Negocio
{
    public class ArticuloNegocio
    {

        public List<Articulo> listar() { 
            
            List <Articulo>lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            
            try
            {
                datos.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, M.Descripcion AS Marca, C.Descripcion AS Tipo, A.Descripcion, A.Precio, I.ImagenUrl AS Imagen,A.IdCategoria,A.IdMarca FROM ARTICULOS A LEFT JOIN IMAGENES I ON I.IdArticulo = A.Id JOIN MARCAS M ON M.Id = A.IdMarca JOIN CATEGORIAS C ON C.Id = A.IdCategoria");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {

                    Articulo aux = new Articulo();
                    aux.id = (int)datos.Lector["Id"];
                    aux.codigoArticulo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.tipo = new Categoria();
                    aux.tipo.Id = (int)datos.Lector["IdCategoria"];
                    aux.tipo.Descripcion = (string)datos.Lector["Tipo"];
                    
                    if (!(datos.Lector["Descripcion"] is DBNull))
                    aux.descripcion = (string)datos.Lector["Descripcion"];
                    aux.precio = (decimal)datos.Lector["Precio"];
                    
                    if (!(datos.Lector["Imagen"] is DBNull))
                    aux.UrlImagen = (string)datos.Lector["Imagen"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                datos.cerrarConexion();    
            }
        }

        public List<Articulo> listarUnaSolaImagen()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT A.Id, A.Codigo, A.Nombre, 
                   M.Descripcion AS Marca, 
                   C.Descripcion AS Tipo, 
                   A.Descripcion, 
                   A.Precio, 
                   (SELECT TOP 1 I.ImagenUrl 
                    FROM IMAGENES I 
                    WHERE I.IdArticulo = A.Id) AS Imagen, 
                   A.IdCategoria, 
                   A.IdMarca
            FROM ARTICULOS A
            JOIN MARCAS M ON M.Id = A.IdMarca
            JOIN CATEGORIAS C ON C.Id = A.IdCategoria
        ");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.id = (int)datos.Lector["Id"];
                    aux.codigoArticulo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];

                    aux.Marca = new Marca
                    {
                        Id = (int)datos.Lector["IdMarca"],
                        Descripcion = (string)datos.Lector["Marca"]
                    };

                    aux.tipo = new Categoria
                    {
                        Id = (int)datos.Lector["IdCategoria"],
                        Descripcion = (string)datos.Lector["Tipo"]
                    };

                    if (!(datos.Lector["Descripcion"] is DBNull))
                        aux.descripcion = (string)datos.Lector["Descripcion"];

                    aux.precio = (decimal)datos.Lector["Precio"];

                    if (!(datos.Lector["Imagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["Imagen"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregarArticulo(Articulo nuevo) {

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria,Precio) values (@Codigo,@Nombre,@Descripcion,@IdMarca,@IdCategoria,@Precio); insert into IMAGENES (ImagenUrl,IdArticulo) values (@imagen,SCOPE_IDENTITY())");
                datos.setearParametro("@Codigo", nuevo.codigoArticulo);
                datos.setearParametro("@Nombre", nuevo.nombre);
                datos.setearParametro("@IdMarca", nuevo.Marca.Id);
                datos.setearParametro("@IdCategoria", nuevo.tipo.Id);
                datos.setearParametro("@Descripcion", nuevo.descripcion);
                datos.setearParametro("@Precio", nuevo.precio);
                datos.setearParametro("@imagen",nuevo.UrlImagen);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally {
                datos.cerrarConexion();
            }



        }

        public void modificarArticulo(Articulo articulo, string urlVieja, string urlNueva)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Primero actualizamos los datos del artículo
                datos.setearConsulta(@"
            UPDATE ARTICULOS 
            SET Codigo = @cod, Nombre = @nom, IdMarca = @idmarca, IdCategoria = @idcategoria, 
                Descripcion = @desc, Precio = @precio 
            WHERE Id = @Id;
            
            -- Actualizamos solo la imagen específica que coincide con la URL vieja
            UPDATE IMAGENES 
            SET ImagenUrl = @nuevaUrl 
            WHERE IdArticulo = @Id AND ImagenUrl = @urlVieja;
        ");

                datos.setearParametro("@cod", articulo.codigoArticulo);
                datos.setearParametro("@nom", articulo.nombre);
                datos.setearParametro("@idmarca", articulo.Marca.Id);
                datos.setearParametro("@idcategoria", articulo.tipo.Id);
                datos.setearParametro("@desc", articulo.descripcion);
                datos.setearParametro("@precio", articulo.precio);
                datos.setearParametro("@Id", articulo.id);

                datos.setearParametro("@urlVieja", urlVieja);
                datos.setearParametro("@nuevaUrl", urlNueva);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el artículo: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }



        public void EliminarArticulo(int id) { 
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("delete from ARTICULOS where id = @id");
                datos.setearParametro("@id",id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarImagen(int idArticulo, string urlImagen)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Borra la imagen que coincida con el artículo y la URL
                datos.setearConsulta("DELETE FROM IMAGENES WHERE IdArticulo = @idArticulo AND ImagenUrl = @urlImagen");
                datos.setearParametro("@idArticulo", idArticulo);
                datos.setearParametro("@urlImagen", urlImagen);
                datos.ejecutarAccion();
            }
            catch (Exception )
            {
                throw new Exception("Error al eliminar la imagen.");
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void AgregarImagen(int idArticulo, string urlImagen)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@idArticulo, @urlImagen)");
                datos.setearParametro("@idArticulo", idArticulo);
                datos.setearParametro("@urlImagen", urlImagen);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la imagen: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public List<Articulo> filtrarPorPrecio(decimal precioMax)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, M.Descripcion AS Marca, C.Descripcion AS Tipo, A.Descripcion, A.Precio, I.ImagenUrl AS Imagen,A.IdCategoria,A.IdMarca FROM ARTICULOS A LEFT JOIN IMAGENES I ON I.IdArticulo = A.Id JOIN MARCAS M ON M.Id = A.IdMarca JOIN CATEGORIAS C ON C.Id = A.IdCategoria");
               

                datos.setearParametro("@PrecioMax", precioMax);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.id = (int)datos.Lector["Id"];
                    aux.codigoArticulo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.tipo = new Categoria();
                    aux.tipo.Id = (int)datos.Lector["IdCategoria"];
                    aux.tipo.Descripcion = (string)datos.Lector["Tipo"];

                    if (!(datos.Lector["Descripcion"] is DBNull))
                        aux.descripcion = (string)datos.Lector["Descripcion"];

                    aux.precio = (decimal)datos.Lector["Precio"];

                    if (!(datos.Lector["Imagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["Imagen"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


    }
}
