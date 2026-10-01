Partial Public Class SiteMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        ' Anti-CSRF: liga el ViewState al usuario autenticado
        If Context.User.Identity.IsAuthenticated Then
            Page.ViewStateUserKey = Context.User.Identity.Name
        End If
    End Sub

    Protected Sub lnkSalir_Click(sender As Object, e As EventArgs) Handles lnkSalir.Click
        FormsAuthentication.SignOut()
        Session.Abandon()
        Response.Redirect(FormsAuthentication.LoginUrl, False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
End Class
