namespace Common.Bootstrap;

public enum Lifetime
{
    Scoped,
    Transient,
    Singleton
}

[Flags]
public enum RegisterAs
{
    Interface = 1,
    Self = 2
}

/// <summary>
/// Attribute to flag classes for auto-registration
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterAttribute : Attribute
{
    public Lifetime Lifetime { get; set; }
    public RegisterAs RegisterAs { get; set; }

    public AutoRegisterAttribute(Lifetime lifetime, RegisterAs registerAs)
    {
        Lifetime = lifetime;
        RegisterAs = registerAs;
    }
}

