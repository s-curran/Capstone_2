using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SiteSqlDAO
    {
        private string ConnectionString;
        public SiteSqlDAO (string DBConnectionString)
        {
            ConnectionString = DBConnectionString;
        }

        public string startdate;
        public string enddate;

        public IDictionary<int, decimal> AvailableSites(Campground campground, string startdate1, string enddate1)
        {
            startdate = startdate1;
            enddate = enddate1;

            IDictionary<int, decimal> sites = new Dictionary<int, decimal>();

            DateTime.TryParse(startdate, out DateTime start);
            DateTime.TryParse(enddate, out DateTime end);
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT DISTINCT TOP 5 s.site_id, daily_fee * DATEDIFF(Day, @startdate, @enddate) as totalFee FROM site s LEFT JOIN reservation r ON s.site_id = r.site_id JOIN campground c ON s.campground_id = c.campground_id WHERE c.name = @campground AND s.site_id NOT IN(SELECT site_id FROM reservation WHERE(@startdate BETWEEN from_date AND to_date) OR from_date IS NULL) AND s.site_id NOT IN(SELECT site_id FROM reservation WHERE(@enddate BETWEEN from_date AND to_date) OR from_date IS NULL) AND s.site_id NOT IN(SELECT site_id FROM reservation WHERE(from_date BETWEEN @startdate AND @enddate) OR from_date IS NULL)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campground", campground);
                    cmd.Parameters.AddWithValue("@startdate", start);
                    cmd.Parameters.AddWithValue("@enddate", end);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        int site = Convert.ToInt32(rdr["site_id"]);
                        decimal fee = Convert.ToDecimal(rdr["totalFee"]);
                        sites[site] = fee;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return sites;
        }

        public int BookReservation(string name, int siteId)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "insert into reservation (site_id, name, from_date, to_date, create_date) values (@siteId, @name, @startdate, @enddate, GETDATE()); select @@identity";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@siteId",siteId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@startdate", startdate);
                    cmd.Parameters.AddWithValue("@enddate", enddate);

                   int confirmation = Convert.ToInt32(cmd.ExecuteScalar());
                    return confirmation;
                }
            }
            catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
