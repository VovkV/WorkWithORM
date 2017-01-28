using System;
using System.Collections.Generic;
using CDM.Tasks.Data.Interfaces;
using CDM.Tasks.Data.Models;

namespace CDM.Tasks.Implementation
{
    class MongoTasksRepository : ITasksRepository
    {
        public List<TaskData> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public TaskData GetTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public TaskData GetTasksByUser(TaskUser user)
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
    }
}
