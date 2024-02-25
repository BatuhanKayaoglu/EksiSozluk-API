using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common
{
    public class SozlukConstants
    {
        public string RabbitMQHost = "amqps://pwayvqwa:FH59f9X2QwhP9LJjHAs4a3v8XAA-W-id@woodpecker.rmq.cloudamqp.com/pwayvqwa";
        public const string RabbitMQHostUri = "localhost";
        public const string DefaultExchangeType = "direct";


        public const string UserExchangeName = "UserExchange";
        public const string UserEmailChangedQueueName = "UserEmailChangedQueue";


        public const string FavExchangeName = "FavExchange";
        public const string CreateEntryFavQueueName = "CreateEntryFavFavQueue";
        public const string CreateEntryCommentFavQueueName = "CreateEntryCommentFavQueue";
        public const string CreateEntryCommentVoteQueueName = "CreateEntryCommentVoteQueue";
        public const string DeleteEntryCommentFavQueueName = "DeleteEntryCommentFavQueue";
        public const string DeleteEntryFavQueueName = "DeleteEntryFavQueue";

        public const string VoteExchangeName= "VoteExchange";
        public const string CreateEntryVoteQueueName = "CreateEntryVoteQueue";
        public const string DeleteEntryVoteQueueName = "DeleteEntryVoteQueue";
        public const string DeleteEntryCommentVoteQueueName = "DeleteEntryCommentVoteQueue";

    }
}
