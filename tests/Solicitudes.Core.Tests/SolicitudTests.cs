using Solicitudes.Core.Application;
using Solicitudes.Core.Domain;
using Solicitudes.Core.Infrastructure;
using Solicitudes.Core.Ports;
using Xunit;

namespace Solicitudes.Core.Tests;

public sealed class SolicitudTests
{
    private static readonly DateTimeOffset FechaFija =
        new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Registrar_CreaSolicitudPendienteConDatosNormalizados()
    {
        var repository = new InMemorySolicitudRepository();
        var service = new SolicitudService(repository, new FixedClock(FechaFija));

        var solicitud = service.Registrar(Comando(asunto: "  Certificado  "));

        Assert.NotEqual(Guid.Empty, solicitud.Id);
        Assert.Equal("Certificado", solicitud.Asunto);
        Assert.Equal(EstadoSolicitud.Pendiente, solicitud.Estado);
        Assert.Equal(FechaFija, solicitud.CreadaEn);
        Assert.Same(solicitud, repository.ObtenerPorId(solicitud.Id));
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Registrar_RechazaAsuntoVacio(string asunto)
    {
        var service = new SolicitudService(
            new InMemorySolicitudRepository(),
            new FixedClock(FechaFija));

        Assert.Throws<ArgumentException>(() => service.Registrar(Comando(asunto)));
    }

    [Fact]
    public void CambiarEstado_PermiteTransicionPendienteAEnProceso()
    {
        var (service, solicitud) = CrearServicioConSolicitud();

        service.CambiarEstado(solicitud.Id, EstadoSolicitud.EnProceso);

        Assert.Equal(EstadoSolicitud.EnProceso, solicitud.Estado);
    }

    [Fact]
    public void CambiarEstado_RechazaSaltoDePendienteAResuelta()
    {
        var (service, solicitud) = CrearServicioConSolicitud();

        Assert.Throws<InvalidOperationException>(
            () => service.CambiarEstado(solicitud.Id, EstadoSolicitud.Resuelta));
        Assert.Equal(EstadoSolicitud.Pendiente, solicitud.Estado);
    }

    [Fact]
    public void CambiarEstado_RechazaSolicitudInexistente()
    {
        var service = new SolicitudService(
            new InMemorySolicitudRepository(),
            new FixedClock(FechaFija));

        Assert.Throws<KeyNotFoundException>(
            () => service.CambiarEstado(Guid.NewGuid(), EstadoSolicitud.EnProceso));
    }

    [Fact]
    public void AsignarResponsable_AsociaAlResponsableIndicado()
    {
        var (service, solicitud) = CrearServicioConSolicitud();
        var responsable = new Responsable(Guid.NewGuid(), "Ana", "Alumnado");

        service.AsignarResponsable(solicitud.Id, responsable);

        Assert.Equal(responsable, solicitud.Responsable);
    }

    [Fact]
    public void Buscar_AplicaFiltrosDeTextoYEstado()
    {
        var repository = new InMemorySolicitudRepository();
        var service = new SolicitudService(repository, new FixedClock(FechaFija));
        var primera = service.Registrar(Comando("Certificado analítico"));
        service.Registrar(Comando("Consulta de inscripción"));
        primera.CambiarEstado(EstadoSolicitud.EnProceso);
        var queryService = new SolicitudQueryService(repository);

        var resultado = queryService.Buscar(new SolicitudCriteria(
            Texto: "CERTIFICADO",
            Estado: EstadoSolicitud.EnProceso));

        Assert.Single(resultado);
        Assert.Equal(primera.Id, resultado.Single().Id);
    }

    [Fact]
    public void GenerarReporte_CuentaSolicitudesPorEstado()
    {
        var repository = new InMemorySolicitudRepository();
        var service = new SolicitudService(repository, new FixedClock(FechaFija));
        service.Registrar(Comando("Solicitud 1"));
        service.Registrar(Comando("Solicitud 2"));
        var tercera = service.Registrar(Comando("Solicitud 3"));
        tercera.CambiarEstado(EstadoSolicitud.EnProceso);
        var queryService = new SolicitudQueryService(repository);

        var reporte = queryService.GenerarReporte();

        Assert.Contains(reporte, item =>
            item.Estado == EstadoSolicitud.Pendiente && item.Cantidad == 2);
        Assert.Contains(reporte, item =>
            item.Estado == EstadoSolicitud.EnProceso && item.Cantidad == 1);
    }

    private static RegistrarSolicitudCommand Comando(string asunto) =>
        new(
            asunto,
            "Descripción de prueba",
            "Académica",
            PrioridadSolicitud.Media,
            new Estudiante(Guid.NewGuid(), "Estudiante de prueba", "alumno@example.test"));

    private static (SolicitudService Service, Solicitud Solicitud)
        CrearServicioConSolicitud()
    {
        var service = new SolicitudService(
            new InMemorySolicitudRepository(),
            new FixedClock(FechaFija));
        return (service, service.Registrar(Comando("Trámite")));
    }

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }
}
