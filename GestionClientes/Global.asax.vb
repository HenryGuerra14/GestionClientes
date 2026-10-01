Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Error(sender As Object, e As EventArgs)
        ' Registra cualquier error no controlado; el usuario ve Error.aspx (customErrors)
        Dim ex = Server.GetLastError()
        If ex IsNot Nothing Then System.Diagnostics.Trace.TraceError("No controlado: " & ex.ToString())
    End Sub
End Class
