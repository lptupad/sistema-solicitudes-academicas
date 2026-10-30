using Solicitudes.Core.Domain;

namespace Solicitudes.Core.Application;

public sealed record SolicitudCriteria(
    string? Texto = null,
    EstadoSolicitud? Estado = null,
    PrioridadSolicitud? Prioridad = null,
    Guid? ResponsableId = null);
