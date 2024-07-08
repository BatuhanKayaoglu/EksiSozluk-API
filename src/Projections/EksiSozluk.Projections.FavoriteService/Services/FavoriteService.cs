using EksiSozluk.Common.Events.Entry;
using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;

namespace EksiSozluk.Projections.FavoriteService.Services
{
    public class FavoriteService
    {
        private readonly string connectionString;

        public FavoriteService(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task CreateEntryFav(CreateEntryFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("INSERT INTO EntryFavorite (Id,EntryId, CreatedById, CreateDate) VALUES(@Id, @EntryId, @CreatedById, GETDATE())",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryId = @event.EntryId,
                    CreatedById = @event.CreatedBy
                    //CreatedById = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                });
        }   
    }
}
