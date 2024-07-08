using EksiSozluk.Common;
using EksiSozluk.Common.Events.Entry;
using EksiSozluk.Common.Infrastructure;

namespace EksiSozluk.Projections.FavoriteService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connStr = configuration.GetConnectionString("SqlServer");
            var favService = new Services.FavoriteService(connStr);

            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.CreateEntryFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<CreateEntryFavEvent>(async (fav) =>
                {
                    favService.CreateEntryFav(fav).GetAwaiter().GetResult();
                     _logger.LogInformation($"Received {fav.EntryId} from {fav.CreatedBy}");
                    // Burada gelen event'e göre iþlem yapýlacak.
                    // Örneðin; EntryId'si 5 olan entry'e favori eklenmiþse, EntryId'si 5 olan entry'i favori olarak db'ye eklemiþ olacagýz.       
                })
                .StartingConsuming(SozlukConstants.CreateEntryFavQueueName);
        }
    }
}