Imports System.Configuration
Imports System.Data.SqlClient

Public Module Db
    Private ReadOnly CadenaConexion As String =
        ConfigurationManager.ConnectionStrings("GestionClientesDB").ConnectionString

    Public Function CrearConexion() As SqlConnection
        Return New SqlConnection(CadenaConexion)
    End Function

    Public Function ValorONulo(valor As String) As Object
        If String.IsNullOrWhiteSpace(valor) Then Return DBNull.Value
        Return valor.Trim()
    End Function

    Public Function LeerTexto(r As SqlDataReader, columna As String) As String
        Dim i = r.GetOrdinal(columna)
        Return If(r.IsDBNull(i), Nothing, Convert.ToString(r.GetValue(i)))
    End Function
End Module
