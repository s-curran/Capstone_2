using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{
    class SubMenu3 : CLIMenu
    {
        private IParkDAO ParkDAO;
        private ICampgroundDAO CampgroundDAO;
        private ISiteDAO SiteDAO;
        private IReservationDAO ReservationDAO;

        // Store any private variables, including DAOs here....
        public Park park;

        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public SubMenu3(Park park, IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO) :
            base("Sub-Menu 2")
        {
            this.park = park;
            this.ParkDAO = parkDAO;
            this.CampgroundDAO = campgroundDAO;
            this.SiteDAO = siteDAO;
            this.ReservationDAO = reservationDAO;

        }

        protected override void SetMenuOptions()
        {

            foreach (Campground cg in CampgroundDAO.GetCampgrounds(park.ParkId))
            {
                this.menuOptions.Add($"{cg.CampgroundId}", $"{cg.Name,-25}{cg.OpenFrom,-10}{cg.OpenTo,-10}{cg.DailyFee,-10:C}");
                //Console.WriteLine($"{cg.CampgroundId,-10} {cg.Name,10} {cg.OpenFrom,-10} {cg.OpenTo,-10} {cg.DailyFee,10}");
            }
            this.menuOptions.Add("", ""); //Add in an empty string for format purposes
            this.menuOptions.Add("B", "Return to Previous Screen");
            this.quitKey = "B";
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {
            if (menuOptions.ContainsKey(choice))
            {
                if (choice == "0")
                {
                    return false;
                }
                    int intChoice = int.Parse(choice);
                DateTime startDate = GetDate("What is the arrival date?");
                DateTime endDate = GetDate("What is the departure date?");

                IList<Campground> cg = CampgroundDAO.GetCampgrounds(park.ParkId);
                Campground camp = new Campground();
                foreach (Campground c in cg)
                {
                    if (c.CampgroundId == intChoice)
                    {
                        camp = c;
                    }
                }
                decimal fee = camp.DailyFee * Convert.ToDecimal((endDate - startDate).TotalDays);

                IList<Site> sites = SiteDAO.AvailableSites(intChoice, startDate, endDate);
                if (sites.Count == 0)
                {
                    string answer = GetString("There are no available sites in that date range, would you like to enter an alternate date range? (Y/N)");
                    if (answer == "Y")
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"{"Site No.", -10}{"Max Occup.", -15}{"Accessible", -15}{"Max RV Length", -15}{"Utility", -15}{"Cost", -10}");
                    Console.WriteLine("-----------------------------------------------------------------------------");

                    foreach (Site site in sites)
                    {

                        Console.WriteLine($"{site.SiteNumber, -10}{site.MaxOccupancy, -15}{site.IsAccessible, -15}{site.MaxRVLength, -15}{site.HasUtilities, -15}{fee, -10:C}");
                    }

                    Console.WriteLine();
                    int site1 = GetInteger("Which site should be reserved? (enter 0 to cancel)");
                    if (site1 == 0)
                    {
                        return false;
                    }
                    string name = GetString("What name should the reservation be made under?");
                    int conf = ReservationDAO.BookReservation(name, site1, startDate, endDate);
                    Console.WriteLine();
                    Console.WriteLine($"The reservation has been made and the confirmation id is {conf}.");
                    Console.ReadLine();
                }
            
                
            }
            
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
            Console.WriteLine($"{park.Name} Campgrounds");
            Console.WriteLine();
            Console.WriteLine($"{"",-1} {"Name",-25}{"Open",-10}{"Close",-10}{"Daily Fee",-10}");
            Console.WriteLine("--------------------------------------------------------------------");

        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("Display some data here, AFTER the sub-menu is shown....");
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.DarkYellow);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Reservations"));
            ResetColor();
        }
    }
}
