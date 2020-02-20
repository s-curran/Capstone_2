using System;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        int BookReservation(string name, int siteId, DateTime start, DateTime end);
    }
}