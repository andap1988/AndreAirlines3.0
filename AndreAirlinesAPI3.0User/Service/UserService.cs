using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0User.Utils;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0User.Service
{
    public class UserService
    {
        private readonly IMongoCollection<User> _user;

        public UserService(IAndreAirlinesDatabaseUserSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>(settings.UserCollectionName);
        }

        public List<User> Get()
        {
            List<User> users = new();

            try
            {
                users = _user.Find(user => true).ToList();

                return users;
            }
            catch (Exception exception)
            {
                users.Add(new User());

                if (exception.InnerException != null)
                    users[0].ErrorCode = exception.InnerException.Message;
                else
                    users[0].ErrorCode = exception.Message.ToString();

                return users;
            }
        }

        public User Get(string id)
        {
            User user = new();

            if (id.Length != 24)
            {
                user.ErrorCode = "noLength";

                return user;
            }

            try
            {
                user = _user.Find<User>(user => user.Id == id).FirstOrDefault();

                return user;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    user.ErrorCode = exception.InnerException.Message;
                else
                    user.ErrorCode = exception.Message.ToString();

                return user;
            }
        }

        public User GetLoginUser(string loginUser)
        {
            User user = new();
            try
            {
                user = _user.Find<User>(user => user.LoginUser == loginUser).FirstOrDefault();

                return user;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    user.ErrorCode = exception.InnerException.Message;
                else
                    user.ErrorCode = exception.Message.ToString();

                return user;
            }
        }

        public User GetCpf(string cpf)
        {
            User user = new();

            try
            {
                user = _user.Find<User>(user => user.Cpf == cpf).FirstOrDefault();

                return user;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    user.ErrorCode = exception.InnerException.Message;
                else
                    user.ErrorCode = exception.Message.ToString();

                return user;
            }
        }

        public async Task<User> Create(User user)
        {
            var userLogin = GetLoginUser(user.LoginUser);

            if (userLogin == null)
            {
                user.ErrorCode = "noBlank";

                return user;
            }
            else if (userLogin.ErrorCode != null)
            {
                user.ErrorCode = userLogin.ErrorCode;

                return user;
            }
            else if (userLogin.Sector != "ADM")
            {
                user.ErrorCode = "noPermited";

                return user;
            }

            bool isValid = VerifyCpf.IsValidCpf(user.Cpf);

            if (!isValid)
            {
                user.ErrorCode = "noCpf";

                return user;
            }

            var searchUser = GetCpf(user.Cpf);

            if (searchUser != null)
            {
                user.ErrorCode = "yesUser";

                return user;
            }

            _user.InsertOne(user);

            Log log = new();
            log.User = user;
            log.BeforeEntity = "";
            log.AfterEntity = JsonConvert.SerializeObject(user);
            log.Operation = "create";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
            {
                _user.DeleteOne(userIn => userIn.Id == user.Id);
                user.ErrorCode = "noLog";

                return user;
            }

            return user;
        }

        public async Task<string> Update(string id, User userIn, User user)
        {
            var userBefore = Get(userIn.Id);

            _user.ReplaceOne(user => user.Id == id, userIn);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(userBefore);
            log.AfterEntity = JsonConvert.SerializeObject(userIn);
            log.Operation = "update";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
                _user.ReplaceOne(user => user.Id == id, userBefore);

            return returnMsg;
        }

        public async Task<string> Remove(string id, User userIn, User user)
        {
            var userBefore = Get(userIn.Id);

            _user.DeleteOne(user => user.Id == userIn.Id);

            Log log = new();
            log.User = user;
            log.BeforeEntity = JsonConvert.SerializeObject(userBefore);
            log.AfterEntity = "";
            log.Operation = "delete";
            log.InsertionDate = DateTime.Now.Date;
            log.ErrorCode = null;

            var returnMsg = await PostLogService.InsertLog(log);

            if (returnMsg != "ok")
                _user.InsertOne(userBefore);

            return returnMsg;
        }
    }
}
