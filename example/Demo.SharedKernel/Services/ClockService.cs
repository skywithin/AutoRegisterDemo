using AutoRegister.DI;

namespace Demo.SharedKernel.Services;

public interface IClockService
{
    DateTime UtcNow { get; }
}

[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
internal sealed class ClockService : IClockService
{
    public DateTime UtcNow => DateTime.UtcNow;
}