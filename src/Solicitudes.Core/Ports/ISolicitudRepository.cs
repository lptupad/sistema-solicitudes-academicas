using Solicitudes.Core.Domain;

namespace Solicitudes.Core.Ports;

public interface ISolicitudRepository
{
    void Agregar(Solicitud solicitud);
    Solicitud? ObtenerPorId(Guid id);
}
