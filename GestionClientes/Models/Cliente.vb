Public Class Cliente
    Public Property Id As Integer
    Public Property Nombres As String
    Public Property Apellidos As String
    Public Property DUI As String
    Public Property Email As String
    Public Property Telefono As String
    Public Property Direccion As String
    Public Property FechaCreacion As DateTime
    Public Property FechaModificacion As DateTime?

    Public ReadOnly Property NombreCompleto As String
        Get
            Return $"{Nombres} {Apellidos}"
        End Get
    End Property

    Public Function Describir() As String
        Return $"Nombres={Nombres}; Apellidos={Apellidos}; DUI={DUI}; Email={Email}; Tel={Telefono}; Dir={Direccion}"
    End Function
End Class
