using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace  RUINORERP.Common.Helper

{
    public static class AutoMapperHelper
    {
        private static IServiceProvider _serviceProvider;

        public static void UseStateAutoMapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static TDestination Map<TDestination>(object source)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TDestination>(source);
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TSource, TDestination>(source);
        }


        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TDestination>(this object source)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TDestination>(source);
        }
    }
}