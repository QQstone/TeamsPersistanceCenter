using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamsPersistanceCenter.Models.Contexts;

namespace TeamsPersistanceCenter.Api.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ApiController]
    public abstract class BaseODataApiController<T> : ControllerBase where T : BaseODataApiController<T>
    {
        private ILogger<T> _logger;
        private TeamsPersistanceContext _dbContext;

        /// <summary>
        /// DB Context
        /// </summary>
        protected TeamsPersistanceContext DbContext
        {
            get
            {
                return _dbContext ??= HttpContext.RequestServices.GetService<TeamsPersistanceContext>();
            }
        }

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger<T> Logger
        { 
            get
            {
                return _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();
            }
        }
    }
}
