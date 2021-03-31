namespace MongoShop.BusinessDomain
{
    public class DatabaseSetting : IDatabaseSetting
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
