using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.DAL
{
    public interface ISiteDAO
    {
        IList<Site> AvailableSites(int campground, DateTime start, DateTime end);
    }
}