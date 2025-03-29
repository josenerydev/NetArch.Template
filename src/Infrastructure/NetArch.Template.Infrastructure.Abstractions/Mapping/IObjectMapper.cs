namespace NetArch.Template.Infrastructure.Abstractions.Mapping;

public interface IObjectMapper
{
    TDestination Map<TSource, TDestination>(TSource source);
    TDestination Map<TDestination>(object source);
    void Map<TSource, TDestination>(TSource source, TDestination destination);
}

