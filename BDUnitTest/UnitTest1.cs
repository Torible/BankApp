using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BDDLL;
using System.Diagnostics;

namespace BDUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        string connectionString = "Data Source=DESKTOP-8P06H53;Initial Catalog=TestBD;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        
        [TestMethod]
        public void GetCostForProduct()
        {
            float? exepectedResult = 15000;
            Database db =  Database.GetInstance(connectionString);
            string query = "Select Стоимость From TestTable Where Название = @Название";
            float? actualResult = (float?)db.GetScalar(query, new Parameter[]{ new Parameter("Название", "Монитор")});
            Assert.AreEqual(exepectedResult, actualResult, "Скаляр не был получен по запросу");
        }

        [TestMethod]
        public void GetCostForProduct_NonExistentRecord()
        {
            float? exepectedResult = null;
            Database db = Database.GetInstance(connectionString);
            string query = "Select Стоимость From TestTable Where Название = @Название";
            float? actualResult = (float?)db.GetScalar(query, new Parameter[] { new Parameter("Название", "Телефон") });
            Assert.AreEqual(exepectedResult, actualResult, "Скаляр был получен по запросу");
        }

        [TestMethod]
        public void InsertNewProduct()
        {
            int exepectedResult = 1;
            Database db = Database.GetInstance(connectionString);
            string query = "Insert Into TestTable Values (@Название, @Стоимость, @Вес)";
            int actualResult = db.Execute(query, new Parameter[] { new Parameter("Название", "Наушники"), new Parameter("Стоимость", "800"), new Parameter("Вес", "0.2") });
            Assert.AreEqual(exepectedResult, actualResult, "Запись с таким названием уже существует");

            db.Execute("Delete From TestTable Where Название = @Название", new Parameter[] { new Parameter("Название", "Наушники") });
        }

        [TestMethod]
        public void InsertNewProduct_RecordWithSamePKExist()
        {
            int exepectedResult = 0;
            Database db = Database.GetInstance(connectionString);
            string query = "Insert Into TestTable (Название, Стоимость, Вес) Values (@Название, @Стоимость, @Вес)";
            int actualResult = db.Execute(query, new Parameter[] { new Parameter("Название", "Монитор"), new Parameter("Стоимость", "400"), new Parameter("Вес", "3") });
            Assert.AreEqual(exepectedResult, actualResult, "Записи с таким названием не существовало");
        }

        [TestMethod]
        public void GetDataTable()
        {
            Database db = Database.GetInstance(connectionString);
            string query = "Select * From TestTable Where Стоимость < @Стоимость";
            var actualResult = db.GetRowsData(query, new Parameter[] {new Parameter("Стоимость", "4000") });
            CollectionAssert.AllItemsAreNotNull(actualResult, "Результат запроса не содержит данных");
        }

        [TestMethod]
        public void GetDataTable_WithNoSuitableRecords()
        {
            Database db = Database.GetInstance(connectionString);
            string query = "Select Стоимость From TestTable Where Вес = @Вес)";
            var actualResult = db.GetRowsData(query, new Parameter[] { new Parameter("Вес", "17") });
            Assert.IsNull(actualResult, "Результат запроса содержит данные");
        }

        [TestMethod]
        public void GetDepositProfit_WithIntPrecision()
        {
            float expectedResult = 2000;
            float actualResult = BankUtilities.CalculateProfit(10000, 10, 24);
            Assert.AreEqual(expectedResult, actualResult, "Фактический результат отличается от ожидаемого");
        }

        [TestMethod]
        public void GetDepositProfit_WithFloatPrecision()
        {
            float expectedResult = 3372.85f;
            float actualResult = BankUtilities.CalculateProfit(67457, 5, 12);
            Assert.AreEqual(expectedResult, actualResult, "Фактический результат отличается от ожидаемого");
        }

        [TestMethod]
        public void GetDepositClosingDate_WithMonthsPrecision()
        {
            DateTime expectedResult = new DateTime(2005, 10, 13);
            DateTime actualResult = BankUtilities.GetClosingDate(new DateTime(2004, 12, 13), 10);
            Assert.AreEqual(expectedResult, actualResult, "Фактический результат отличается от ожидаемого");
        }

        [TestMethod]
        public void GetDepositClosingDate_WithYearsPrecision()
        {
            DateTime expectedResult = new DateTime(2007, 12, 13);
            DateTime actualResult = BankUtilities.GetClosingDate(new DateTime(2004, 12, 13), 36);
            Assert.AreEqual(expectedResult, actualResult, "Фактический результат отличается от ожидаемого");
        }
    }
}
