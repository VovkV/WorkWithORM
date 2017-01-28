using System;
using System.Collections.Generic;
using CDM.Tasks.Data.Interfaces;
using CDM.Tasks.Data.Models;
using MicroLite;
using MicroLite.Builder;
using MicroLite.Configuration;

namespace CDM.Tasks.Implementation
{
    public class MicroLiteTasksRepository : ITasksRepository
    {
        private readonly ISessionFactory _sessionFactory;

        public MicroLiteTasksRepository()
        {
            Configure.Extensions().WithAttributeBasedMapping();
            _sessionFactory =
                Configure.Fluently().ForMsSql2012Connection("Tasks").CreateSessionFactory();
        }

        #region ITasksRepository

        public List<TaskData> GetAllTasks()
        {
            List<TaskData> result = new List<TaskData>();
            using (var session = _sessionFactory.OpenSession())
            {
                var list =
                    session.Fetch<TaskData>(SqlBuilder.Select("*").From(typeof(TaskData)).ToSqlQuery());
                result.AddRange(list);
            }
            return result;
        }

        public TaskData GetTaskById(int id)
        {
            TaskData result;
            using (var session = _sessionFactory.OpenSession())
            {
                result = session.Single<TaskData>(id);
            }
            return result;
        }

        public TaskData GetTasksByUser(TaskUser user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasks(List<TaskData> tasks)
        {
            int deleteCount = 0;
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var field in tasks)
                    {
                        if (session.Delete(field))
                            deleteCount++;
                    }
                    transaction.Commit();
                }
            }
            return deleteCount == tasks.Count;
        }

        public bool UpsertTask(TaskData task)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                try
                {
                    var line = session.Single<TaskData>(task.Id);

                    if (line == null)
                    {
                        SqlQuery query;
                        if (task.Id == 0)
                            query = new SqlQuery("SET IDENTITY_INSERT dbo.Tasks OFF; " +
                                                 "Insert Into Tasks (task_text) Values (@taskText)",
                                task.Text);
                        else
                            query = new SqlQuery("SET IDENTITY_INSERT dbo.Tasks ON; " +
                                                 "Insert Into Tasks (task_id,task_text) Values (@taskId,@taskText)",
                                task.Id, task.Text);

                        session.Fetch<dynamic>(query);
                    }
                    else
                    {
                        line.Text = task.Text;
                        session.Update(line);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }

        public bool DeleteTaskById(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                try
                {
                    var deleted = session.Delete(new TaskData(id,""));
                    return deleted;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
