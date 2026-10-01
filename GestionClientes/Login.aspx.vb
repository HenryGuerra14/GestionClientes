Partial Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack AndAlso User.Identity.IsAuthenticated Then
            Response.Redirect(FormsAuthentication.DefaultUrl, False)
        End If
    End Sub

    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        If Not Page.IsValid Then Return
        Dim autenticado As Boolean = False
        Dim nombreUsuario = txtUsuario.Text.Trim()
        Try
            Dim u = New UsuarioRepository().ObtenerPorNombre(nombreUsuario)
            autenticado = u IsNot Nothing AndAlso u.Activo AndAlso
                          PasswordHasher.Verificar(txtPassword.Text, u.PasswordSalt, u.PasswordHash)
            If autenticado Then nombreUsuario = u.NombreUsuario
        Catch ex As Exception
            System.Diagnostics.Trace.TraceError("Login: " & ex.ToString())
            MostrarError("No fue posible validar las credenciales. Intente más tarde.")
            Return
        End Try

        If autenticado Then
            FormsAuthentication.SetAuthCookie(nombreUsuario, False)
            Response.Redirect(FormsAuthentication.GetRedirectUrl(nombreUsuario, False), False)
            Context.ApplicationInstance.CompleteRequest()
        Else
            ' Mensaje genérico: no revelar si existe el usuario
            MostrarError("Usuario o contraseña incorrectos.")
        End If
    End Sub

    Private Sub MostrarError(msg As String)
        lblMensaje.Text = Server.HtmlEncode(msg)
        lblMensaje.Visible = True
    End Sub
End Class
