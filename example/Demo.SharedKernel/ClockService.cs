using AutoRegister.DI;

namespace Demo.SharedKernel;

public interface IClockService
{
    DateTime UtcNow { get; }
}

[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
public class ClockService : IClockService
{
    public virtual DateTime UtcNow => DateTime.UtcNow;
}