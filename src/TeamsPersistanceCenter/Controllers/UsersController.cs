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

        /// <summary>
        /// Update User with specified code.
        /// </summary>
        /// <param name="key">The specified code</param>
        /// <param name="user">User detail</param>
        /// <returns>The newly updated member.</returns>
        [EnableQueryIfSuccess]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpPut]
        public async Task<ActionResult<IQueryable<User>>> Put([FromODataUri] string key, [FromBody] User user)
        {
            try
            {
                if(key != user.Code)
                {
                    return Problem($"code is not matching", null, StatusCodes.Status400BadRequest);
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

        [EnableQueryIfSuccess]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpDelete]
        public async Task<ActionResult<IQueryable<User>>> Delete([FromODataUri] string key)
        {
            var user = DbContext.Users.Where(u => u.Code == key).FirstOrDefault();
            if (user == null)
            {
                return Problem($"Failed to find a user with code : {key}", "User code", StatusCodes.Status404NotFound);
            }
            var res = await _userManager.DeactiveUserAsync(key);
            return StatusCode(StatusCodes.Status202Accepted, res);
        }
    }
}
