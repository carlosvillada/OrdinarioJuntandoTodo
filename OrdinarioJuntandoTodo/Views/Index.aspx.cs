using System;
using System.Configuration;
using System.Web.UI;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using OrdinarioJuntandoTodo.Models;
using OrdinarioJuntandoTodo.Controller;
using OrdinarioJuntandoTodo.Models.ProductosBellezaTableAdapters;
using OrdinarioJuntandoTodo.Views;



namespace OrdinarioJuntandoTodo.Views
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Aquí puedes poner cualquier lógica adicional al cargar la página
            ControllerUsuario controller = new ControllerUsuario();
            controller.EncriptarYActualizarContrasenaExistente();
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            // Obtener los datos del formulario de login
            string nombreUsuario = txtNombreUsuario.Text;
            string contrasenaIngresada = txtContrasena.Text;

            // Instancia del ControllerUsuario para validar el login
            ControllerUsuario controller = new ControllerUsuario();

            // Validar el usuario y la contraseña
            bool esValido = controller.Loggin(nombreUsuario, contrasenaIngresada);

            // Mostrar el resultado
            if (esValido)
            {
                lblMensaje.Text = "Inicio de sesión exitoso!";
                // Redirigir al usuario a la página principal o al dashboard
                Response.Redirect("AgregarProducto.aspx");
            }
            else
            {
                lblMensaje.Text = "Usuario o contraseña incorrectos.";
            }
        }
    }
}
