using System.Collections.Generic;
using CDM.Tasks.Data.Models;

namespace CDM.Tasks.Data.Interfaces
{
    public interface ITasksRepository
    {
            List<TaskData> GetAllTasks();

            TaskData GetTaskById(int id);

            TaskData GetTasksByUser(TaskUser user);

            bool DeleteTasks(List<TaskData> tasks);

            bool UpsertTask(TaskData task);

            bool DeleteTaskById(int id);
        }
    }
