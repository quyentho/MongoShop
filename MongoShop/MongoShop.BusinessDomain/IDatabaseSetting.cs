namespace MongoShop.BusinessDomain
{
    public interface IDatabaseSetting
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
