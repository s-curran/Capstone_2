using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundSqlDAOTests
    {
        private TransactionScope transaction = null;
        private string connectionString = "Server=.\\SqlExpress;Database=NPCampground;Trusted_Connection=True;";
        private int lastCampgroundId;
        private int lastParkId;


        [TestInitialize]
        public void SetupDatabase()
        {
            // Start a transaction, so we can roll back when we are finished with this test
            transaction = new TransactionScope();

            // Open Setup.Sql and read in the script to be executed
            string setupSQL;
            using (StreamReader rdr = new StreamReader("Setup.sql"))
            {
                setupSQL = rdr.ReadToEnd();
            }

            // Connect to the DB and execute the script
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(setupSQL, conn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lastCampgroundId = Convert.ToInt32(rdr["lastcampground"]);
                    lastParkId = Convert.ToInt32(rdr["lastPark"]);
                }
            }
        }


        [TestCleanup]
        public void CleanupDatabase()
        {
            // Rollback the transaction to get our good data back
            transaction.Dispose();
        }
        [TestMethod]
        public void GetCampgroundsTest()
        {
            // Arrange
            CampgroundSqlDAO dao = new CampgroundSqlDAO(connectionString);

            // Act
            IList<Campground> actual = dao.GetCampgrounds(lastParkId);

            // Assert 
            Assert.AreEqual(1, actual.Count);

        }
    }

}
