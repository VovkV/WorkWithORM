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
            _collection = database.GetCollection<TaskData>("Tasks");
        }
        #region
        public List<TaskData> GetAllTasks()
        {
            var result = _collection.Find(p => true).ToList();

            return result;
        }

        public TaskData GetTaskById(int id)
        {
            TaskData result = null;

            var filtered = _collection.Find(p => p.Id == id).ToList();

            if(filtered.Count >=1)
                 result = filtered.First();

            return result;
        }

        public List<TaskData> GetTasksByUserGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasks(List<TaskData> tasks)
        {
            try
            {
                int result = 0;
                foreach (var task in tasks)
                {
                    result += (int)_collection.DeleteOne(new BsonDocument("_id", task.Id)).DeletedCount;
                }
                return result == tasks.Count;
            }
            catch (Exception ex)
            {
                var t= ex.Message;
                return false;
            }
        }

        public bool UpsertTask(TaskData task)
        {
            if (task.Id <= 0 || task.Text == null)
                return false;
            try
            {
                _collection.ReplaceOne(
                    new BsonDocument("_id", task.Id),
                    task,
                    new UpdateOptions {IsUpsert = true});
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteTaskById(int id)
        {
            try
            {
                var result = _collection.DeleteOne(new BsonDocument("_id", id));
                return result.DeletedCount == 1;
            }
            catch (Exception )
            {
                return false;
            }
        }
        #endregion
    }
}
