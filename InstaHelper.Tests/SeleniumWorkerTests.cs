using InstaHelper.Selenium;
using InstaHelper.DataBase;
using NUnit.Framework;

namespace InstaHelper.Tests
{
    public class SeleniumWorkerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Authorization()
        {
            IDataBaseWorker dataBase = SqlWorker.getInstance();

            using (SeleniumWorker driver = new SeleniumWorker(dataBase))
            {

            }
        }
    }
}
