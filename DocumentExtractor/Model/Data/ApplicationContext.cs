using Microsoft.EntityFrameworkCore;

namespace DocumentExtractor.Model.Data
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<ExecutorRecord> ExecutorRecords { get; set; }
        public DbSet<ExecutorRecordData> ExecutorRecordDatas { get; set; }
        private string DatabaseName { get; set; }
        private static string Host { get; set; }
        private static string User { get; set; }
        private static string Password { get; set; }
        private static string Port { get; set; }


        public ApplicationContext(string databaseName, string host, string user, string password, string port)
        {
            DatabaseName = databaseName;
            Host = host;
            User = user;
            Password = password;
            Port = port;
            Port = port;
            if (!Database.CanConnect())
            {
                /// TODO
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(@$"Host={Host};Database={DatabaseName};Username={User};Password={Password};Port={Port};");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
