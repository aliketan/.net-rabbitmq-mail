namespace App.Application.Constants
{
    public class RabbitMqConsts
    {
        //yaşam süresi
        public static int MessagesTTL { get; set; } = 1000 * 60 * 60 * 2;

        //Aynı anda - eş zamanlı e-posta gönderimi sayısı, thread açma için sınırı belirleriz.
        public static ushort ParallelThreadsCount { get; set; } = 3;

        /// <summary>
        /// durable: ile in-memory mi yoksa fiziksel olarak mı saklanacağı belirlenir.
        /// exclusive: yalnızca bir bağlantı tarafından kullanılır ve bu bağlantı kapandığında sıra silinir — özel olarak işaretlenirse silinmez.
        /// autoDelete: En son bir abonelik iptal edildiğinde en az bir müşteriye sahip olan kuyruk silinir.
        /// arguments: İsteğe bağlı; eklentiler tarafından kullanılır ve TTL mesajı, kuyruk uzunluğu sınırı, vb. özellikler tanımlanır.
        /// </summary>
        public static class Options
        {
            public static bool Durable { get; set; } = true;
            public static bool Exclusive { get; set; } = false;
            public static bool AutoDelete { get; set; } = true;
            public static IDictionary<string, object> Arguments { get; set; } = null;
        }
    }
}
