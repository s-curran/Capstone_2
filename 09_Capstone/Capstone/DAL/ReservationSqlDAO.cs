using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO
    {
        private string ConnectionString;

        public ReservationSqlDAO(string DBConnectionString)
        {
            ConnectionString = DBConnectionString;
        }
        public int BookReservation(string name, int siteId, DateTime start, DateTime end)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "insert into reservation (site_id, name, from_date, to_date, create_date) values (@siteId, @name, @startdate, @enddate, GETDATE()); select @@identity";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@siteId", siteId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@startdate", start);
                    cmd.Parameters.AddWithValue("@enddate", end);

                    int confirmation = Convert.ToInt32(cmd.ExecuteScalar());
                    return confirmation;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
