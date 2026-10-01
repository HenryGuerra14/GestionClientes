Public Class Usuario
    Public Property Id As Integer
    Public Property NombreUsuario As String
    Public Property NombreCompleto As String
    Public Property PasswordHash As Byte()
    Public Property PasswordSalt As Byte()
    Public Property Activo As Boolean
End Class
