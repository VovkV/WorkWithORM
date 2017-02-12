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
            test.GetAllTasks();
        }
    }
}
