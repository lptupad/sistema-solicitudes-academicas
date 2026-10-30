namespace Solicitudes.Core.Ports;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
