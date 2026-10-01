Partial Public Class Bitacora
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then Cargar()
    End Sub

    Protected Sub ddlAccion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAccion.SelectedIndexChanged
        gvBitacora.PageIndex = 0
        Cargar()
    End Sub

    Protected Sub gvBitacora_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvBitacora.PageIndexChanging
        gvBitacora.PageIndex = e.NewPageIndex
        Cargar()
    End Sub

    Private Sub Cargar()
        Try
            gvBitacora.DataSource = New BitacoraRepository().Listar(ddlAccion.SelectedValue)
            gvBitacora.DataBind()
        Catch ex As Exception
            System.Diagnostics.Trace.TraceError(ex.ToString())
            lblMensaje.Text = "No fue posible cargar la bitácora."
            lblMensaje.Visible = True
        End Try
    End Sub
End Class
