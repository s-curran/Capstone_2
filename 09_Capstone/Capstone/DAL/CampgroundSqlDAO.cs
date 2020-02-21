using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string ConnectionString;
        public CampgroundSqlDAO(string DBConnectionString)
        {
            ConnectionString = DBConnectionString;
        }
        public IList<Campground> GetCampgrounds(int parkId)
        {
            IList<Campground> campgrounds = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "Select * from campground WHERE park_id = @park";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@park", parkId);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Campground campground = new Campground();

                        campground.CampgroundId = Convert.ToInt32(rdr["campground_id"]);
                        campground.ParkId = Convert.ToInt32(rdr["park_id"]);
                        campground.Name = Convert.ToString(rdr["name"]);
                        campground.OpenFrom = Convert.ToInt32(rdr["open_from_mm"]);
                        campground.OpenTo = Convert.ToInt32(rdr["open_to_mm"]);
                        campground.DailyFee = Convert.ToDecimal(rdr["daily_fee"]);

                        campgrounds.Add(campground);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return campgrounds;
        }
    }
}
