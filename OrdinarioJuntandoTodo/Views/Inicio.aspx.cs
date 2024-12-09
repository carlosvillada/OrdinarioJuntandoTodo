using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using OrdinarioJuntandoTodo.Models;
using OrdinarioJuntandoTodo.Models.ProductosBellezaTableAdapters;

namespace OrdinarioJuntandoTodo.Views
{
    public partial class Inicio : System.Web.UI.Page
    {
        // Tabla estática para simular un carrito (en caso de usar un backend real, esto sería temporal)
        private static DataTable carrito = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos(); // Cargar productos al inicio

                // Inicializar carrito si no existe en sesión
                if (Session["Carrito"] == null)
                {
                    Session["Carrito"] = new List<CarritoItem>();
                }
            }
        }

        // Función para agregar productos al carrito
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnAgregar = (Button)sender;
                GridViewRow row = (GridViewRow)btnAgregar.NamingContainer;
                int index = row.RowIndex;

                // Asegúrate de que las claves existen
                if (gvProductos.DataKeys[index] != null)
                {
                    int idProducto = Convert.ToInt32(gvProductos.DataKeys[index].Values["ID_Producto"]);
                    decimal precioProducto = Convert.ToDecimal(gvProductos.DataKeys[index].Values["Precio"]);
                    string nombreProducto = row.Cells[0].Text;

                    List<CarritoItem> carrito = (List<CarritoItem>)Session["Carrito"];
                    var productoExistente = carrito.FirstOrDefault(p => p.ID_Producto == idProducto);

                    if (productoExistente != null)
                    {
                        productoExistente.Cantidad++;
                    }
                    else
                    {
                        carrito.Add(new CarritoItem
                        {
                            ID_Producto = idProducto,
                            Nombre = nombreProducto,
                            Precio = precioProducto,
                            Cantidad = 1
                        });
                    }

                    Session["Carrito"] = carrito;
                    ActualizarCarrito();
                }
                else
                {
                    lblCarrito.Text = "No se pudo encontrar la información del producto.";
                }
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al agregar producto: " + ex.Message;
            }
        }

        private void ActualizarCarrito()
        {
            try
            {
                List<CarritoItem> carrito = (List<CarritoItem>)Session["Carrito"];
                gvCarrito.DataSource = carrito.Select(item => new
                {
                    item.Nombre,
                    item.Cantidad,
                    item.Precio,
                    Subtotal = item.Cantidad * item.Precio
                }).ToList();
                gvCarrito.DataBind();

                lblCarrito.Text = carrito.Sum(item => item.Cantidad).ToString();
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al actualizar carrito: " + ex.Message;
            }
        }

        // Cargar productos desde la base de datos al GridView
        private void CargarProductos()
        {
            productoTableAdapter adapter = new productoTableAdapter();
            DataTable productos = adapter.GetData();

            gvProductos.DataSource = productos;
            gvProductos.DataBind();
        }

        // Mostrar el carrito de compras en el modal
        protected void btnConfirmarCompra_Click(object sender, EventArgs e)
        {
            try
            {
                List<CarritoItem> carrito = (List<CarritoItem>)Session["Carrito"];
                var carritoDisplay = carrito.Select(item => new
                {
                    item.Nombre,
                    item.Cantidad,
                    item.Precio,
                    Subtotal = item.Cantidad * item.Precio
                }).ToList();

                gvCarrito.DataSource = carritoDisplay;
                gvCarrito.DataBind();
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al mostrar carrito: " + ex.Message;
            }
        }

        // Procesar compra
        protected void btnRealizarCompra_Click(object sender, EventArgs e)
        {
            lblCarrito.Text = "¡Compra realizada con éxito!";
            Session["Carrito"] = new List<CarritoItem>(); // Vaciar carrito
        }

        // Redirección al inicio de sesión
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }

        // Clase para representar productos en el carrito
        public class CarritoItem
        {
            public int ID_Producto { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
            public int Cantidad { get; set; }
        }

        // ----------------------------- Lo de arriba de aquí funciona todo bien ---------------------------------------------------

        private bool ActualizarInventario(string nombreProducto, int cantidadVendida)
        {
            string connectionString = "server=127.0.0.1;database=ordinario;uid=root;pwd=SoyLuffy1P;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Primero verificamos si hay suficiente stock
                string verificarQuery = "SELECT Cantidad_Disponible FROM Producto WHERE Nombre = @Nombre";
                using (MySqlCommand verificarCmd = new MySqlCommand(verificarQuery, connection))
                {
                    verificarCmd.Parameters.AddWithValue("@Nombre", nombreProducto);
                    object resultado = verificarCmd.ExecuteScalar();

                    if (resultado == null || Convert.ToInt32(resultado) < cantidadVendida)
                    {
                        return false; // No hay suficiente stock
                    }
                }

                // Actualizamos el inventario
                string actualizarQuery = "UPDATE Producto SET Cantidad_Disponible = Cantidad_Disponible - @Cantidad WHERE Nombre = @Nombre";
                using (MySqlCommand actualizarCmd = new MySqlCommand(actualizarQuery, connection))
                {
                    actualizarCmd.Parameters.AddWithValue("@Cantidad", cantidadVendida);
                    actualizarCmd.Parameters.AddWithValue("@Nombre", nombreProducto);
                    actualizarCmd.ExecuteNonQuery();
                }
            }

            return true; // Operación exitosa
        }

        // ----------------------------- Lo de arriba de aquí funciona todo bien ---------------------------------------------------
        private string GenerarPdfRecibo(List<CarritoItem> carrito)
        {
            string filePath = Server.MapPath("~/Recibos/recibo_compra.pdf");

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Título
                doc.Add(new Paragraph("Recibo de Compra"));
                doc.Add(new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                doc.Add(new Paragraph("\n"));

                // Tabla
                PdfPTable table = new PdfPTable(4); // Nombre, Cantidad, Precio, Subtotal
                table.AddCell("Nombre");
                table.AddCell("Cantidad");
                table.AddCell("Precio");
                table.AddCell("Subtotal");

                decimal total = 0;

                foreach (var item in carrito)
                {
                    table.AddCell(item.Nombre);
                    table.AddCell(item.Cantidad.ToString());
                    table.AddCell(item.Precio.ToString("C"));
                    decimal subtotal = item.Cantidad * item.Precio;
                    table.AddCell(subtotal.ToString("C"));
                    total += subtotal;
                }

                doc.Add(table);

                // Total
                doc.Add(new Paragraph($"\nTotal: {total.ToString("C")}"));
                doc.Close();
            }

            return filePath;
        }

        public void GuardarRecibo()
        {
            // Define la ruta de la carpeta y el archivo
            string folderPath = @"C:\Users\villa\source\repos\OrdinarioJuntandoTodo\OrdinarioJuntandoTodo\Recibos";

            // Verifica si la carpeta existe; si no, la crea
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Define la ruta completa del archivo
            string filePath = Path.Combine(folderPath, "recibo_compra.pdf");

            // Código para generar y guardar el PDF
            File.WriteAllText(filePath, "Contenido del recibo"); // Ejemplo
        }

        private void EnviarCorreoConRecibo(string correoDestino, string archivoPdf)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("carlosvilladagonzalez@gmail.com");
                mail.To.Add(correoDestino);
                mail.Subject = "Recibo de tu Compra";
                mail.Body = "Gracias por tu compra. Adjunto encontrarás tu recibo en formato PDF.";
                mail.Attachments.Add(new Attachment(archivoPdf));

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("carlosvilladagonzalez@gmail.com", "fbyk oabt hfoe vyqt");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al enviar el correo: " + ex.Message;
            }
        }

        protected void btnRealizarCompra_Click2(object sender, EventArgs e)
        {
            GuardarRecibo();

            try
            {
                List<CarritoItem> carrito = (List<CarritoItem>)Session["Carrito"];
                if (carrito == null || !carrito.Any())
                {
                    lblCarrito.Text = "El carrito está vacío.";
                    return;
                }

                // Obtener el correo proporcionado por el usuario
                string correoUsuario = txtCorreo.Text.Trim();
                if (string.IsNullOrEmpty(correoUsuario))
                {
                    lblCarrito.Text = "Por favor, ingresa tu correo para completar la compra.";
                    return;
                }

                // Validar el formato del correo electrónico
                try
                {
                    var addr = new System.Net.Mail.MailAddress(correoUsuario);
                    if (addr.Address != correoUsuario)
                    {
                        lblCarrito.Text = "El correo proporcionado no es válido.";
                        return;
                    }
                }
                catch
                {
                    lblCarrito.Text = "El correo proporcionado no es válido.";
                    return;
                }

                // Actualizar inventario
                foreach (var item in carrito)
                {
                    if (!ActualizarInventario(item.Nombre, item.Cantidad))
                    {
                        lblCarrito.Text = $"No hay suficiente stock para el producto: {item.Nombre}.";
                        return;
                    }
                }

                // Generar PDF
                string pdfPath = GenerarPdfRecibo(carrito);

                // Enviar el correo al usuario y una copia al administrador
                EnviarCorreoConRecibo(correoUsuario, pdfPath); // Al usuario
                EnviarCorreoConRecibo("carlosvilladagonzalez@gmail.com", pdfPath); // Al administrador

                // Vaciar carrito y mostrar mensaje
                Session["Carrito"] = new List<CarritoItem>();
                ActualizarCarrito();
                lblCarrito.CssClass = "text-success";
                lblCarrito.Text = "Compra realizada con éxito. Recibo enviado a tu correo.";
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al realizar la compra: " + ex.Message;
            }
        }

    }



}
