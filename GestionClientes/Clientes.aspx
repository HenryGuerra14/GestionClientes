<%@ Page Title="Clientes" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Clientes.aspx.vb" Inherits="GestionClientes.Clientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h3 class="mb-0">Clientes</h3>
        <asp:Button ID="btnNuevo" runat="server" Text="+ Nuevo cliente" CssClass="btn btn-success" CausesValidation="false" />
    </div>

    <asp:Label ID="lblMensaje" runat="server" Visible="false" EnableViewState="false" />

    <asp:Panel ID="pnlFormulario" runat="server" Visible="false" CssClass="card mb-4 shadow-sm" DefaultButton="btnGuardar">
        <div class="card-header fw-bold"><asp:Literal ID="litTitulo" runat="server" /></div>
        <div class="card-body">
            <asp:HiddenField ID="hfClienteId" runat="server" Value="0" />
            <asp:ValidationSummary runat="server" ValidationGroup="cli" CssClass="alert alert-danger" />
            <div class="row g-3">
                <div class="col-md-6">
                    <label class="form-label">Nombres *</label>
                    <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control" MaxLength="100" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombres" ValidationGroup="cli" Display="None" ErrorMessage="Los nombres son obligatorios." />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Apellidos *</label>
                    <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control" MaxLength="100" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellidos" ValidationGroup="cli" Display="None" ErrorMessage="Los apellidos son obligatorios." />
                </div>
                <div class="col-md-4">
                    <label class="form-label">DUI * <small class="text-muted">(00000000-0)</small></label>
                    <asp:TextBox ID="txtDUI" runat="server" CssClass="form-control" MaxLength="10" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDUI" ValidationGroup="cli" Display="None" ErrorMessage="El DUI es obligatorio." />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDUI" ValidationGroup="cli" Display="None" ValidationExpression="^\d{8}-\d$" ErrorMessage="El DUI debe tener el formato 00000000-0." />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="150" TextMode="Email" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="cli" Display="None" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="El email no es válido." />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="20" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtTelefono" ValidationGroup="cli" Display="None" ValidationExpression="^[0-9+\-\s]{8,20}$" ErrorMessage="El teléfono no es válido." />
                </div>
                <div class="col-12">
                    <label class="form-label">Dirección</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" MaxLength="250" />
                </div>
            </div>
            <div class="mt-3">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" ValidationGroup="cli" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" CausesValidation="false" />
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlBuscar" runat="server" CssClass="input-group mb-3" DefaultButton="btnBuscar">
        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar por nombre, apellido o DUI" MaxLength="100" />
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-primary" CausesValidation="false" />
    </asp:Panel>

    <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
        CssClass="table table-striped table-hover align-middle bg-white" GridLines="None"
        AllowPaging="true" PageSize="10" EmptyDataText="No se encontraron clientes.">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" />
            <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre" />
            <asp:BoundField DataField="DUI" HeaderText="DUI" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-nowrap">
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-primary" CausesValidation="false">Editar</asp:LinkButton>
                    <asp:LinkButton runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-danger" CausesValidation="false"
                        OnClientClick="return confirm('¿Seguro que desea eliminar este cliente?');">Eliminar</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="pagination-sm" />
    </asp:GridView>
</asp:Content>
