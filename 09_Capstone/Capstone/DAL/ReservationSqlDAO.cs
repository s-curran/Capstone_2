using System;
using System.Collections.Generic;
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

    }
}
