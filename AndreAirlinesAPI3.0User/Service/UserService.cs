using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0User.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

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

        public User Create(User user)
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

            return user;
        }

        public void Update(string id, User userIn) =>
            _user.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _user.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _user.DeleteOne(user => user.Id == id);
    }
}
