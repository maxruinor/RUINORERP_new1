using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

public class ModuleInfo {

    //public IServiceCollection AddDistributedMemoryCache(this IServiceCollection services)
    //{
    //    if (services == null)
    //    {
    //        throw new ArgumentNullException(nameof(services));
    //    }
    //    services.AddOptions();
    //    services.TryAdd(ServiceDescriptor.Singleton<IDistributedCache, MemoryDistributedCache>());
    //    return services;
    //}


    public string Name { get; set; }

	public Assembly Assembly { get; set; }
 
	public string ShortName {
		get {
			return Name.Split('.').Last();
		}
	}

	public string Path { get; set; }
}