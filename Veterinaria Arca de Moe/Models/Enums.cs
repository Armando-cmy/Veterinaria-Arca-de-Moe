namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>Estados posibles de una cita veterinaria.</summary>
    public enum EstadoCita
    {
        Pendiente = 0,
        Confirmada = 1,
        EnAtencion = 2,
        Completada = 3,
        Cancelada = 4,
        NoAsistio = 5
    }

    /// <summary>Estado de actividad de entidades (Propietario, Mascota, Veterinario).</summary>
    public enum EstadoRegistro
    {
        Activo = 0,
        Inactivo = 1,
        Eliminado = 2   // Soft-delete: el registro no se borra físicamente
    }

    /// <summary>Sexo biológico de la mascota.</summary>
    public enum Sexo
    {
        Macho = 0,
        Hembra = 1,
        Desconocido = 2
    }

    /// <summary>Rol de un usuario del sistema.</summary>
    public enum RolUsuario
    {
        Administrador = 0,
        Veterinario = 1,
        Recepcionista = 2
    }

    /// <summary>Tipo de movimiento de inventario.</summary>
    public enum TipoMovimiento
    {
        Entrada = 0,
        Salida = 1,
        AjustePositivo = 2,
        AjusteNegativo = 3
    }
}
