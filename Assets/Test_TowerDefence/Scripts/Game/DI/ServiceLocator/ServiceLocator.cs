using System;
using System.Collections.Generic;


public static class ServiceLocator
{
    private static readonly Dictionary<Type, IService> services = new();

    public static void AddService<T>(T service) where T : IService
    {
        services[typeof(T)] = service;
    }

    public static T GetService<T>() where T : IService
    {
        return (T)services[typeof(T)];
    }

    public static object GetServiceByType(Type argType)
    {
        return services[argType];
    }
}

public interface IService
{

}
