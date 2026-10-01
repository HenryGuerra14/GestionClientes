Imports System.Data.SqlClient

Partial Public Class Clientes
    Inherits System.Web.UI.Page

    Private ReadOnly _repo As New ClienteRepository()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then CargarClientes()
    End Sub

    Private Sub CargarClientes()
        Try
            gvClientes.DataSource = _repo.Listar(txtBuscar.Text)
            gvClientes.DataBind()
        Catch ex As Exception
            Registrar(ex)
            MostrarMensaje("No fue posible cargar los clientes.", "danger")
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        gvClientes.PageIndex = 0
        CargarClientes()
    End Sub

    Protected Sub gvClientes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvClientes.PageIndexChanging
        gvClientes.PageIndex = e.NewPageIndex
        CargarClientes()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarFormulario()
        litTitulo.Text = "Nuevo cliente"
        pnlFormulario.Visible = True
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarFormulario()
        pnlFormulario.Visible = False
    End Sub

    Protected Sub gvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        If e.CommandName <> "Editar" AndAlso e.CommandName <> "Eliminar" Then Return ' p.ej. "Page"
        Dim id As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), id) Then Return

        Try
            If e.CommandName = "Editar" Then
                Dim c = _repo.ObtenerPorId(id)
                If c Is Nothing Then
                    MostrarMensaje("El cliente ya no existe.", "warning")
                Else
                    CargarFormulario(c)
                End If
            Else
                If _repo.Eliminar(id, User.Identity.Name) Then
                    MostrarMensaje("Cliente eliminado correctamente.", "success")
                Else
                    MostrarMensaje("El cliente ya no existe.", "warning")
                End If
                pnlFormulario.Visible = False
            End If
        Catch ex As Exception
            Registrar(ex)
            MostrarMensaje("Ocurrió un error al procesar la solicitud.", "danger")
        End Try
        CargarClientes()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Page.Validate("cli")
        If Not Page.IsValid Then Return ' validación en servidor, aunque se omita el JS

        Dim c As New Cliente With {
            .Id = CInt(hfClienteId.Value),
            .Nombres = txtNombres.Text,
            .Apellidos = txtApellidos.Text,
            .DUI = txtDUI.Text,
            .Email = txtEmail.Text,
            .Telefono = txtTelefono.Text,
            .Direccion = txtDireccion.Text
        }

        Try
            If c.Id = 0 Then
                _repo.Insertar(c, User.Identity.Name)
                MostrarMensaje("Cliente agregado correctamente.", "success")
            ElseIf _repo.Actualizar(c, User.Identity.Name) Then
                MostrarMensaje("Cliente actualizado correctamente.", "success")
            Else
                MostrarMensaje("El cliente ya no existe.", "warning")
            End If
            LimpiarFormulario()
            pnlFormulario.Visible = False
            CargarClientes()
        Catch ex As SqlException When ex.Number = 2601 OrElse ex.Number = 2627
            MostrarMensaje("Ya existe un cliente registrado con ese DUI.", "danger")
        Catch ex As Exception
            Registrar(ex)
            MostrarMensaje("Ocurrió un error al guardar el cliente.", "danger")
        End Try
    End Sub

    ' ---------- auxiliares ----------
    Private Sub CargarFormulario(c As Cliente)
        hfClienteId.Value = c.Id.ToString()
        txtNombres.Text = c.Nombres
        txtApellidos.Text = c.Apellidos
        txtDUI.Text = c.DUI
        txtEmail.Text = c.Email
        txtTelefono.Text = c.Telefono
        txtDireccion.Text = c.Direccion
        litTitulo.Text = "Editar cliente #" & c.Id
        pnlFormulario.Visible = True
    End Sub

    Private Sub LimpiarFormulario()
        hfClienteId.Value = "0"
        For Each tb In {txtNombres, txtApellidos, txtDUI, txtEmail, txtTelefono, txtDireccion}
            tb.Text = String.Empty
        Next
    End Sub

    Private Sub MostrarMensaje(texto As String, tipo As String)
        lblMensaje.Text = Server.HtmlEncode(texto)
        lblMensaje.CssClass = "alert alert-" & tipo & " d-block"
        lblMensaje.Visible = True
    End Sub

    Private Shared Sub Registrar(ex As Exception)
        System.Diagnostics.Trace.TraceError(ex.ToString())
    End Sub
End Class
