using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TestGba.IServices;
using TestGPA.Helper;
using TestGPA.ViewModels;

namespace TestGPA.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationServices _applicationServices;

        public ApplicationController(IApplicationServices applicationServices)
        {
            _applicationServices = applicationServices;
        }

        [HttpGet("GetDepartments")]
        public IActionResult GetDepartments()
        {
            List<string> result = _applicationServices.GetDepartmentNames();

            if (result.Count == 0)
                return NotFound("Departments not found");

            return Ok(result);
        }

        [HttpGet("{dep}/{lev}")]
        public IActionResult GetMaterial(string dep, string lev)
        {
            List<MaterialViewModel> result = _applicationServices.GetAllMaterialInLevel(lev, dep);

            if (result.Count == 0)
                return NotFound("Material not found");

            return Ok(result);
        }

        [HttpPost("{GPA}/{count}")]
        public IActionResult Calcluate([FromBody]IEnumerable<PostData> data, string GPA, int count)
        {
            return Ok(_applicationServices.CalculateGPA(data.ToList(), GPA, count));
        }
    }
}
