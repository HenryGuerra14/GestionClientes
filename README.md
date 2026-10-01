# Gestión de Clientes — Prueba Técnica

Aplicación web en **ASP.NET Web Forms (VB.NET)** con **SQL Server** para gestionar clientes, con login seguro y bitácora de acciones.

## Funcionalidades

- **Login** validado contra la tabla `Usuarios` de SQL Server (Forms Authentication).
- **Mantenimiento de clientes**: ver, buscar, agregar, editar y eliminar.
- **Bitácora**: cada alta, edición o eliminación queda registrada en la tabla `Bitacora` con la acción, el ID del cliente, la fecha y hora y el usuario que hizo el cambio. En las ediciones se guardan también los valores de antes y después.

## Tecnologías

- ASP.NET Web Forms, VB.NET, .NET Framework 4.8
- SQL Server (LocalDB, Express o superior), ADO.NET
- Bootstrap 5 (CDN)

## Requisitos

- Visual Studio 2022 con la carga de trabajo **Desarrollo de ASP.NET y web** (incluye .NET Framework 4.8 y LocalDB)
- SQL Server Management Studio (SSMS)

## Instalación

1. **Base de datos:** abre `Database/01_CrearBaseDatos.sql` en SSMS, conectado a `(localdb)\MSSQLLocalDB`, y ejecútalo con F5. Crea la base de datos `GestionClientesDB`, las tablas `Usuarios`, `Clientes` y `Bitacora`, y datos de ejemplo.
2. **Cadena de conexión:** está en `GestionClientes/Web.config`, con el nombre `GestionClientesDB`:
   ```xml
   Server=(localdb)\MSSQLLocalDB;Database=GestionClientesDB;Integrated Security=True;
   ```
   Si usas SQL Server Express, cambia `Server=` por `.\SQLEXPRESS`.
3. Abre `GestionClientes.sln` en Visual Studio y presiona **F5**.

### Credenciales de prueba

| Usuario | Contraseña |
|---|---|
| `admin` | `Admin123!` |

## Estructura

```
Database/            Script SQL de creación
GestionClientes/
  Models/            Entidades (Cliente, Usuario, BitacoraRegistro)
  Data/              Acceso a datos con ADO.NET (repositorios)
  Security/          Hashing de contraseñas (PBKDF2)
  Site.Master        Plantilla con menú de navegación
  Login.aspx         Pantalla de login
  Clientes.aspx      Pantalla principal (CRUD de clientes)
  Bitacora.aspx      Consulta de la bitácora
  Error.aspx         Página de error genérica
  Web.config         Configuración y cadena de conexión
```

## Seguridad implementada

- **Contraseñas con hashing**: PBKDF2-HMAC-SHA256 con 100,000 iteraciones y salt aleatorio de 16 bytes por usuario. La comparación es de tiempo constante.
- **Inyección SQL**: todas las consultas son parametrizadas, con tipos de dato explícitos. No se concatenan valores del usuario en el SQL.
- **Autenticación**: Forms Authentication. Las páginas exigen sesión (`<deny users="?" />`).
- **XSS**: los datos se muestran codificados (HtmlEncode).
- **CSRF**: el `ViewStateUserKey` se liga al usuario autenticado.
- **Cookies** HttpOnly y cabeceras `X-Frame-Options` y `X-Content-Type-Options`.
- **Errores**: mensajes genéricos al usuario. El detalle técnico se registra internamente y nunca se muestra.
- **Integridad de la bitácora**: el cambio en el cliente y su registro en la bitácora se guardan en una misma transacción. Se guardan los dos o ninguno.

## Validaciones

- Nombres, apellidos y DUI son obligatorios.
- El DUI debe tener el formato `00000000-0` y no puede repetirse.
- El email y el teléfono deben tener un formato válido.
- Las validaciones se ejecutan en el navegador y también en el servidor.
