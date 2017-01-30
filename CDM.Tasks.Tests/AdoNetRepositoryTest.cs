using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDM.Tasks.Data.Interfaces;
using CDM.Tasks.Data.Models;
using CDM.Tasks.Implementation;
using Xunit;

namespace CDM.Tasks.Tests
{
    public class AdoNetRepositoryTest
    {
        private ITasksRepository _sut;
        private string _testText;
        private int _GetRand()
        {
            return 100000 + new Random().Next(100000, 200000);
        }
        public AdoNetRepositoryTest()
        {
            _testText = "TEST";
            _sut = new MicroLiteTasksRepository();
        }
        
        
        [Fact]
        public void TestInsertRecord()
        {
            int testId = _GetRand();
            _sut.UpsertTask(new TaskData(testId, _testText));

            var testTask = _sut.GetTaskById(testId);

            Assert.NotEqual(null,testTask);
            Assert.Equal(testId,testTask.Id);
            Assert.Equal(_testText,testTask.Text);

            _sut.DeleteTaskById(testId);
        }

        [Fact]
        public void TestDeleteTask()
        {
            int testId = _GetRand();
            _sut.UpsertTask(new TaskData(testId, _testText));

            bool testResultTrue = _sut.DeleteTaskById(testId);
            bool testResultFalse = _sut.DeleteTaskById(0);

            var testTask = _sut.GetTaskById(testId);

            Assert.Equal(null,testTask);
            Assert.Equal(true,testResultTrue);
            Assert.Equal(false,testResultFalse);
        }

        [Fact]
        public void TestDoubleInsert()
        {
            int taskId = _GetRand();
            _sut.UpsertTask(new TaskData(taskId, "FirstInsert"));
            _sut.UpsertTask(new TaskData(taskId, "SecondInsert"));

            var testTask = _sut.GetTaskById(taskId);

            Assert.NotEqual(null, testTask);
            Assert.Equal(taskId, testTask.Id);
            Assert.Equal("SecondInsert", testTask.Text);

            _sut.DeleteTaskById(taskId);
        }

        [Fact]
        public void TestDeleteTasks()
        {
            List<TaskData> testList = new List<TaskData>();

            int rand = _GetRand();

            for (int i = 0; i < 1000; i++)
            {
                TaskData task = new TaskData(rand++,"DeleteTasks");

                testList.Add(task);

                _sut.UpsertTask(task);
            }

            bool testResultTrue = _sut.DeleteTasks(testList);
            bool testResultFalse = _sut.DeleteTasks(testList);

            foreach (var task in testList)
            {
                Assert.Equal(null, _sut.GetTaskById(task.Id));
            }

            Assert.Equal(true, testResultTrue);
            Assert.Equal(false, testResultFalse);
        }

        [Fact]
        public void TestInsertNegative()
        {
            int testId = -_GetRand();

            var testResult = _sut.UpsertTask(new TaskData(testId, _testText));
            var testTask = _sut.GetTaskById(testId);

            Assert.Equal(null,testTask);
            Assert.Equal(false, testResult);
        }

        [Fact]
        public void TestInsertNullArgument()
        {
            var task = new TaskData();
            task.Id = _GetRand();

            int countBefore = _sut.GetAllTasks().Count;
            var testResult = _sut.UpsertTask(task);
            int countAfter = _sut.GetAllTasks().Count;

            Assert.Equal(false, testResult);
            Assert.Equal(countBefore,countAfter);
        }
    }
}
