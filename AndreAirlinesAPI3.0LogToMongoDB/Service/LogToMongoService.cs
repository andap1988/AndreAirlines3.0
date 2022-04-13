using AndreAirlinesAPI3._0LogToMongoDB.Utils;
using AndreAirlinesAPI3._0Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0LogToMongoDB.Service
{
    public class LogToMongoService
    {
        private readonly IMongoCollection<Log> _log;

        public LogToMongoService(IAndreAirlinesDatabaseLogToMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _log = database.GetCollection<Log>(settings.LogCollectionName);
        }

        public List<Log> GetLog()
        {
            List<Log> logs = new();

            try
            {
                logs = _log.Find(log => true).ToList();

                return logs;
            }
            catch (Exception exception)
            {
                logs.Add(new Log());

                if (exception.InnerException != null)
                    logs[0].ErrorCode = exception.InnerException.Message;
                else
                    logs[0].ErrorCode = exception.Message.ToString();

                return logs;
            }
        }

        public Log GetLog(string id)
        {
            Log log = new();

            try
            {
                log = _log.Find<Log>(log => log.Id == id).FirstOrDefault();

                return log;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    log.ErrorCode = exception.InnerException.Message;
                else
                    log.ErrorCode = exception.Message.ToString();

                return log;
            }
        }

        public async Task<Log> Create(Log log)
        {
            _log.InsertOne(log);

            return log;
        }
    }
}
