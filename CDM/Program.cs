using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDM.Tasks.Data.Models;
using CDM.Tasks.Implementation;
using MongoDB.Driver;

namespace CDM
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new MongoTasksRepository();
            //var t = test.UpsertTask(new TaskData(3, "t"));
            //test.UpsertTask(new TaskData(4, "t"));
            var d = test.DeleteTasks(new List<TaskData>()
            {
                new TaskData(3, "t"),
                new TaskData(4, "t")
            });
        }
    }
}
