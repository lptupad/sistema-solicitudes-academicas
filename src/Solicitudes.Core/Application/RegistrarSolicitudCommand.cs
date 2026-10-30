using Solicitudes.Core.Domain;

namespace Solicitudes.Core.Application;

public sealed record RegistrarSolicitudCommand(
    string Asunto,
    string Descripcion,
    string Tipo,
    PrioridadSolicitud Prioridad,
    Estudiante Estudiante);
