using MapsterMapper;

using NetArch.Template.Infrastructure.Abstractions.Mapping;

namespace NetArch.Template.Infrastructure.Mapping;

public class MapsterObjectMapper : IObjectMapper
{
    private readonly IMapper _mapper;

    public MapsterObjectMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TSource, TDestination>(TSource source)
        => _mapper.Map<TSource, TDestination>(source);

    public TDestination Map<TDestination>(object source)
        => _mapper.Map<TDestination>(source);

    public void Map<TSource, TDestination>(TSource source, TDestination destination)
        => _mapper.Map(source, destination);
}
