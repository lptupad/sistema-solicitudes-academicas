using Solicitudes.Core.Domain;
using Solicitudes.Core.Ports;

namespace Solicitudes.Core.Application;

public sealed class SolicitudService(
    ISolicitudRepository repository,
    IClock clock)
{
    public Solicitud Registrar(RegistrarSolicitudCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Validar(command);

        var solicitud = new Solicitud(
            Guid.NewGuid(),
            command.Asunto.Trim(),
            command.Descripcion.Trim(),
            command.Tipo.Trim(),
            command.Prioridad,
            command.Estudiante,
            clock.UtcNow);

        repository.Agregar(solicitud);
        return solicitud;
    }

    public void CambiarEstado(Guid solicitudId, EstadoSolicitud nuevoEstado)
    {
        var solicitud = ObtenerSolicitud(solicitudId);
        solicitud.CambiarEstado(nuevoEstado);
    }

    public void AsignarResponsable(Guid solicitudId, Responsable responsable)
    {
        ArgumentNullException.ThrowIfNull(responsable);
        var solicitud = ObtenerSolicitud(solicitudId);
        solicitud.AsignarA(responsable);
    }

    private Solicitud ObtenerSolicitud(Guid id) =>
        repository.ObtenerPorId(id)
        ?? throw new KeyNotFoundException($"No existe la solicitud {id}.");

    private static void Validar(RegistrarSolicitudCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Asunto))
        {
            throw new ArgumentException("El asunto es obligatorio.", nameof(command));
        }

        if (string.IsNullOrWhiteSpace(command.Descripcion))
        {
            throw new ArgumentException("La descripción es obligatoria.", nameof(command));
        }

        if (string.IsNullOrWhiteSpace(command.Tipo))
        {
            throw new ArgumentException("El tipo es obligatorio.", nameof(command));
        }

        ArgumentNullException.ThrowIfNull(command.Estudiante);
    }
}
