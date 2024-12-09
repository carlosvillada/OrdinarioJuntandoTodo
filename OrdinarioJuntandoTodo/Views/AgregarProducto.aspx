<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgregarProducto.aspx.cs" Inherits="OrdinarioJuntandoTodo.Views.AgregarProducto" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agregar Producto</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f9f9f9;
        }
        .container {
            max-width: 600px;
            margin: auto;
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            font-weight: bold;
            display: block;
            margin-bottom: 5px;
        }
        .form-group input, .form-group textarea {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
        }
        .form-group textarea {
            resize: none;
        }
        .btn {
            padding: 10px 20px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            text-align: center;
        }
        .btn:hover {
            background-color: #218838;
        }
        .btn-secondary {
            background-color: #007bff;
        }

        .btn-inicio {
            background-color: coral;
        }

        .btn-secondary:hover {
            background-color: #0056b3;
        }
        .btn-warning {
            background-color: #ffc107;
        }
        .btn-warning:hover {
            background-color: #e0a800;
        }
        .message {
            margin-top: 20px;
            font-size: 14px;
        }
        table {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }
        table th, table td {
            padding: 10px;
            border: 1px solid #ddd;
            text-align: left;
        }
        table th {
            background-color: #f1f1f1;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Agregar Producto</h2>
            <div class="form-group">
                <label for="txtNombre">Nombre del Producto:</label>
                <asp:TextBox ID="txtNombre" runat="server" placeholder="Nombre"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtMarca">Marca:</label>
                <asp:TextBox ID="txtMarca" runat="server" placeholder="Marca"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtCategoria">Categoría:</label>
                <asp:TextBox ID="txtCategoria" runat="server" placeholder="Categoría"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtPrecio">Precio:</label>
                <asp:TextBox ID="txtPrecio" runat="server" placeholder="Precio"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtCantidad">Cantidad Disponible:</label>
                <asp:TextBox ID="txtCantidad" runat="server" placeholder="Cantidad"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtDescripcion">Descripción:</label>
                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="3" placeholder="Descripción"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtFecha">Fecha:</label>
                <asp:TextBox ID="txtFecha" runat="server" placeholder="yyyy-MM-dd"></asp:TextBox>
            </div>
            <asp:Button ID="btnAgregarProducto" runat="server" Text="Agregar Producto" CssClass="btn" OnClick="btnAgregarProducto_Click" />
            <asp:Button ID="BtnMostrar" runat="server" Text="Mostrar Productos" CssClass="btn btn-secondary" OnClick="BtnMostrar_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
            <asp:Button ID="btnVolverInicio" runat="server" Text="Volver a Inicio" CssClass="btn btn-inicio" OnClick="btnVolverInicio_Click" />


            <!-- GridView para mostrar y actualizar los productos -->
            <asp:GridView ID="GridDatos" runat="server" AutoGenerateColumns="false" CssClass="grid" OnRowCommand="GridDatos_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ID_Producto" HeaderText="ID" SortExpression="IdProducto" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                    <asp:BoundField DataField="Marca" HeaderText="Marca" SortExpression="Marca" />
                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" SortExpression="Categoria" />
                    <asp:BoundField DataField="Precio" HeaderText="Precio" SortExpression="Precio" />
                    <asp:BoundField DataField="Cantidad_Disponible" HeaderText="Cantidad" SortExpression="CantidadDisponible" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                    <asp:BoundField DataField="Fecha_Caducidad" HeaderText="Fecha" SortExpression="Fecha" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" 
                                        CommandName="ActualizarProducto" 
                                        CommandArgument='<%# Eval("ID_Producto") %>' 
                                        CssClass="btn btn-warning" />
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                                        CommandName="EliminarProducto" 
                                        CommandArgument='<%# Eval("ID_Producto") %>' 
                                        CssClass="btn btn-danger" 
                                        OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este producto?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>