using Solicitudes.Core.Domain;
using Solicitudes.Core.Ports;

namespace Solicitudes.Core.Infrastructure;

public sealed class InMemorySolicitudRepository :
    ISolicitudRepository,
    ISolicitudReadRepository
{
    private readonly Dictionary<Guid, Solicitud> _solicitudes = [];

    public void Agregar(Solicitud solicitud)
    {
        ArgumentNullException.ThrowIfNull(solicitud);
        _solicitudes.Add(solicitud.Id, solicitud);
    }

    public Solicitud? ObtenerPorId(Guid id) =>
        _solicitudes.GetValueOrDefault(id);

    public IReadOnlyCollection<Solicitud> Listar() =>
        _solicitudes.Values.ToArray();
}
