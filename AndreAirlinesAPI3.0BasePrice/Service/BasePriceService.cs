using AndreAirlinesAPI3._0BasePrice.Utils;
using AndreAirlinesAPI3._0Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0BasePrice.Service
{
    public class BasePriceService
    {
        private readonly IMongoCollection<BasePrice> _basePrice;

        public BasePriceService(IAndreAirlinesDatabaseBasePriceSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _basePrice = database.GetCollection<BasePrice>(settings.BasePriceCollectionName);
        }

        public List<BasePrice> Get()
        {
            List<BasePrice> basePrices = new();

            try
            {
                basePrices = _basePrice.Find(basePrice => true).ToList();

                return basePrices;
            }
            catch (Exception exception)
            {
                basePrices.Add(new BasePrice());

                if (exception.InnerException != null)
                    basePrices[0].ErrorCode = exception.InnerException.Message;
                else
                    basePrices[0].ErrorCode = exception.Message.ToString();

                return basePrices;
            }
        }            

        public BasePrice Get(string id)
        {
            BasePrice basePrice = new();

            if (id.Length != 24)
            {
                basePrice.ErrorCode = "noLength";

                return basePrice;
            }

            try
            {
                basePrice = _basePrice.Find<BasePrice>(basePrice => basePrice.Id == id).FirstOrDefault();

                return basePrice;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    basePrice.ErrorCode = exception.InnerException.Message;
                else
                    basePrice.ErrorCode = exception.Message.ToString();

                return basePrice;
            }
        }       

        public async Task<BasePrice> Create(BasePrice basePrice)
        {
            if (basePrice.LoginUser == null)
            {
                basePrice.ErrorCode = "noBlank";

                return basePrice;
            }

            var airportOrigin = await SearchAirport.ReturnAirport(basePrice.Origin);

            if (airportOrigin.ErrorCode != null)
            {
                basePrice.Origin = airportOrigin;

                return basePrice;
            }
            else
                basePrice.Origin = airportOrigin;

            var airportDestiny = await SearchAirport.ReturnAirport(basePrice.Destiny);

            if (airportDestiny.ErrorCode != null)
            {
                basePrice.Destiny = airportDestiny;

                return basePrice;
            }
            else
                basePrice.Destiny = airportDestiny;

            var user = await SearchUser.ReturnUser(basePrice.LoginUser);

            if (user.ErrorCode != null)
            {
                basePrice.ErrorCode = user.ErrorCode;

                return basePrice;
            }
            else if (user.Sector != "ADM")
            {
                basePrice.ErrorCode = "noPermited";

                return basePrice;
            }
            else
                _basePrice.InsertOne(basePrice);

            Log log = new();
            log.User = user;
            log.BeforeEntity = "";
            log.AfterEntity = JsonConvert.SerializeObject(basePrice);
            log.Operation = "create";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
            {
                _basePrice.DeleteOne(airportIn => airportIn.Id == basePrice.Id);
                basePrice.ErrorCode = "noLog";

                return basePrice;
            }

            return basePrice;
        }

        public async Task<string> Update(string id, BasePrice basePriceIn, User user)
        {
            var basePriceBefore = Get(basePriceIn.Id);

            _basePrice.ReplaceOne(basePrice => basePrice.Id == id, basePriceIn);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(basePriceBefore);
            log.AfterEntity = JsonConvert.SerializeObject(basePriceIn);
            log.Operation = "update";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
                _basePrice.ReplaceOne(basePrice => basePrice.Id == id, basePriceBefore);

            return returnMsg;
        }

        public async Task<string> Remove(string id, BasePrice basePriceIn, User user)
        {
            var basePriceBefore = Get(basePriceIn.Id);

            _basePrice.DeleteOne(basePrice => basePrice.Id == basePriceIn.Id);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(basePriceBefore);
            log.AfterEntity = "";
            log.Operation = "delete";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
                _basePrice.InsertOne(basePriceBefore);

            return returnMsg;
        }
    }
}
