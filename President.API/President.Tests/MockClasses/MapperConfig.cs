using AutoMapper;
using AutoMapper.Configuration;
using President.BLL.Mapping;

namespace President.Tests.MockClasses
{
    public static class MapperConfig
    {
        private readonly static object Lock = new object();
        private static string mapper;

        public static Mapper MapperInstance
        {
            get
            {
                if (mapper == null)
                {
                    lock (Lock)
                    {
                        if (mapper == null)
                        {
                            var mappings = new MapperConfigurationExpression();
                            mappings.AddProfile<AutoMapperProfile>();
                            Mapper.Initialize(mappings);
                            mapper = "ready";
                        }
                    }
                }
                return (Mapper)Mapper.Instance;
            }
        }
    }
}
