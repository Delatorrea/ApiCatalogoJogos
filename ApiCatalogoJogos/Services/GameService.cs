using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Repository;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Delete(Guid id)
        {
            Game game = await _gameRepository.Get(id);

            if (game == null)
            {
                throw new GameNotRegisteredException();
            }

            await _gameRepository.Delete(id);
        }

        public async Task<List<GameViewModel>> Get(int page, int amount)
        {
            List<Game> games = await _gameRepository.Get(page, amount);
            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            }).ToList();
        }

        public async Task<GameViewModel> Get(Guid id)
        {
            Game game = await _gameRepository.Get(id);

            if(game == null)
            {
                return null;
            }

            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task<GameViewModel> Insert(GameInputModel game)
        {
            var entityGame = await _gameRepository.Get(game.Name, game.Producer);

            if (entityGame.Count > 0)
            {
                throw new RegisteredGameException();
            }

            Game gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };

            await _gameRepository.Insert(gameInsert);

            return new GameViewModel
            {
                Id = gameInsert.Id,
                Name = gameInsert.Name,
                Producer = gameInsert.Producer,
                Price = gameInsert.Price
            };
        }

        public async Task Update(Guid id, GameInputModel game)
        {
            Game entityGame = await _gameRepository.Get(id);

            if (entityGame == null)
            {
                throw new GameNotRegisteredException();
            }

            entityGame.Name = game.Name;
            entityGame.Producer = game.Producer;
            entityGame.Price = game.Price;

            await _gameRepository.Update(entityGame);
        }

        public async Task Update(Guid id, double price)
        {
            Game entityGame = await _gameRepository.Get(id);

            if (entityGame == null)
            {
                throw new GameNotRegisteredException();
            }

            entityGame.Price = price;

            await _gameRepository.Update(entityGame);
        }

        public void Dispose()
        {
            _gameRepository?.Dispose();
        }
    }
}
