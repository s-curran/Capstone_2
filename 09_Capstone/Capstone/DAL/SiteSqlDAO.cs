using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteDAO
    {
        private string ConnectionString;
        public SiteSqlDAO(string DBConnectionString)
        {
            ConnectionString = DBConnectionString;
        }


        public IList<Site> AvailableSites(int campgroundId, DateTime start, DateTime end)
        {

            IList<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT DISTINCT TOP 5 s.* FROM site s LEFT JOIN reservation r ON s.site_id = r.site_id JOIN campground c ON s.campground_id = c.campground_id WHERE c.campground_id = @campground AND s.site_id NOT IN(SELECT site_id FROM reservation WHERE(@startdate BETWEEN from_date AND to_date) OR from_date IS NULL) AND s.site_id NOT IN(SELECT site_id FROM reservation WHERE(@enddate BETWEEN from_date AND to_date) OR from_date IS NULL) AND s.site_id NOT IN(SELECT site_id FROM reservation WHERE(from_date BETWEEN @startdate AND @enddate) OR from_date IS NULL)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campground", campgroundId);
                    cmd.Parameters.AddWithValue("@startdate", start);
                    cmd.Parameters.AddWithValue("@enddate", end);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Site site = new Site();

                        site.SiteId = Convert.ToInt32(rdr["site_id"]);
                        site.SiteNumber = Convert.ToInt32(rdr["site_number"]);
                        site.CampgroundId = Convert.ToInt32(rdr["campground_id"]);
                        site.MaxOccupancy = Convert.ToInt32(rdr["max_occupancy"]);
                        site.IsAccessible = Convert.ToBoolean(rdr["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(rdr["max_rv_length"]);
                        site.HasUtilities = Convert.ToBoolean(rdr["utilities"]);

                        sites.Add(site);

                        //int site = Convert.ToInt32(rdr["site_id"]);
                        //decimal fee = Convert.ToDecimal(rdr["totalFee"]);
                        //sites[site] = fee;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return sites;
        }



    }
}
