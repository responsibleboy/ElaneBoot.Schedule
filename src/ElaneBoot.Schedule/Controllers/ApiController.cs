using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WangSql;

namespace ElaneBoot.Schedule.Controllers
{
    [Route("/Schedule/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class ApiController : ControllerBase
    {
    }
}
