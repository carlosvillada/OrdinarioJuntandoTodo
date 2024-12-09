using System;
using System.Collections.Generic;
using System.Data;  // Para DataTable y DataRow
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OrdinarioJuntandoTodo.Models.ProductosBellezaTableAdapters;

namespace OrdinarioJuntandoTodo.Controller
{
    public class Productos
    {
        // Método para obtener productos
        public List<Productos> ObtenerProductos()
        {
            try
            {
                using (productoTableAdapter productosAdapter = new productoTableAdapter())
                {
                    var dt = productosAdapter.GetData();  // Obtiene todos los registros de productos

                    if (dt.Rows.Count > 0)
                    {
                        List<Productos> listaProductos = new List<Productos>();

                        foreach (DataRow row in dt.Rows)
                        {
                            Productos producto = new Productos
                            {
                                ID_Producto = Convert.ToInt32(row["id_producto"]),  // Asegúrate de que este campo existe en tu base de datos
                                Nombre = row["nombre"].ToString(),
                                Marca = row["marca"].ToString(),
                                Categoria = row["categoria"].ToString(),
                                Precio = Convert.ToDecimal(row["precio"]),
                                Cantidad_Disponible = Convert.ToInt32(row["cantidad_disponible"]),
                                Descripcion = row["descripcion"].ToString(),
                                Fecha_Caducidad = Convert.ToDateTime(row["fecha_caducidad"])
                            };
                            listaProductos.Add(producto);
                        }


                        return listaProductos;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return new List<Productos>();
        }

        // Método para eliminar un producto por su ID
        public bool EliminarProducto(int idProducto)
        {
            try
            {
                using (productoTableAdapter productosAdapter = new productoTableAdapter())
                {
                    // Llamamos al método de eliminación en el TableAdapter
                    productosAdapter.DeleteProducto(idProducto);
                    return true; // Retornamos true si la eliminación fue exitosa
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el producto: " + ex.Message);
                return false; // Retornamos false si ocurre un error
            }
        }


        // Método para actualizar un producto
        // Método para actualizar un producto
        public bool ActualizarProducto(string nombre, string marca, string categoria, decimal precio, int cantidadDisponible, string descripcion, DateTime fechaCaducidad, int idProducto)
        {
            try
            {
                using (productoTableAdapter productosAdapter = new productoTableAdapter())
                {
                    // Llamamos al método de actualización en el TableAdapter
                    productosAdapter.UpdateProducto(nombre, marca, categoria, precio, cantidadDisponible, descripcion, fechaCaducidad, idProducto);
                    return true; // Retornamos true si la actualización fue exitosa
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar el producto: " + ex.Message);
                return false; // Retornamos false si ocurre un error
            }
        }



        // Método para obtener un producto específico por su ID
        public Productos ObtenerProductoPorID(int idProducto)
        {
            try
            {
                using (productoTableAdapter productosAdapter = new productoTableAdapter())
                {
                    // Obtiene los datos del producto con el ID proporcionado
                    var dt = productosAdapter.GetDataByID(idProducto); // Asegúrate de tener este método en tu TableAdapter

                    if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0]; // Obtenemos la primera (y única) fila
                        Productos producto = new Productos
                        {
                            ID_Producto = Convert.ToInt32(row["id_producto"]),
                            Nombre = row["nombre"].ToString(),
                            Marca = row["marca"].ToString(),
                            Categoria = row["categoria"].ToString(),
                            Precio = Convert.ToDecimal(row["precio"]),
                            Cantidad_Disponible = Convert.ToInt32(row["cantidad_disponible"]),
                            Descripcion = row["descripcion"].ToString(),
                            Fecha_Caducidad = Convert.ToDateTime(row["fecha_caducidad"])
                        };

                        return producto; // Devolvemos el producto encontrado
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el producto: " + ex.Message);
            }
            return null; // Retornamos null si no se encontró el producto o hubo un error
        }



        // Constructor que recibe parámetros
        public Productos(string nombre, string marca, string categoria, decimal precio, int cantidad_disponible, string descripcion, DateTime fecha_caducidad)
        {
            Nombre = nombre;
            Marca = marca;
            Categoria = categoria;
            Precio = precio;
            Cantidad_Disponible = cantidad_disponible;
            Descripcion = descripcion;
            Fecha_Caducidad = fecha_caducidad;
        }

        // Constructor sin parámetros
        public Productos()
        {
        }

        // Propiedades
        public int ID_Producto { get; set; }  // Asegúrate de tener una propiedad para el ID
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad_Disponible { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_Caducidad { get; set; }


        // Todo lo de arriba jala--------------------------------------------------------------------------------------
        // Nuevo método para realizar una compra
        public void RealizarCompra(int idProducto, int cantidad)
        {
            // Obtener detalles del producto
            Productos producto = ObtenerProductoPorID(idProducto);
            if (producto == null)
            {
                Console.WriteLine("Producto no encontrado.");
                return;
            }

            decimal totalCompra = producto.Precio * cantidad;

            // Obtener el correo del usuario desde la base de datos
            string correoElectronico = ObtenerCorreoDeUsuario();

            if (string.IsNullOrEmpty(correoElectronico))
            {
                Console.WriteLine("Correo electrónico del usuario no encontrado.");
                return;
            }

            // Generar el PDF del recibo de compra
            string archivoPdf = GenerarPdfRecibo(producto, cantidad, totalCompra);

            // Enviar el PDF por correo electrónico
            EnviarCorreo(correoElectronico, archivoPdf);
        }

        // Nuevo método para obtener el correo electrónico del usuario
        private string ObtenerCorreoDeUsuario()
        {
            try
            {
                string correo = string.Empty;
                using (MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=yourpassword;database=ordinario"))
                {
                    conn.Open();
                    string query = "SELECT correo_electronico FROM usuarios WHERE nombre_usuario = 'cvillada'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        correo = reader.GetString("correo_electronico");
                    }
                }
                return correo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el correo del usuario: " + ex.Message);
                return null;
            }
        }

        // Nuevo método para generar el PDF del recibo
        private string GenerarPdfRecibo(Productos producto, int cantidad, decimal totalCompra)
        {
            string filePath = @"C:\path\to\save\recibo_compra.pdf"; // Cambia esta ruta según tu entorno

            Document document = new Document();
            PdfWriter.GetInstance(document, new System.IO.FileStream(filePath, System.IO.FileMode.Create));
            document.Open();

            // Contenido del PDF
            document.Add(new Paragraph("Recibo de Compra"));
            document.Add(new Paragraph($"Producto: {producto.Nombre}"));
            document.Add(new Paragraph($"Cantidad: {cantidad}"));
            document.Add(new Paragraph($"Precio: {producto.Precio:C}"));
            document.Add(new Paragraph($"Total: {totalCompra:C}"));
            document.Add(new Paragraph($"Fecha de compra: {DateTime.Now}"));

            document.Close();
            return filePath;
        }

        // Nuevo método para enviar el correo con el PDF adjunto
        private void EnviarCorreo(string correoDestino, string archivoAdjunto)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your_email@gmail.com", "your_email_password"),
                    EnableSsl = true
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("your_email@gmail.com"),
                    Subject = "Recibo de Compra",
                    Body = "Gracias por tu compra. Adjunto te enviamos tu recibo.",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(correoDestino);

                // Adjuntar el archivo PDF
                Attachment attachment = new Attachment(archivoAdjunto);
                mailMessage.Attachments.Add(attachment);

                smtpClient.Send(mailMessage);
                Console.WriteLine("Correo enviado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }
    }
}
