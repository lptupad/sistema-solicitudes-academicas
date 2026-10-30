using Solicitudes.Core.Ports;

namespace Solicitudes.Core.Infrastructure;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
