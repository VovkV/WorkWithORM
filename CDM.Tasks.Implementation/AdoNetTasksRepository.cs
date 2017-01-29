using CDM.Tasks.Data.Interfaces;
using CDM.Tasks.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CDM.Tasks.Implementation
{
    public class AdoNetTasksRepository : ITasksRepository
    {
        private readonly string _connectionString;

        private SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public AdoNetTasksRepository()
        {
            _connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["Tasks"].ConnectionString;
        }

        #region ITasksRepository   

        public List<TaskData> GetAllTasks()
        {
            var tasksData = new List<TaskData>();

            using (var connection = GetConnection())
            {
                var sqlCommand = new SqlCommand("Select * From Tasks", connection);
                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                    tasksData.Add(new TaskData(sqlReader.GetInt32(0), sqlReader.GetString(1)));
            }

            return tasksData;
        }

        public TaskData GetTaskById(int id)
        {
            TaskData taskData = null;

            using (var connection = GetConnection())
            {
                var sqlCommand = new SqlCommand("Select * From Tasks Where task_id=@id", connection);
                sqlCommand.Parameters.AddWithValue("@id", id);
                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    taskData = new TaskData();
                    taskData.Id = sqlReader.GetInt32(0);
                    taskData.Text = sqlReader.GetString(1);
                }
            }

            return taskData;
        }

        public TaskData GetTasksByUser(TaskUser user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasks(List<TaskData> tasks)
        {
            int deleteCount = 0;

            using (var connection = GetConnection())
            {
                using (var tr = connection.BeginTransaction())
                {
                    string sqlExpression = "Delete From Tasks Where task_id=@fieldId";
                    var sqlCommand = new SqlCommand(sqlExpression, connection,tr);
                    sqlCommand.Parameters.Add("@fieldId",SqlDbType.Int);

                    foreach (var field in tasks)
                    {
                        sqlCommand.Parameters["@fieldId"].Value = field.Id;
                        deleteCount += sqlCommand.ExecuteNonQuery();
                    }

                    tr.Commit();
                }
            }

            return deleteCount == tasks.Count;
        }

        public bool UpsertTask(TaskData task)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    if (task.Id < 0)
                        throw new Exception("Negative ID");

                    var cmd = new SqlCommand("Select count(*) From Tasks Where task_id=@taskId",
                        connection);
                    cmd.Parameters.AddWithValue("@taskId", task.Id);
                    cmd.Parameters.AddWithValue("@taskText", task.Text);
                    int recCount = (int) cmd.ExecuteScalar();

                    if (recCount == 0)
                    {
                        if (task.Id == 0)
                        { 
                            cmd.CommandText = "SET IDENTITY_INSERT dbo.Tasks OFF; Insert Into Tasks (task_text) Values (@taskText)";
                        }
                        else
                        {
                            cmd.CommandText = "SET IDENTITY_INSERT dbo.Tasks ON; Insert Into Tasks (task_id,task_text) Values (@taskId,@taskText)";
                        }
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE Tasks SET task_text=@taskText WHERE task_id=@taskId";
                        cmd.ExecuteNonQuery();
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
            using (var connection = GetConnection())
            {
                try
                {
                    
                    var sqlCommand = new SqlCommand("Delete From Tasks Where task_id=@id", connection);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    int result = sqlCommand.ExecuteNonQuery();
                    return result == 1;
                }
                catch(Exception)
                {
                    return false;
                }
            }     
        }
        #endregion 
    }
}
