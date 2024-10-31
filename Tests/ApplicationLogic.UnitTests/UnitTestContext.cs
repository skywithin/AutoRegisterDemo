using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;

namespace ApplicationLogic.UnitTests;

[TestFixture]
public abstract class UnitTestContext<T> where T : class
{
    protected Fixture Fixture { get; private set; }
    private Dictionary<Type, Mock> _injectedMocks;
    private Dictionary<Type, object> _injectedConcreteClasses;
    private T? _sut;

    public T Sut
    {
        get
        {
            return _sut ??= Fixture.Create<T>();
        }
    }

    protected virtual bool ConfigureMembers => false;

    [SetUp]
    public void BaseSetup()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        Fixture.Customize(new AutoMoqCustomization { ConfigureMembers = ConfigureMembers });

        _injectedMocks = new Dictionary<Type, Mock>();
        _injectedConcreteClasses = new Dictionary<Type, object>();

        SetUp();

        // When ConfigureMembers flag is true, we don't need to
        // manually scan constructor and inject all dependencies
        if (ConfigureMembers)
            return;

        // Inject the rest of constructor params that have not been injected during SetUp
        var type = typeof(T);
        var constructors = type.GetConstructors();
        var method = typeof(UnitTestContext<T>).GetMethod(nameof(GetMockFor));

        foreach (var contructor in constructors)
        {
            var parameters = contructor.GetParameters();
            foreach (var parameter in parameters)
            {
                var parameterType = parameter.ParameterType;
                if (!_injectedConcreteClasses.ContainsKey(parameterType) && (parameterType.IsInterface || parameterType.IsAbstract))
                {
                    var genericMethod = method!.MakeGenericMethod(parameterType);
                    genericMethod.Invoke(obj: this, null);
                }
            }
        }
    }

    public virtual void SetUp() { }

    /// <summary>
    /// Generates a mock for a class and injects it into the final fixture
    /// </summary>
    /// <typeparam name="TMockType"></typeparam>
    /// <returns></returns>
    public Mock<TMockType> GetMockFor<TMockType>() where TMockType : class
    {
        var mockType = typeof(TMockType);
        var existingMock = _injectedMocks.FirstOrDefault(x => x.Key == mockType);
        if (existingMock.Key == null)
        {
            var newMock = new Mock<TMockType>();
            _injectedMocks.Add(mockType, newMock);
            Fixture.Inject(newMock);
            return newMock;
        }

        return (Mock<TMockType>)existingMock.Value;
    }

    /// <summary>
    /// Injects a concrete class to be used when generating the fixture. 
    /// </summary>
    /// <typeparam name="TClassType"></typeparam>
    /// <returns></returns>
    public void InjectMock<TClassType>(TClassType injectedClass) where TClassType : class
    {
        var classType = typeof(TClassType);
        var existingClass = _injectedConcreteClasses.FirstOrDefault(x => x.Key == classType);
        if (existingClass.Key != null)
        {
            throw new ArgumentException($"{injectedClass.GetType().Name} has been injected more than once");
        }
        _injectedConcreteClasses.Add(classType, injectedClass);
        Fixture.Inject(injectedClass);
    }

    public TClassType? GetInjectedMock<TClassType>() where TClassType : class
    {
        var classType = typeof(TClassType);

        return _injectedConcreteClasses[classType] as TClassType;
    }

    [TearDown]
    public void TearDown()
    {
        Fixture = null;
        _sut = null;
        _injectedMocks = new Dictionary<Type, Mock>();
        _injectedConcreteClasses = new Dictionary<Type, object>();
    }

    public Mock<T> CreateSutMock()
    {
        return Fixture.Create<Mock<T>>();
    }
}