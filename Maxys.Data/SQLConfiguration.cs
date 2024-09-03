namespace ERP_MaxysHC.Maxys.Data
{
    public class SQLConfiguration
    {
        public string ConnectionString { get; set; }

        public SQLConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

    }
}
