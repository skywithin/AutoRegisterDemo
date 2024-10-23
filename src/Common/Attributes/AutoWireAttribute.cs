namespace Common.Attributes;

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
public class AutoWireAttribute : Attribute
{
    public Lifetime Lifetime { get; set; }
    public RegisterAs RegisterAs { get; set; }

    public AutoWireAttribute(Lifetime lifetime, RegisterAs registerAs)
    {
        Lifetime = lifetime;
        RegisterAs = registerAs;
    }
}

