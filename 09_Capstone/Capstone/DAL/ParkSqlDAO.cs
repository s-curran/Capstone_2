using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ParkSqlDAO : IParkDAO
    {
        private string ConnectionString;
        public ParkSqlDAO(string DBConnectionString)
        {
            ConnectionString = DBConnectionString;
        }
        public IList<Park> GetAllParks()
        {
            IList<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    String Sql = "SELECT * FROM Park Order By name";

                    SqlCommand cmd = new SqlCommand(Sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Park park = new Park();

                        park.ParkId = Convert.ToInt32(rdr["park_id"]);
                        park.Name = Convert.ToString(rdr["name"]);
                        park.Location = Convert.ToString(rdr["location"]);
                        park.EstablishedDate = Convert.ToDateTime(rdr["establish_date"]);
                        park.Area = Convert.ToInt32(rdr["area"]);
                        park.Visitors = Convert.ToInt32(rdr["visitors"]);
                        park.Description = Convert.ToString(rdr["description"]);

                        parks.Add(park);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return parks;
        }
        public Park GetPark(int parkId)
        {
            Park park = new Park();
            //IList<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    String Sql = "SELECT * FROM Park WHERE park_id = @parkId Order By name";

                    SqlCommand cmd = new SqlCommand(Sql, conn);
                    cmd.Parameters.AddWithValue("@parkId", parkId);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        park.ParkId = Convert.ToInt32(rdr["park_id"]);
                        park.Name = Convert.ToString(rdr["name"]);
                        park.Location = Convert.ToString(rdr["location"]);
                        park.EstablishedDate = Convert.ToDateTime(rdr["establish_date"]);
                        park.Area = Convert.ToInt32(rdr["area"]);
                        park.Visitors = Convert.ToInt32(rdr["visitors"]);
                        park.Description = Convert.ToString(rdr["description"]);
                        
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            return park;
        }

    }
}
