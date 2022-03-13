using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TeamsPersistanceCenter.Api.Infrastructure.ActionFilters;
using TeamsPersistanceCenter.Managers.Interfaces;
using TeamsPersistanceCenter.Models;
using TeamsPersistanceCenter.Models.Enums;

namespace TeamsPersistanceCenter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminsController : BaseODataApiController<AdminsController>
    {
        private readonly IAdministratorManager _administratorManager;
        private readonly ILogger<AdminsController> _logger;
        public AdminsController(IAdministratorManager administratorManager, ILogger<AdminsController> logger)
        {
            _administratorManager = administratorManager;
            _logger = logger;
        }


        [EnableQueryIfSuccess]
        [ProducesResponseType(typeof(Administrator), StatusCodes.Status200OK)]
        public ActionResult<IQueryable> Get()
        {
            return Ok(_administratorManager.GetAdmins());
        }

        //[EnableQueryIfSuccess]
        //[ProducesResponseType(typeof(Administrator), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public ActionResult<IQueryable> GetAdminByEmail(string email)
        //{
        //    var result = _administratorManager.GetAdminByEmail(email);
        //    return result == null ?
        //        Problem($"Failed to return user list") : Ok(result);
        //}

        [EnableQueryIfSuccess]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IQueryable<Administrator>>> Post([FromBody] Administrator administrator)
        {
            try
            {
                administrator.Id = new Guid().ToString();
                administrator.IsValid = (byte)ValidStatus.Valid;
                var result = await _administratorManager.CreateAdminAsync(administrator);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (DataException ex)
            {
                return Problem($"{ex.Message}", null, StatusCodes.Status400BadRequest);
            }
        }

        [EnableQueryIfSuccess]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IQueryable<Administrator>>> Put([FromODataUri] string email, [FromBody] Administrator administrator)
        {
            try
            {
                if (email != administrator.Email)
                {
                    return Problem($"email is not natching", null, StatusCodes.Status400BadRequest);
                }
                var result = await _administratorManager.UpdateAdminAsync(administrator);
                if (result == null)
                {
                    return Problem($"No administrator has been found", null, StatusCodes.Status404NotFound);
                }
                return StatusCode(StatusCodes.Status202Accepted, result);

            }
            catch (DataException ex)
            {
                return Problem($"{ex.Message}", null, StatusCodes.Status400BadRequest);
            }
        }
    }
}
