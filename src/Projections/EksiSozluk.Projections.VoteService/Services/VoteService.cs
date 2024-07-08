using EksiSozluk.Common.Events.Entry;
using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using EksiSozluk.Common.ViewModels;

namespace EksiSozluk.Projections.VoteService.Services
{
    public class VoteService
    {
        private readonly string connectionString;

        public VoteService(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task CreateEntryVote(CreateEntryVoteEvent @event)
        {
            await DeleteEntryVote(@event.EntryId, @event.CreatedBy); // eski oyu siliyoruz  
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("INSERT INTO EntryVote (Id,EntryId, VoteType,CreatedById,CreateDate) VALUES(@Id, @EntryId, @VoteType,@CreatedById, GETDATE())",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryId = @event.EntryId,
                    VoteType = (int)@event.VoteType,
                    CreatedById = @event.CreatedBy      
                });
        }

        public async Task DeleteEntryVote(Guid entryId, Guid userId)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE EntryVote WHERE EntryId=@EntryId AND CREATEDBYID =@UserId",
                new
                {
                    EntryId = entryId,
                    UserId = userId
                });
        }

    }
}
