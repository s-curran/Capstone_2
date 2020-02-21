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
                this.menuOptions.Add($"{cg.CampgroundId}", $"{cg.Name}{cg.OpenFrom}{cg.OpenTo}{cg.DailyFee}");
                //Console.WriteLine($"{cg.CampgroundId,-10} {cg.Name,10} {cg.OpenFrom,-10} {cg.OpenTo,-10} {cg.DailyFee,10}");
            }
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

                Console.WriteLine($"{"Site No."}{"Max Occup."}{"Accessible"}{"Max RV Length"}{"Utility"}{"Cost"}");
                foreach (Site site in sites)
                {

                    Console.WriteLine($"{site.SiteNumber}  {site.MaxOccupancy}  {site.IsAccessible}  {site.MaxRVLength}  {site.HasUtilities}  {fee:C}");
                }

                Console.WriteLine();
                int site1 = GetInteger("Which site should be reserved? (enter 0 to cancel)");
                string name = GetString("What name should the reservation be made under?");
                int conf = ReservationDAO.BookReservation(name, site1, startDate, endDate);
                Console.WriteLine();
                Console.WriteLine($"The reservation has been made and the confirmation id is {conf}.");
                Console.ReadLine();
            }
            else if (choice == "0")
            {
                return false;
            }
            //switch (choice)
            //{
            //    case "1": // Do whatever option 1 is
            //        CampgroundDAO.GetCampgrounds(park.ParkId);
            //        Pause("");
            //        return true;
            //    case "2": // Do whatever option 2 is
            //        WriteError("Not yet implemented");
            //        Pause("");
            //        return false;
            //}
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
            Console.WriteLine($"{park.Name} Campgrounds");
            Console.WriteLine();
            Console.WriteLine($"Name    Open    Close   Daily Fee");

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
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Sub-Menu 2"));
            ResetColor();
        }
    }
}
