using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{
    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class MainMenu : CLIMenu
    {
        // DAOs - Interfaces to our data objects can be stored here...
        private IParkDAO ParkDAO;
        private ICampgroundDAO CampgroundDAO;
        private ISiteDAO SiteDAO;
        private IReservationDAO ReservationDAO;

        /// <summary>
        /// Constructor adds items to the top-level menu. YOu will likely have parameters for one or more DAO's here...
        /// </summary>
        public MainMenu(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO) : base("Main Menu")
        {
            this.ParkDAO = parkDAO;
            this.CampgroundDAO = campgroundDAO;
            this.SiteDAO = siteDAO;
            this.ReservationDAO = reservationDAO;
        }

        protected override void SetMenuOptions()
        {
            int counter = 1;
            foreach (Park park in ParkDAO.GetAllParks())
            {
                this.menuOptions.Add($"{counter}", park.Name);
                counter++;
            }

            this.menuOptions.Add("Q", "Quit program");
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
                string name = menuOptions[choice];
                Park park = ParkDAO.GetPark(name);

                SubMenu1 subMenu1 = new SubMenu1(park);
                subMenu1.Run();
                return true;
            }
            //switch (choice)
            //{
            //    case "1": // Do whatever option 1 is
            //        int i1 = GetInteger("Enter the first integer: ");
            //        int i2 = GetInteger("Enter the second integer: ");
            //        Console.WriteLine($"{i1} + {i2} = {i1+i2}");
            //        Pause("Press enter to continue");
            //        return true;    // Keep running the main menu
            //    case "2": // Do whatever option 2 is
            //        WriteError("Not yet implemented");
            //        Pause("");
            //        return true;    // Keep running the main menu
            //    case "3": // Create and show the sub-menu
            //        SubMenu1 sm = new SubMenu1();
            //        sm.Run();
            //        return true;    // Keep running the main menu
            //}
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }


        private void PrintHeader()
        {
            SetColor(ConsoleColor.Yellow);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("My Program"));
            ResetColor();
        }
    }
}
