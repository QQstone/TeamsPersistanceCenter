using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TeamsPersistanceCenter.Api.Infrastructure.Helpers
{
    public static class JsonHelpers
    {
        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings =
            new JsonSerializerSettings().ConfigureDefaultJsonSerializerSettings();

        public static JsonSerializerSettings ConfigureDefaultJsonSerializerSettings(this JsonSerializerSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return settings;
        }

    }
}
