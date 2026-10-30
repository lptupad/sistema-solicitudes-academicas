using Solicitudes.Core.Domain;

namespace Solicitudes.Core.Ports;

public interface ISolicitudReadRepository
{
    IReadOnlyCollection<Solicitud> Listar();
}
