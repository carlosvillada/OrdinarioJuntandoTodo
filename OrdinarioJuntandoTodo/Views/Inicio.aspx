<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="OrdinarioJuntandoTodo.Views.Inicio" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tienda Online</title>
    <style>
        /* General */
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f9f9f9;
            color: #333;
        }

        .container {
            max-width: 960px;
            margin: 0 auto;
            padding: 20px;
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        /* Header */
        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
            padding: 10px;
            background-color: #007bff;
            color: #fff;
            border-radius: 8px;
        }

        .header h1 {
            margin: 0;
            font-size: 24px;
        }

        .cart-info {
            font-size: 16px;
        }

        .cart-counter {
            font-weight: bold;
            color: #fff;
        }

        /* Placeholder button */
        .login-button {
            background-color: #28a745;
            color: white;
            border: none;
            padding: 8px 12px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }

        .login-button:hover {
            background-color: #218838;
        }

        /* GridView Styles */
        .product-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

        .product-table th {
            background-color: #007bff;
            color: white;
            text-align: left;
            padding: 8px;
        }

        .product-table td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }

        .product-table tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        .product-table tr:hover {
            background-color: #ddd;
        }

        /* Button */
        .add-button {
            background-color: #28a745;
            color: white;
            border: none;
            padding: 8px 12px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }

        .add-button:hover {
            background-color: #218838;
        }

        /* Estilos para el modal */
        .modal {
            display: none;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0, 0, 0, 0.5);
            justify-content: center;
            align-items: center;
        }

        .modal-dialog {
            margin: auto;
            max-width: 600px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- Cabecera -->
            <header class="header">
                <h1>Bienvenido a la Tienda Online</h1>
                <div class="cart-info">
                    <span>Productos en el carrito: </span>
                    <asp:Label ID="lblCarrito" runat="server" Text="0" CssClass="cart-counter" />
                </div>
                <!-- botón de Log in -->
                <asp:Button ID="btnEntrar" Text="Log in" runat="server" class="login-button" OnClick="btnLogin_Click" />
                <asp:Button ID="btnConfirmarCompra" runat="server" Text="Confirmar Compra" CssClass="login-button" OnClientClick="mostrarModal(); return false;" />
            </header>

            <!-- GridView para mostrar los productos -->
            <section class="products-section">
                <asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="False" CssClass="product-table" DataKeyNames="ID_Producto,Precio">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                        <asp:BoundField DataField="Precio" HeaderText="Precio" SortExpression="Precio" DataFormatString="{0:C}" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="add-button" 
                                    OnClick="btnAgregar_Click" CommandArgument='<%# Container.DataItemIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </section>

            <!-- Modal de confirmación -->
            <div id="modalConfirmacion" class="modal" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Confirmar Compra</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Cerrar" onclick="cerrarModal();">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:GridView ID="gvCarrito" runat="server" AutoGenerateColumns="False" CssClass="product-table">
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                    <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
                                </Columns>
                            </asp:GridView>
                            <div style="margin-top: 20px;">
                                <label for="txtCorreo">Introduce tu correo electrónico:</label>
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" />
                                <asp:Label ID="lblCorreoError" runat="server" Text="" CssClass="text-danger" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnRealizarCompra" runat="server" Text="Realizar Compra" CssClass="login-button" OnClick="btnRealizarCompra_Click2" />
                            <button type="button" class="login-button" onclick="cerrarModal();">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script>
        function mostrarModal() {
            document.getElementById('modalConfirmacion').style.display = 'flex';
        }

        function cerrarModal() {
            document.getElementById('modalConfirmacion').style.display = 'none';
        }
    </script>
</body>
</html>
