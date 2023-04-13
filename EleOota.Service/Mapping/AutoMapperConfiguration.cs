using AutoMapper;

namespace EleOota.Service.Mapping
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Intialize()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddMaps(typeof(EntitiesProfile));
            });
            return configuration;
        }
    }
}
