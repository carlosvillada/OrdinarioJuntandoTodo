using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrdinarioJuntandoTodo.Models;
using OrdinarioJuntandoTodo.Controller;
using OrdinarioJuntandoTodo.Models.ProductosBellezaTableAdapters;

namespace OrdinarioJuntandoTodo.Views
{
    public partial class AgregarProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cualquier inicialización que solo deba ocurrir una vez aquí.
            }
        }

        // Botón agregar productos
        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text; // Nombre del producto
            string marca = txtMarca.Text;
            string categoria = txtCategoria.Text;
            decimal precio = decimal.Parse(txtPrecio.Text);
            int cantidad_disponible = int.Parse(txtCantidad.Text);
            string descripcion = txtDescripcion.Text;
            DateTime fecha = DateTime.Parse(txtFecha.Text);

            // Insertar el producto en la base de datos
            productoTableAdapter productos = new productoTableAdapter();
            productos.Insert(nombre, marca, categoria, precio, cantidad_disponible, descripcion, fecha);

            // Mostrar un mensaje de éxito
            lblMessage.Text = "Producto agregado exitosamente.";
            lblMessage.ForeColor = System.Drawing.Color.Green;

            // Limpiar los campos del formulario
            LimpiarCampos();
        }

        // Botón mostrar productos
        protected void BtnMostrar_Click(object sender, EventArgs e)
        {
            CargarProductos();
        }

        // Método para cargar productos en el GridView
        private void CargarProductos()
        {
            Productos productosController = new Productos();
            var productos = productosController.ObtenerProductos();

            if (productos.Count > 0)
            {
                GridDatos.DataSource = productos;
                GridDatos.DataBind();
            }
            else
            {
                lblMessage.Text = "No se encontraron productos.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }



        // Manejar el evento de actualizar producto
        protected void CargarProductoParaEdicion(int idProducto)
        {
            Productos productosController = new Productos();
            var productos = productosController.ObtenerProductos();
            var producto = productos.Find(p => p.ID_Producto == idProducto);

            if (producto != null)
            {
                txtNombre.Text = producto.Nombre;
                txtMarca.Text = producto.Marca;
                txtCategoria.Text = producto.Categoria;
                txtPrecio.Text = producto.Precio.ToString();
                txtCantidad.Text = producto.Cantidad_Disponible.ToString();
                txtDescripcion.Text = producto.Descripcion;
                txtFecha.Text = producto.Fecha_Caducidad.ToString("yyyy-MM-dd");

                // Guardamos el ID del producto para usarlo en la actualización
                ViewState["ID_Producto"] = producto.ID_Producto;
            }
        }


        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            // Obtener los valores desde los controles de la página
            int idProducto = Convert.ToInt32(ViewState["ID_Producto"]);  // Suponiendo que el ID se guarda en ViewState
            string nombre = txtNombre.Text;
            string marca = txtMarca.Text;
            string categoria = txtCategoria.Text;
            decimal precio = decimal.Parse(txtPrecio.Text);
            int cantidadDisponible = int.Parse(txtCantidad.Text);
            string descripcion = txtDescripcion.Text;
            DateTime fechaCaducidad = DateTime.Parse(txtFecha.Text);

            // Crear una instancia de la clase Productos
            Productos productos = new Productos();

            // Llamar al método para actualizar el producto
            bool exito = productos.ActualizarProducto(nombre, marca, categoria, precio, cantidadDisponible, descripcion, fechaCaducidad, idProducto);

            if (exito)
            {
                lblMessage.Text = "Producto actualizado exitosamente.";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMessage.Text = "Error al actualizar el producto.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }


        // Funciona Actializar pero no Eliminar
        protected void GridDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ActualizarProducto")
            {
                int idProducto = Convert.ToInt32(e.CommandArgument); // Obtener el ID del producto desde el CommandArgument

                // Cargar los datos del producto en los controles de texto
                //CargarProductoParaEdicion(idProducto);

                // Aquí iría el código para actualizar el producto.
                // Deberás tomar los valores de los controles de texto y pasar esos datos a la base de datos.

                string nombre = txtNombre.Text;
                string marca = txtMarca.Text;
                string categoria = txtCategoria.Text;
                decimal precio = decimal.Parse(txtPrecio.Text);
                int cantidad_disponible = int.Parse(txtCantidad.Text);
                string descripcion = txtDescripcion.Text;
                DateTime fecha = DateTime.Parse(txtFecha.Text);

                // Crear un TableAdapter para actualizar el producto
                productoTableAdapter productosAdapter = new productoTableAdapter();

                // Ejecutar la consulta de actualización
                productosAdapter.Update(nombre, marca, categoria, precio, cantidad_disponible, descripcion, fecha, idProducto);

                // Mensaje de éxito
                lblMessage.Text = "Producto actualizado exitosamente.";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                // Limpiar los campos
                LimpiarCampos();

                // Volver a cargar los productos para reflejar los cambios
                CargarProductos();
            }


            if (e.CommandName == "EliminarProducto")
            {
                // Obtener el ID del producto que se quiere eliminar
                int idProducto = Convert.ToInt32(e.CommandArgument);

                // Crear una instancia de la clase Productos
                Productos productosController = new Productos();

                // Llamar al método de eliminación en el controlador
                bool eliminado = productosController.EliminarProducto(idProducto);

                // Verificar si la eliminación fue exitosa
                if (eliminado)
                {
                    // Mostrar un mensaje de éxito
                    lblMessage.Text = "Producto eliminado con éxito.";
                    lblMessage.CssClass = "message success";
                }
                else
                {
                    // Mostrar un mensaje de error
                    lblMessage.Text = "Error al eliminar el producto.";
                    lblMessage.CssClass = "message error";
                }

                // Actualizar la lista de productos
                CargarProductos(); // Método que actualiza el GridView con los productos actuales
            }


        }



        protected void btnVolverInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Inicio.aspx");
        }



        // Limpiar los campos del formulario
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtMarca.Text = "";
            txtCategoria.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";
            txtDescripcion.Text = "";
            txtFecha.Text = "";
        }
    }
}
