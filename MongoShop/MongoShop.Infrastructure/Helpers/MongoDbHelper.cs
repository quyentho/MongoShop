namespace MongoShop.Infrastructure.Helpers
{
    public static class MongoDbHelper
    {
        /// <summary>
        /// Gets mongodb collection name by extracting from the service's name.
        /// </summary>
        /// <param name="serviceName">Controller name.</param>
        /// <returns></returns>
        public static string GetCollectionName(string serviceName)
        {
            serviceName = serviceName.Replace("Services", "");

            return serviceName.ToLower();
        }
    }
}
