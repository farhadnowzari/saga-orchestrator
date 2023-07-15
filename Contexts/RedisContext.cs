using StackExchange.Redis;

namespace SagaOrchestrator.Contexts;

public class RedisContext
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisContext(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    protected IDatabase Database => _connectionMultiplexer.GetDatabase();

    public long UpdateSet(string key, RedisValue value)
    {
        var transaction = Database.CreateTransaction();
        transaction.SetAddAsync(key, value);
        var setLength = transaction.SetLengthAsync(key);
        transaction.Execute(CommandFlags.DemandMaster);
        return setLength.Result;
    }

    public void Pop(string key, RedisValue value)
    {
        Database.SetRemove(key, value);
    }
    public RedisValue? First(string key)
    {
        var members = Database.SetMembers(key);
        return members?.FirstOrDefault();
    }
}

public static class DI
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = ConnectionMultiplexer.Connect(configuration.GetConnectionString("redis"));
        services.AddSingleton<IConnectionMultiplexer>(connection);
        services.AddScoped<RedisContext>();
        return services;
    }
}