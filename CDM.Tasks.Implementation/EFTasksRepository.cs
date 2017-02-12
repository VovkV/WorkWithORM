using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CDM.Tasks.Data.Interfaces;
using CDM.Tasks.Data.Models;

namespace CDM.Tasks.Implementation
{
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("Tasks") { }

        public DbSet<TaskData> Tasks { get; set; }
    }
    public class EFTasksRepository : ITasksRepository
    {
        private DbSet<TaskData> _tasks;
        public EFTasksRepository()
        {
            _tasks = new EFDbContext().Tasks;
            var t = new EFDbContext();
            
        }

        #region ITasksRepository
        public bool DeleteTaskById(int id)
        {
            var result = _tasks.Remove(new TaskData(id, ""));
            return result!=null;
        }

        public bool DeleteTasks(List<TaskData> tasks)
        {
            throw new NotImplementedException();
        }

        public List<TaskData> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public TaskData GetTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TaskData> GetTasksByUserGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool UpsertTask(TaskData task)
        {
            throw new NotImplementedException();

        }
#endregion
    }

}
