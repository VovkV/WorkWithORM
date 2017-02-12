using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        private AdoNetTasksRepository _sut;
        private string _testText;
        private Random _testId;
        private int _min;
        private int _max;
        public AdoNetRepositoryTest()
        {
            _testId = new Random();
            _testText = "TEST";
            _sut = new AdoNetTasksRepository();
            _min = 10000;
            _max = 20000;
        }
        
        
        [Fact]
        public void TestInsertRecord()
        {
            int testId = _testId.Next(_min,_max);
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
            int testId = _testId.Next(_min, _max);
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
            int taskId = _testId.Next(_min, _max);
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

            int rand = _testId.Next(_min, _max);

            for (int i = 0; i < 2; i++)
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
            int testId = -_testId.Next(_min, _max);

            var testResult = _sut.UpsertTask(new TaskData(testId, _testText));
            var testTask = _sut.GetTaskById(testId);

            Assert.Equal(null,testTask);
            Assert.Equal(false, testResult);
        }

        [Fact]
        public void TestInsertNullArgument()
        {
            var task = new TaskData();
            task.Id = _testId.Next(_min, _max);

            var testResult = _sut.UpsertTask(task);
            var testTask = _sut.GetTaskById(task.Id);

            Assert.Equal(false, testResult);
            Assert.Equal(null,testTask);
        }
    }
}
