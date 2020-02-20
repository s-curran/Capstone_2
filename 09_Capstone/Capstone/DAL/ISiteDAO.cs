using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.DAL
{
    public interface ISiteDAO
    {
        IDictionary<Site, decimal> AvailableSites(string campground, DateTime start, DateTime end);
    }
}