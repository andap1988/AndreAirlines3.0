using AndreAirlinesAPI3._0Class.Utils;
using AndreAirlinesAPI3._0Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace AndreAirlinesAPI3._0Class.Service
{
    public class ClassService
    {
        private readonly IMongoCollection<Class> _class;

        public ClassService(IAndreAirlinesDatabaseClassSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _class = database.GetCollection<Class>(settings.ClassCollectionName);
        }

        public List<Class> Get()
        {
            List<Class> classes = new();

            try
            {
                classes = _class.Find(clas => true).ToList();

                return classes;
            }
            catch (Exception exception)
            {
                classes.Add(new Class());

                if (exception.InnerException != null)
                    classes[0].ErrorCode = exception.InnerException.Message;
                else
                    classes[0].ErrorCode = exception.Message.ToString();

                return classes;
            }
        }

        public Class Get(string id)
        {
            Class classs = new();

            if (id.Length != 24)
            {
                classs.ErrorCode = "noLength";

                return classs;
            }

            try
            {
                classs = _class.Find<Class>(clas => clas.Id == id).FirstOrDefault();

                return classs;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    classs.ErrorCode = exception.InnerException.Message;
                else
                    classs.ErrorCode = exception.Message.ToString();

                return classs;
            }
        }

        public Class Create(Class classs)
        {            
            try
            {
                _class.InsertOne(classs);

                return classs;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    classs.ErrorCode = exception.InnerException.Message;
                else
                    classs.ErrorCode = exception.Message.ToString();

                return classs;
            }            
        }

        public void Update(string id, Class classIn) =>
            _class.ReplaceOne(classs => classs.Id == id, classIn);

        public void Remove(Class classIn) =>
            _class.DeleteOne(classs => classs.Id == classIn.Id);

        public void Remove(string id) =>
            _class.DeleteOne(classs => classs.Id == id);
    }
}
