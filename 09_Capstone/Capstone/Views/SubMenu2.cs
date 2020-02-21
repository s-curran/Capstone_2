using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{
    public class SubMenu2 : CLIMenu
    {
        private IParkDAO ParkDAO;
        private ICampgroundDAO CampgroundDAO;
        private ISiteDAO SiteDAO;
        private IReservationDAO ReservationDAO;

        // Store any private variables, including DAOs here....
        public Park park;

        public IDictionary<int, string> months = new Dictionary<int, string>()
        {
            {1, "January"},
            {2, "February"},
            {3, "March"},
            {4, "April"},
            {5, "May"},
            {6, "June"},
            {7, "July"},
            {8, "August"},
            {9, "September"},
            {10, "October"},
            {11, "November"},
            {12, "December"}
        };

        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public SubMenu2(Park park, IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO) :
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
            this.menuOptions.Add("1", "Search for Available Reservation");
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
            switch (choice)
            {
                case "1": // Do whatever option 1 is
                    SubMenu3 subMenu3 = new SubMenu3(park, ParkDAO, CampgroundDAO, SiteDAO, ReservationDAO);
                    subMenu3.Run();
                    Pause("");
                    return true;
                case "2": // Do whatever option 2 is
                    WriteError("Not yet implemented");
                    Pause("");
                    return false;
            }
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
            Console.WriteLine($"{park.Name} Campgrounds");
            Console.WriteLine();
            Console.WriteLine($"{"", -10} {"Name", -25}{"Open", -10}{"Close", -10}{"Daily Fee", -10}");
            Console.WriteLine("--------------------------------------------------------------------");
            foreach(Campground cg in CampgroundDAO.GetCampgrounds(park.ParkId))
            {
                Console.WriteLine($"#{cg.CampgroundId, -10}{cg.Name, -25}{months[cg.OpenFrom], -10}{months[cg.OpenTo], -10}{cg.DailyFee, -10:C}");
            }
            Console.WriteLine();


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
            SetColor(ConsoleColor.DarkBlue);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Campgrounds"));
            ResetColor();
        }

    }
}
