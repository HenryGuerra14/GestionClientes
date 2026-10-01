Imports System.Security.Cryptography

Public NotInheritable Class PasswordHasher
    Private Const SaltSize As Integer = 16
    Private Const HashSize As Integer = 32
    Private Const Iterations As Integer = 100000

    Private Sub New()
    End Sub

    Public Shared Function GenerarSalt() As Byte()
        Dim salt(SaltSize - 1) As Byte
        Using rng = RandomNumberGenerator.Create()
            rng.GetBytes(salt)
        End Using
        Return salt
    End Function

    Public Shared Function Hash(password As String, salt As Byte()) As Byte()
        Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256)
            Return pbkdf2.GetBytes(HashSize)
        End Using
    End Function

    Public Shared Function Verificar(password As String, salt As Byte(), hashEsperado As Byte()) As Boolean
        If String.IsNullOrEmpty(password) OrElse salt Is Nothing OrElse hashEsperado Is Nothing Then Return False
        Return ComparacionTiempoConstante(Hash(password, salt), hashEsperado)
    End Function

    ' Evita ataques de temporización
    Private Shared Function ComparacionTiempoConstante(a As Byte(), b As Byte()) As Boolean
        If a.Length <> b.Length Then Return False
        Dim diff As Integer = 0
        For i = 0 To a.Length - 1
            diff = diff Or (CInt(a(i)) Xor CInt(b(i)))
        Next
        Return diff = 0
    End Function
End Class
