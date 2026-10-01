<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="GestionClientes.Login" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Iniciar sesión</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light d-flex align-items-center" style="min-height:100vh">
    <form id="form1" runat="server" class="container" style="max-width:400px">
        <div class="card shadow-sm">
            <div class="card-body p-4">
                <h4 class="mb-4 text-center">Gestión de Clientes</h4>
                <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-danger d-block" Visible="false" EnableViewState="false" />
                <div class="mb-3">
                    <label class="form-label" for="txtUsuario">Usuario</label>
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" MaxLength="50" autocomplete="username" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsuario" ErrorMessage="Ingrese el usuario." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="mb-3">
                    <label class="form-label" for="txtPassword">Contraseña</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" MaxLength="100" autocomplete="current-password" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" ErrorMessage="Ingrese la contraseña." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-primary w-100" />
            </div>
        </div>
    </form>
</body>
</html>
