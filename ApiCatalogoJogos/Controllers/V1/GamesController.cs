using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Object>>> Get()
        {
            return Ok();
        }

        [HttpGet("{idGame:guid}")]
        public async Task<ActionResult<Object>> Get(Guid idGame)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Object>> Insert(Object game)
        {
            return Ok();
        }

        [HttpPut("{idGame:guid}")]
        public async Task<ActionResult> Update(Guid idGame, Object game)
        {
            return Ok();
        }

        [HttpPatch("{idGame:guid}/price/{price:double}")]
        public async Task<ActionResult> Update(Guid idGame, Double price)
        {
            return Ok();
        }

        [HttpDelete("{idGame:guid}")]
        public async Task<ActionResult> Delete(Guid idGame)
        {
            return Ok();
        }
    }
}
