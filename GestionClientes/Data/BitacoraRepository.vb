Imports System.Data
Imports System.Data.SqlClient

Public Class BitacoraRepository
    ' Se invoca SIEMPRE dentro de la transacción del cambio: o se guardan ambos o ninguno.
    Friend Shared Sub Registrar(cn As SqlConnection, tx As SqlTransaction, accion As String,
                                clienteId As Integer, usuario As String, detalle As String)
        Const sql As String =
            "INSERT INTO dbo.Bitacora (Accion, ClienteId, Usuario, Detalle) VALUES (@accion, @clienteId, @usuario, @detalle)"
        Using cmd As New SqlCommand(sql, cn, tx)
            cmd.Parameters.Add("@accion", SqlDbType.NVarChar, 10).Value = accion
            cmd.Parameters.Add("@clienteId", SqlDbType.Int).Value = clienteId
            cmd.Parameters.Add("@usuario", SqlDbType.NVarChar, 50).Value = usuario
            cmd.Parameters.Add("@detalle", SqlDbType.NVarChar, -1).Value = If(CObj(detalle), DBNull.Value)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function Listar(accion As String) As List(Of BitacoraRegistro)
        Const sql As String =
            "SELECT TOP (500) Id, Accion, ClienteId, FechaHora, Usuario, Detalle FROM dbo.Bitacora " &
            "WHERE (@accion = N'' OR Accion = @accion) ORDER BY FechaHora DESC, Id DESC"
        Dim lista As New List(Of BitacoraRegistro)
        Using cn = Db.CrearConexion(), cmd As New SqlCommand(sql, cn)
            cmd.Parameters.Add("@accion", SqlDbType.NVarChar, 10).Value = If(accion, String.Empty)
            cn.Open()
            Using r = cmd.ExecuteReader()
                While r.Read()
                    lista.Add(New BitacoraRegistro With {
                        .Id = CLng(r("Id")),
                        .Accion = CStr(r("Accion")),
                        .ClienteId = CInt(r("ClienteId")),
                        .FechaHora = CDate(r("FechaHora")),
                        .Usuario = CStr(r("Usuario")),
                        .Detalle = Db.LeerTexto(r, "Detalle")
                    })
                End While
            End Using
        End Using
        Return lista
    End Function
End Class
