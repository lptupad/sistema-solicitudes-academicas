namespace Solicitudes.Core.Domain;

public sealed class Solicitud
{
    public Guid Id { get; }
    public string Asunto { get; }
    public string Descripcion { get; }
    public string Tipo { get; }
    public PrioridadSolicitud Prioridad { get; }
    public EstadoSolicitud Estado { get; private set; }
    public Estudiante Estudiante { get; }
    public Responsable? Responsable { get; private set; }
    public DateTimeOffset CreadaEn { get; }

    public Solicitud(
        Guid id,
        string asunto,
        string descripcion,
        string tipo,
        PrioridadSolicitud prioridad,
        Estudiante estudiante,
        DateTimeOffset creadaEn)
    {
        Id = id;
        Asunto = asunto;
        Descripcion = descripcion;
        Tipo = tipo;
        Prioridad = prioridad;
        Estudiante = estudiante;
        CreadaEn = creadaEn;
        Estado = EstadoSolicitud.Pendiente;
    }

    public void CambiarEstado(EstadoSolicitud nuevoEstado)
    {
        var transicionPermitida = (Estado, nuevoEstado) switch
        {
            (EstadoSolicitud.Pendiente, EstadoSolicitud.EnProceso) => true,
            (EstadoSolicitud.EnProceso, EstadoSolicitud.Resuelta) => true,
            _ => false
        };

        if (!transicionPermitida)
        {
            throw new InvalidOperationException(
                $"No se permite cambiar el estado de {Estado} a {nuevoEstado}.");
        }

        Estado = nuevoEstado;
    }

    public void AsignarA(Responsable responsable)
    {
        ArgumentNullException.ThrowIfNull(responsable);
        Responsable = responsable;
    }
}
