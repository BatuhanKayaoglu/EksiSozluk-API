using EksiSozluk.Api.Application.Repositories;
using EksİSozluk.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Infrastructure.Persistance.Repositories
{
    public class EntryCommentRepository : GenericRepository<EntryComment>, IEntryCommentRepository
    {
        public EntryCommentRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
