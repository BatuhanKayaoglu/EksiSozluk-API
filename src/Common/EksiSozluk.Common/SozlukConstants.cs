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

    }
}
