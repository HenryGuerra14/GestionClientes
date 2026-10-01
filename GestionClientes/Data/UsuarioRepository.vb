Imports System.Data
Imports System.Data.SqlClient

Public Class UsuarioRepository
    Public Function ObtenerPorNombre(nombreUsuario As String) As Usuario
        Const sql As String =
            "SELECT Id, NombreUsuario, NombreCompleto, PasswordHash, PasswordSalt, Activo " &
            "FROM dbo.Usuarios WHERE NombreUsuario = @usuario"
        Using cn = Db.CrearConexion(), cmd As New SqlCommand(sql, cn)
            cmd.Parameters.Add("@usuario", SqlDbType.NVarChar, 50).Value = nombreUsuario
            cn.Open()
            Using r = cmd.ExecuteReader(CommandBehavior.SingleRow)
                If Not r.Read() Then Return Nothing
                Return New Usuario With {
                    .Id = CInt(r("Id")),
                    .NombreUsuario = CStr(r("NombreUsuario")),
                    .NombreCompleto = CStr(r("NombreCompleto")),
                    .PasswordHash = CType(r("PasswordHash"), Byte()),
                    .PasswordSalt = CType(r("PasswordSalt"), Byte()),
                    .Activo = CBool(r("Activo"))
                }
            End Using
        End Using
    End Function
End Class
