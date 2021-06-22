using ApiCatalogoJogos.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repository
{
    public class GameSqlServerRepository : IGameRepository
    {
        private readonly SqlConnection sqlConnection;

        public GameSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task Delete(Guid id)
        {
            var command = $"DELETE FROM Games WHERE Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }

        public async Task<List<Game>> Get(int page, int amount)
        {
            List<Game> games = new List<Game>();

            string command = $"SELECT * FROM Games ORDER BY Id OFFSET {((page - 1) * amount)} ROWS FETCH NEXT {amount} ROWS ONLY";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Producer = (string)sqlDataReader["Producer"],
                    Price = (double)sqlDataReader["Price"]
                });

                await sqlConnection.CloseAsync();
            }

            return games;

        }

        public async Task<Game> Get(Guid id)
        {
            Game game = null;

            var command = $"SELECT * FROM Games WHERE Id {id}";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Producer = (string)sqlDataReader["Producer"],
                    Price = (double)sqlDataReader["Price"]
                };

                await sqlConnection.CloseAsync();
            }
            return game;
        }

        public async Task<List<Game>> Get(string name, string producer)
        {
            List<Game> games = new List<Game>();

            string command = $"SELECT * FROM Games WHERE Name = '{name}' AND Producer '{producer}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Producer = (string)sqlDataReader["Producer"],
                    Price = (double)sqlDataReader["Price"]
                });

                await sqlConnection.CloseAsync();
            }

            return games;
        }

        public async Task Insert(Game game)
        {
            var command = $"INSERT Games (Id, Name, Producer, Price) VALUES ('{game.Id}', '{game.Name}', '{game.Producer}', '{game.Price.ToString().Replace(",", ".")}')";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();
        }

        public async Task Update(Game game)
        {
            var command = $"UPDATE Games SET Id = '{game.Id}', Name = '{game.Name}', Producer = '{game.Producer}', Price = '{game.Price.ToString().Replace(",", ".")}' WHERE Id = '{game.Id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();
        }
    }
}
