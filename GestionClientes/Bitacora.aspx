<%@ Page Title="Bitácora" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Bitacora.aspx.vb" Inherits="GestionClientes.Bitacora" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="mb-3">Bitácora de acciones</h3>
    <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-danger d-block" Visible="false" EnableViewState="false" />
    <div class="row g-2 mb-3" style="max-width:400px">
        <div class="col">
            <asp:DropDownList ID="ddlAccion" runat="server" CssClass="form-select" AutoPostBack="true">
                <asp:ListItem Value="" Text="Todas las acciones" />
                <asp:ListItem Value="AGREGAR" Text="Agregar" />
                <asp:ListItem Value="EDITAR" Text="Editar" />
                <asp:ListItem Value="ELIMINAR" Text="Eliminar" />
            </asp:DropDownList>
        </div>
    </div>
    <asp:GridView ID="gvBitacora" runat="server" AutoGenerateColumns="false"
        CssClass="table table-sm table-striped bg-white" GridLines="None"
        AllowPaging="true" PageSize="15" EmptyDataText="Sin registros.">
        <Columns>
            <asp:BoundField DataField="FechaHora" HeaderText="Fecha y hora" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
            <asp:BoundField DataField="Accion" HeaderText="Acción" />
            <asp:BoundField DataField="ClienteId" HeaderText="ID Cliente" />
            <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
            <asp:BoundField DataField="Detalle" HeaderText="Detalle" ItemStyle-CssClass="small" />
        </Columns>
    </asp:GridView>
</asp:Content>
