using NLog;
using System;
using System.Data;
using System.Data.SqlClient;


namespace AVulnerableLibrary
{
    public class VulnerableQueryExecutor
    {
        public string ConnectionString { get; set; }
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public VulnerableQueryExecutor() {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }
        protected SqlConnection GetConnection() {
            Logger.Info("Logging connection string - this is unsafe as we log username, password and dbserver");
            Logger.Info(ConnectionString);

            return new SqlConnection(ConnectionString);
        }

        public void RunSelectQuery(string parameter) {
            string query = "SELECT column1, column2 FROM FOO WHERE X = " + parameter;

            using (SqlConnection sqlC = GetConnection())
            {
                sqlC.Open();
                SqlCommand cmd = new SqlCommand(query, sqlC);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    ReadSingleRow((IDataRecord)reader);
                }
            } 
        }
        private static void ReadSingleRow(IDataRecord record)
        {
            Console.WriteLine(String.Format("{0}, {1}", record[0], record[1]));
        }
    }
}
