using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;

namespace TeamsPersistanceCenter.Api.Infrastructure.OData.Configuration
{
    /// <summary>
    /// Base class for ODAta Entity configuration
    /// </summary>
    public static class EntitySetConfigurationBase<TEntity, TController>
        where TEntity : class
        where TController : ODataController
    {
        // Get entity set name based on Controller
        private static string GetEntitySetName()
        {
            // Try to get the entity set name from the controller name
            const string controllerNameSuffix = "Controller";
            var configurationName = typeof(TController).Name;
            Debug.Assert(configurationName.EndsWith(controllerNameSuffix));
            var entitySetName = configurationName.Substring(0, configurationName.Length - controllerNameSuffix.Length);
            return entitySetName;
        }


        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            var entity = builder.EntitySet<TEntity>(GetEntitySetName()).EntityType;
            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }
    }

}
