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
    public class SiteSqlDAOTests
    {
        private TransactionScope transaction = null;
        private string connectionString = "Server=.\\SqlExpress;Database=NPCampground;Trusted_Connection=True;";
        private int lastSiteId;
        private int lastCampgroundId;
        private int Confirmation;

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
                    lastSiteId = Convert.ToInt32(rdr["lastsite"]);
                    lastCampgroundId = Convert.ToInt32(rdr["lastCampground"]);
                    Confirmation = Convert.ToInt32(rdr["lastres"]);
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
        public void AvailableSitesTests()
        {
            // Arrange
            SiteSqlDAO dao = new SiteSqlDAO(connectionString);

            // Act
            IList<Site> actual = dao.AvailableSites(lastCampgroundId, new DateTime(2020,04,05), new DateTime(2020,04,07));
         
            // Assert 
            Assert.AreEqual(1, actual.Count);

        }
    }
}
