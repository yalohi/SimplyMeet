using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimplyMeetApi.Configuration
{
	public static class ConfigurationHelper
	{
		//===========================================================================================
		// Public Static Methods
		//===========================================================================================
		public static T Configure<T>(IConfiguration InConfiguration, IServiceCollection InServices) where T : class
		{
			var ConfigurationSection = InConfiguration.GetSection(typeof(T).Name);
			InServices.Configure<T>(ConfigurationSection, InBinderOptions => InBinderOptions.BindNonPublicProperties = true);
			var ConfigurationInstance = Activator.CreateInstance<T>();
			ConfigurationSection.Bind(ConfigurationInstance, InBinderOptions => InBinderOptions.BindNonPublicProperties = true);

			return ConfigurationInstance;
		}
	}
}