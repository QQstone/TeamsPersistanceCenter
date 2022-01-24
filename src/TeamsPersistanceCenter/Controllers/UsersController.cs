using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TeamsPersistanceCenter.Api.Infrastructure.ActionFilters;
using TeamsPersistanceCenter.Managers.Interfaces;
using TeamsPersistanceCenter.Models;

namespace TeamsPersistanceCenter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : BaseODataApiController<UsersController>
    {
        private readonly IUserManager _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserManager userManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [EnableQueryIfSuccess]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public ActionResult<IQueryable> Get()
        {
            return Ok(_userManager.GetUsers());
        }

        [EnableQueryIfSuccess]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IQueryable> Get(string key)
        {
            var result = _userManager.GetUserByCode(key);
            return result == null ?
                Problem($"Failed to return user list") : Ok(result);
        }

        [EnableQueryIfSuccess]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IQueryable<User>>> Post([FromBody] User user)
        {
            try
            {
                var result = await _userManager.CreateUserAsync(user);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (DataException ex)
            {
                return Problem($"{ex.Message}", null, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<ActionResult<IQueryable<User>>> Put([FromODataUri] string code, [FromBody] User user)
        {
            try
            {
                if(code != user.Code)
                {
                    return Problem($"code is not natching", null, StatusCodes.Status400BadRequest);
                }
                var result = await _userManager.UpdateUserAsync(user);
                if(result == null)
                {
                    return Problem($"No user has been found", null, StatusCodes.Status404NotFound);
                }
                return StatusCode(StatusCodes.Status202Accepted, result);

            }catch(DataException ex)
            {
                return Problem($"{ex.Message}", null, StatusCodes.Status400BadRequest);
            }
        }
    }
}
