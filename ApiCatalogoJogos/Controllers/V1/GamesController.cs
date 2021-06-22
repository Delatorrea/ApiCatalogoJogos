using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="page">Indica qual página está sendo consultada. Mínimo 1</param>
        /// <param name="amount">Indica a quantidade de registros por página. Mínimo 1 e máximo 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não haja jogos"</response>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameViewModel>>> Get(
            [FromQuery, Range(1, int.MaxValue)] int page = 1, [FromQuery, Range(1,50)] int amount = 5)
        {
            List<GameViewModel> games = await _gameService.Get(page, amount);

            if (games.Count == 0)
            {
                return NoContent();
            }

            return Ok(games);
        }

        /// <summary>
        /// Buscar um jogo pelo seu Id
        /// </summary>
        /// <param name="idGame">Id do Jogo buscado</param>
        /// <response code="200">Retorna o jogo filtrado</response>
        /// <response code="204">Caso não haja o jogo"</response>
        /// <returns></returns>
        [HttpGet("{idGame:guid}")]
        public async Task<ActionResult<GameViewModel>> Get([FromRoute] Guid idGame)
        {
            GameViewModel game = await _gameService.Get(idGame);
            
            if (game == null)
            {
                return NoContent();
            }

            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<GameViewModel>> Insert([FromBody] GameInputModel gameInputModel)
        {
            try
            {
                GameViewModel game = await _gameService.Insert(gameInputModel);
                return Ok(game);
                
            }
            catch (RegisteredGameException ex)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }
        }

        [HttpPut("{idGame:guid}")]
        public async Task<ActionResult> Update([FromRoute] Guid idGame, [FromBody] GameInputModel gameInputModel)
        {
            try
            {
                await _gameService.Update(idGame, gameInputModel);
                return Ok();
            }
            catch (GameNotRegisteredException ex)
            {
                return NotFound("Jogo não existe");
            }
        }

        [HttpPatch("{idGame:guid}/price/{price:double}")]
        public async Task<ActionResult> Update([FromRoute] Guid idGame, [FromRoute] Double price)
        {
            try
            {
                await _gameService.Update(idGame, price);
                return Ok();
            }
            catch (GameNotRegisteredException ex)
            {
                return NotFound("Jogo não existe");
            }
        }

        [HttpDelete("{idGame:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid idGame)
        {
            try
            {
                await _gameService.Delete(idGame);
                return Ok();
            }
            catch (GameNotRegisteredException ex)
            {
                return NotFound("Jogo não existe");
            }
        }
    }
}
