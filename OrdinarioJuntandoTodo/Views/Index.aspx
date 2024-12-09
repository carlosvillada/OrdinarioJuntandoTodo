<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OrdinarioJuntandoTodo.Views.Index" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }

        .login-container {
            background-color: white;
            border-radius: 8px;
            padding: 30px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            width: 100%;
            max-width: 400px;
        }

        h2 {
            text-align: center;
            color: #333;
        }

        label {
            font-size: 14px;
            color: #555;
            display: block;
            margin-bottom: 6px;
        }

        .form-control {
            width: 100%;
            padding: 10px;
            margin-bottom: 20px;
            border-radius: 4px;
            border: 1px solid #ddd;
            font-size: 14px;
        }

        .form-control:focus {
            border-color: #4A90E2;
            outline: none;
        }

        .btn-submit {
            background-color: #4A90E2;
            color: white;
            padding: 10px;
            border: none;
            border-radius: 4px;
            width: 100%;
            cursor: pointer;
            font-size: 16px;
        }

        .btn-submit:hover {
            background-color: #357ABD;
        }

        .message {
            color: red;
            font-size: 14px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form runat="server" class="login-container">
        <h2>Iniciar sesión</h2>

        <!-- TextBox para el nombre de usuario -->
        <label for="txtNombreUsuario">Nombre de usuario:</label>
        <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" /><br />

        <!-- TextBox para la contraseña -->
        <label for="txtContrasena">Contraseña:</label>
        <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control" /><br />

        <!-- Botón para iniciar sesión -->
        <asp:Button ID="btnEntrar" runat="server" Text="Entrar" OnClick="btnEntrar_Click" CssClass="btn-submit" /><br />

        <!-- Label para mostrar mensajes -->
        <asp:Label ID="lblMensaje" runat="server" CssClass="message" />
    </form>
</body>
</html>
