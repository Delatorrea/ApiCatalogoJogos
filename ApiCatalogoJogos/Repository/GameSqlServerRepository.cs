using ApiCatalogoJogos.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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
            var sql = $"DELETE FROM Games WHERE Id = @ID";

            await sqlConnection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            command.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
            command.Parameters["@ID"].Value = id;

            await command.ExecuteNonQueryAsync();
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

            string sql = $"SELECT * FROM Games ORDER BY Id OFFSET @Page ROWS FETCH NEXT @Amount ROWS ONLY";

            await sqlConnection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            command.Parameters.Add("@Page", SqlDbType.Int);
            command.Parameters.Add("@Amount", SqlDbType.Int);
            command.Parameters["@Page"].Value = ((page - 1) * amount);
            command.Parameters["@Amount"].Value = amount;

            SqlDataReader sqlDataReader = await command.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Producer = (string)sqlDataReader["Producer"],
                    Price = (double)sqlDataReader["Price"]
                });
            }

            await sqlConnection.CloseAsync();

            return games;

        }

        public async Task<Game> Get(Guid id)
        {
            Game game = null;

            var sql = $"SELECT * FROM Games WHERE Id = @ID";

            await sqlConnection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            command.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
            command.Parameters["@ID"].Value = id;

            SqlDataReader sqlDataReader = await command.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Producer = (string)sqlDataReader["Producer"],
                    Price = (double)sqlDataReader["Price"]
                };
            }

            await sqlConnection.CloseAsync();

            return game;
        }

        public async Task<List<Game>> Get(string name, string producer)
        {
            List<Game> games = new List<Game>();

            string sql = $"SELECT * FROM Games WHERE Name = @Name AND Producer = @Producer";

            await sqlConnection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            command.Parameters.Add("@Name", SqlDbType.VarChar,100);
            command.Parameters.Add("@Producer", SqlDbType.VarChar, 100);
            command.Parameters["@Name"].Value = name;
            command.Parameters["@Producer"].Value = producer;

            SqlDataReader sqlDataReader = await command.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Producer = (string)sqlDataReader["Producer"],
                    Price = (double)sqlDataReader["Price"]
                });
            }
            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task Insert(Game game)
        {
            var sql = $"INSERT Games (Id, Name, Producer, Price) VALUES (@ID, @Name, @Producer, @Price)";

            await sqlConnection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            command.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
            command.Parameters.Add("@Name", SqlDbType.VarChar, 100);
            command.Parameters.Add("@Producer", SqlDbType.VarChar, 100);
            command.Parameters.Add("@Price", SqlDbType.Float);

            command.Parameters["@ID"].Value = game.Id;
            command.Parameters["@Name"].Value = game.Name;
            command.Parameters["@Producer"].Value = game.Producer;
            command.Parameters["@Price"].Value = game.Price;

            await command.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();
        }

        public async Task Update(Game game)
        {
            var sql = $"UPDATE Games SET Id = @ID, Name = @Name, Producer = @Producer, Price = @Price WHERE Id = @ID";

            await sqlConnection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            command.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
            command.Parameters.Add("@Name", SqlDbType.VarChar, 100);
            command.Parameters.Add("@Producer", SqlDbType.VarChar, 100);
            command.Parameters.Add("@Price", SqlDbType.Float);

            command.Parameters["@ID"].Value = game.Id;
            command.Parameters["@Name"].Value = game.Name;
            command.Parameters["@Producer"].Value = game.Producer;
            command.Parameters["@Price"].Value = game.Price;
            await command.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();
        }
    }
}
