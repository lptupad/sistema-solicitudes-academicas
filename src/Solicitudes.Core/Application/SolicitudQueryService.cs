using Solicitudes.Core.Domain;
using Solicitudes.Core.Ports;

namespace Solicitudes.Core.Application;

public sealed class SolicitudQueryService(ISolicitudReadRepository repository)
{
    public IReadOnlyCollection<Solicitud> Buscar(SolicitudCriteria criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);

        IEnumerable<Solicitud> resultado = repository.Listar();

        if (!string.IsNullOrWhiteSpace(criteria.Texto))
        {
            var texto = criteria.Texto.Trim();
            resultado = resultado.Where(s =>
                s.Asunto.Contains(texto, StringComparison.OrdinalIgnoreCase)
                || s.Descripcion.Contains(texto, StringComparison.OrdinalIgnoreCase));
        }

        if (criteria.Estado is not null)
        {
            resultado = resultado.Where(s => s.Estado == criteria.Estado);
        }

        if (criteria.Prioridad is not null)
        {
            resultado = resultado.Where(s => s.Prioridad == criteria.Prioridad);
        }

        if (criteria.ResponsableId is not null)
        {
            resultado = resultado.Where(s => s.Responsable?.Id == criteria.ResponsableId);
        }

        return resultado
            .OrderByDescending(s => s.CreadaEn)
            .ToArray();
    }

    public IReadOnlyCollection<SolicitudResumen> GenerarReporte()
    {
        return repository.Listar()
            .GroupBy(s => s.Estado)
            .OrderBy(group => group.Key)
            .Select(group => new SolicitudResumen(group.Key, group.Count()))
            .ToArray();
    }
}
