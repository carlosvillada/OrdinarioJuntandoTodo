using System;
using OrdinarioJuntandoTodo.Models.ProductosBellezaTableAdapters;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using OrdinarioJuntandoTodo.Controller;

public class ControllerUsuario
{
    public void EncriptarYActualizarContrasenaExistente()
    {
        string connectionString = "Server=127.0.0.1;Database=ordinario;Uid=root;Pwd=SoyLuffy1P;";
        AESCryptography crypto = new AESCryptography();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Seleccionar todas las contraseñas que no están encriptadas (su longitud es <= 20)
                string querySelect = "SELECT id_usuario, contrasena FROM usuarios WHERE LENGTH(contrasena) <= 20";
                MySqlCommand selectCommand = new MySqlCommand(querySelect, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(selectCommand);
                DataTable usuarios = new DataTable();
                adapter.Fill(usuarios);

                foreach (DataRow row in usuarios.Rows)
                {
                    int id_usuario = Convert.ToInt32(row["id_usuario"]);
                    string contrasenaActual = row["contrasena"].ToString();

                    // Encriptar la contraseña solo si no está encriptada
                    string contrasenaEncriptada = crypto.Encrypt(contrasenaActual);

                    // Actualizar la contraseña en la base de datos
                    string queryUpdate = "UPDATE usuarios SET contrasena = @contrasena WHERE id_usuario = @id_usuario";
                    MySqlCommand updateCommand = new MySqlCommand(queryUpdate, connection);
                    updateCommand.Parameters.AddWithValue("@contrasena", contrasenaEncriptada);
                    updateCommand.Parameters.AddWithValue("@id_usuario", id_usuario);
                    updateCommand.ExecuteNonQuery();
                }

                Console.WriteLine("Contraseñas encriptadas exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al encriptar las contraseñas: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }



    public bool Loggin(string user, string password)
    {
        try
        {
            Console.WriteLine("Iniciando el proceso de login para el usuario: " + user);
            using (usuariosTableAdapter users = new usuariosTableAdapter())
            {
                var filausuario = users.GetDataByUsuario(user.Trim().ToLower());
                Console.WriteLine("Consulta a la base de datos ejecutada.");

                if (filausuario.Rows.Count > 0)
                {
                    string contrasenaEncriptada = filausuario.Rows[0]["contrasena"].ToString();
                    Console.WriteLine("Contraseña encriptada obtenida de la base de datos: " + contrasenaEncriptada);

                    // Si la contraseña está encriptada, desencriptar
                    AESCryptography aES = new AESCryptography();
                    string contrasenaDesencriptada = aES.Decrypt(contrasenaEncriptada);
                    Console.WriteLine("Contraseña desencriptada: " + contrasenaDesencriptada);

                    if (contrasenaDesencriptada == password && filausuario.Rows[0]["nombre_usuario"].ToString() == user)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("No se encontró ningún usuario con el nombre: " + user);
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error durante el proceso: " + ex.Message);
            throw new Exception("Error durante el proceso: " + ex.Message);
        }
    }




}