using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
    public class AssignNumberController : BaseODataApiController<AssignNumberController>
    {
        private readonly IAssignNumberManager _assignNumberManager;
        private readonly ILogger<AssignNumberController> _logger;

        public AssignNumberController(IAssignNumberManager assignNumberManager, ILogger<AssignNumberController> logger)
        {
            _assignNumberManager = assignNumberManager;
            _logger = logger;
        }

        [EnableQueryIfSuccess]
        [ProducesResponseType(typeof(AssignNumber), StatusCodes.Status200OK)]
        public ActionResult<IQueryable> Get()
        {
            return Ok(_assignNumberManager.GetAssignNumbers());
        }

        [EnableQueryIfSuccess]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AssignNumber>> Post([FromBody] AssignNumber assignNumber)
        {
            try
            {
                var result = await _assignNumberManager.CreateAssignNumberAsync(assignNumber.Num);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (DataException ex)
            {
                return Problem($"{ex.Message}", null, StatusCodes.Status400BadRequest);
            }
        }
    }
}
