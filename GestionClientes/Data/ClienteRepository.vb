Imports System.Data
Imports System.Data.SqlClient

Public Class ClienteRepository
    Private Const Columnas As String =
        "Id, Nombres, Apellidos, DUI, Email, Telefono, Direccion, FechaCreacion, FechaModificacion"

    Public Function Listar(filtro As String) As List(Of Cliente)
        Dim sql = $"SELECT {Columnas} FROM dbo.Clientes " &
                  "WHERE (@filtro = N'' OR Nombres LIKE @like OR Apellidos LIKE @like OR DUI LIKE @like) " &
                  "ORDER BY Apellidos, Nombres"
        Dim f = If(filtro, String.Empty).Trim()
        Dim lista As New List(Of Cliente)
        Using cn = Db.CrearConexion(), cmd As New SqlCommand(sql, cn)
            cmd.Parameters.Add("@filtro", SqlDbType.NVarChar, 100).Value = f
            cmd.Parameters.Add("@like", SqlDbType.NVarChar, 102).Value = "%" & f & "%"
            cn.Open()
            Using r = cmd.ExecuteReader()
                While r.Read()
                    lista.Add(Mapear(r))
                End While
            End Using
        End Using
        Return lista
    End Function

    Public Function ObtenerPorId(id As Integer) As Cliente
        Using cn = Db.CrearConexion()
            cn.Open()
            Return ObtenerPorId(id, cn, Nothing)
        End Using
    End Function

    Public Function Insertar(c As Cliente, usuario As String) As Integer
        Const sql As String =
            "INSERT INTO dbo.Clientes (Nombres, Apellidos, DUI, Email, Telefono, Direccion) " &
            "OUTPUT INSERTED.Id VALUES (@nombres, @apellidos, @dui, @email, @telefono, @direccion)"
        Using cn = Db.CrearConexion()
            cn.Open()
            Using tx = cn.BeginTransaction() ' Si no hay Commit, Dispose hace Rollback
                Dim nuevoId As Integer
                Using cmd As New SqlCommand(sql, cn, tx)
                    AgregarParametros(cmd, c)
                    nuevoId = CInt(cmd.ExecuteScalar())
                End Using
                BitacoraRepository.Registrar(cn, tx, "AGREGAR", nuevoId, usuario, c.Describir())
                tx.Commit()
                Return nuevoId
            End Using
        End Using
    End Function

    Public Function Actualizar(c As Cliente, usuario As String) As Boolean
        Const sql As String =
            "UPDATE dbo.Clientes SET Nombres=@nombres, Apellidos=@apellidos, DUI=@dui, Email=@email, " &
            "Telefono=@telefono, Direccion=@direccion, FechaModificacion=SYSDATETIME() WHERE Id=@id"
        Using cn = Db.CrearConexion()
            cn.Open()
            Using tx = cn.BeginTransaction()
                Dim anterior = ObtenerPorId(c.Id, cn, tx)
                If anterior Is Nothing Then Return False
                Using cmd As New SqlCommand(sql, cn, tx)
                    AgregarParametros(cmd, c)
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = c.Id
                    cmd.ExecuteNonQuery()
                End Using
                BitacoraRepository.Registrar(cn, tx, "EDITAR", c.Id, usuario,
                    $"ANTES: {anterior.Describir()} | DESPUÉS: {c.Describir()}")
                tx.Commit()
                Return True
            End Using
        End Using
    End Function

    Public Function Eliminar(id As Integer, usuario As String) As Boolean
        Using cn = Db.CrearConexion()
            cn.Open()
            Using tx = cn.BeginTransaction()
                Dim anterior = ObtenerPorId(id, cn, tx)
                If anterior Is Nothing Then Return False
                Using cmd As New SqlCommand("DELETE FROM dbo.Clientes WHERE Id=@id", cn, tx)
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id
                    cmd.ExecuteNonQuery()
                End Using
                BitacoraRepository.Registrar(cn, tx, "ELIMINAR", id, usuario, anterior.Describir())
                tx.Commit()
                Return True
            End Using
        End Using
    End Function

    ' ---------- privados ----------
    Private Function ObtenerPorId(id As Integer, cn As SqlConnection, tx As SqlTransaction) As Cliente
        Using cmd As New SqlCommand($"SELECT {Columnas} FROM dbo.Clientes WHERE Id=@id", cn, tx)
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id
            Using r = cmd.ExecuteReader(CommandBehavior.SingleRow)
                Return If(r.Read(), Mapear(r), Nothing)
            End Using
        End Using
    End Function

    Private Shared Sub AgregarParametros(cmd As SqlCommand, c As Cliente)
        cmd.Parameters.Add("@nombres", SqlDbType.NVarChar, 100).Value = c.Nombres.Trim()
        cmd.Parameters.Add("@apellidos", SqlDbType.NVarChar, 100).Value = c.Apellidos.Trim()
        cmd.Parameters.Add("@dui", SqlDbType.Char, 10).Value = c.DUI.Trim()
        cmd.Parameters.Add("@email", SqlDbType.NVarChar, 150).Value = Db.ValorONulo(c.Email)
        cmd.Parameters.Add("@telefono", SqlDbType.NVarChar, 20).Value = Db.ValorONulo(c.Telefono)
        cmd.Parameters.Add("@direccion", SqlDbType.NVarChar, 250).Value = Db.ValorONulo(c.Direccion)
    End Sub

    Private Shared Function Mapear(r As SqlDataReader) As Cliente
        Dim iMod = r.GetOrdinal("FechaModificacion")
        Return New Cliente With {
            .Id = CInt(r("Id")),
            .Nombres = CStr(r("Nombres")),
            .Apellidos = CStr(r("Apellidos")),
            .DUI = CStr(r("DUI")).Trim(),
            .Email = Db.LeerTexto(r, "Email"),
            .Telefono = Db.LeerTexto(r, "Telefono"),
            .Direccion = Db.LeerTexto(r, "Direccion"),
            .FechaCreacion = CDate(r("FechaCreacion")),
            .FechaModificacion = If(r.IsDBNull(iMod), CType(Nothing, DateTime?), r.GetDateTime(iMod))
        }
    End Function
End Class
