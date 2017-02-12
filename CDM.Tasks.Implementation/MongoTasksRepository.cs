using System;
using System.Collections.Generic;
using System.Linq;
using CDM.Tasks.Data.Interfaces;
using CDM.Tasks.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CDM.Tasks.Implementation
{
    public class MongoTasksRepository : ITasksRepository
    {
        private IMongoCollection<TaskData> _collection;
        public MongoTasksRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test");
            _collection = database.GetCollection<TaskData>("Task");
        }
        #region
        public List<TaskData> GetAllTasks()
        {
            var result = new List<TaskData>();

            var docs = _collection;
            foreach (var doc in docs)
            {
                result.Add(new TaskData {Id = doc.});
            }
            return null;
        }

        public TaskData GetTaskById(int id)
        {
            try
            {
                var result = new List<TaskData>();
                var filter = Builders<BsonDocument>.Filter.Eq("id", id.ToString());
                var docs = _collection.Find<TaskData>(filter).ToList();

                if (docs.Count!=1)
                    throw new Exception("more then one with this ID");

                var doc = docs.FirstOrDefault();

                return new TaskData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public List<TaskData> GetTasksByUserGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasks(List<TaskData> tasks)
        {
            throw new NotImplementedException();
        }

        public bool UpsertTask(TaskData task)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTaskById(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
