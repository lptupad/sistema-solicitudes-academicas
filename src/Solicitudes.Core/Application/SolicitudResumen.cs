using Solicitudes.Core.Domain;

namespace Solicitudes.Core.Application;

public sealed record SolicitudResumen(
    EstadoSolicitud Estado,
    int Cantidad);
