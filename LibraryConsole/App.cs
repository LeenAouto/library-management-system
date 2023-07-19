using Library.ConsoleUI.Display;
using Library.DAL.Abstractions;
using Library.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library.ConsoleUI
{
    public class App : IApp
    {
        private readonly ILogger<App> _logger;
        //private readonly IConfiguration _config;
        private readonly IBookManager bookManager;
        private readonly IUserManager userManager;
        private readonly IReservationManager reservationManager;
        private readonly IPasswordHasher hasher;
        private readonly IDisplay display;

        public App(ILogger<App> logger, IBookManager bookManager, 
            IUserManager userManager, IReservationManager reservationManager,
            IPasswordHasher hasher, IDisplay display
            )
        {
            _logger = logger;
            //_config = config;
            this.bookManager = bookManager;
            this.userManager = userManager;
            this.reservationManager = reservationManager;
            this.hasher = hasher;
            this.display = display;
        }

        public void Run()
        {
            Console.WriteLine("Welcome to my basic Library Management System! (Part 3)\n**********************************");
            Console.WriteLine("Enter 1 to start the program, or any number to exit.");
            Console.Write("Your option: ");
            try
            {
                int option = Convert.ToInt32(Console.ReadLine());
                while (option == 1)
                {
                    Console.WriteLine("\nDo you want to...?");
                    Console.WriteLine("1- Sign in\n2- Sign up\n* Enter Any input to Exit *");
                    Console.Write("Your option: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    if (choice != 1 && choice != 2)
                    {
                        break;
                    }
                    else if (choice == 1) // Sign in
                    {
                        SignIn();
                    }
                    else // Sign up
                    {
                        SignUp();
                    }
                    Console.Write("\nDisplay Home Page Again?\n--- Yes (1)\n--- No (Enter any number).");
                    Console.Write("Your option: ");
                    option = Convert.ToInt32(Console.ReadLine());
                }
                Console.WriteLine("You have exited the program...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            //================================================================
            //for (int i = 0; i < 10; i++)
            //{
            //    _logger.LogInformation("Errorrr");
            //}

        }

        public void SignIn()
        {
            Console.WriteLine("\n******** SIGN IN PAGE ********\n");

            Console.Write("*** USERNAME : ");
            string username = Console.ReadLine().ToLower();

            User user = userManager.Get(username);
            if (user != null)
            {
                Console.Write("*** PASSWORD : ");
                string password = Console.ReadLine();

                if (hasher.Verify(user.Password, password))
                {

                    string userType = user.Type;

                    if (userType.Equals("student")) // student page
                    {
                        string name = string.Format("{0} {1}", user.FirstName.ToUpper(), user.LastName.ToUpper());
                        Console.WriteLine("\n**** WELCOME STUDENT '" + name + "' ****\n");
                        int exit = 0;
                        while (exit != 1)
                        {
                            Console.WriteLine("* PLEASE CHOOSE AN OPTION: ");
                            Console.WriteLine("1- DISPLAY BOOKS\n2- SEARCH FOR BOOKS\n3- RESERVE A BOOK");
                            Console.Write("* YOUR OPTION: ");
                            int option;
                            try
                            {
                                option = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                return;
                            }
                            
                            switch (option)
                            {
                                case 1:
                                    Console.WriteLine("\n******** BOOKS TABLE ********\n");
                                    List<Book> books = bookManager.GetList();
                                    if (books != null)
                                    {
                                        display.DisplayBook(books);
                                    }
                                    else
                                    {
                                        Console.WriteLine("NO BOOKS ARE FOUND");
                                    }
                                    Console.WriteLine("\n****************************************\n");
                                    break;
                                case 2:
                                    Console.WriteLine("\n******** SEARCH PAGE ********\n");
                                    Console.WriteLine("* SEARCH BY ID... ");
                                    Console.Write("*** BOOK ID: ");
                                    int id;
                                    try
                                    {
                                        id = Convert.ToInt32(Console.ReadLine());
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                        break;
                                    }
                                    Book book = bookManager.Get(id);
                                    if (book != null)
                                    {
                                        display.DisplayBook(book);
                                    }
                                    else
                                    {
                                        Console.WriteLine("NO BOOK WITH ID = " + id + " IS FOUND");
                                    }
                                    break;
                                case 3:
                                    Console.WriteLine("\n******** AVAILABLE BOOKS TABLE ********\n");
                                    List<Book> booksAvailable = bookManager.GetList();
                                    if (booksAvailable != null)
                                    {
                                        display.DisplayBook(booksAvailable);
                                    }
                                    else
                                    {
                                        Console.WriteLine("NO BOOKS ARE AVAILABLE FOR RESERVATION");
                                        break;
                                    }
                                    Console.WriteLine("\n****************************************\n");

                                    Console.Write("*** BOOK ID : ");
                                    int bookId;

                                    try
                                    {
                                        bookId = Convert.ToInt32(Console.ReadLine());
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                        break;
                                    }

                                    Book bookToReserve = bookManager.Get(bookId);
                                    if (bookToReserve == null)
                                    {
                                        Console.WriteLine("*** NOT FOUND !!! ***");
                                        break;
                                    }

                                    Reservation reservation = new Reservation()
                                    {
                                        BookId = bookId,
                                        UserId = user.Id,
                                        Username = user.Username,
                                        StartDate = DateTime.Now,
                                        IsReturned = false
                                    };

                                    try
                                    {
                                        Reservation addedReservation = reservationManager.Add(reservation);
                                        bookToReserve.IsAvailable = false;
                                        bookManager.Update(bookToReserve);
                                        Console.WriteLine("\n*** YOUR RESERVATION HAS BEEN PLACED SUCCESSFULLY (Id = "
                                                + addedReservation.Id + ") ***\n");
                                        
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\n*** SOMETHING WENT WRONG ***\n");
                                        _logger.LogError(e.StackTrace);
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Invalid option!");
                                    break;
                            }
                            Console.Write("\nDo you want to exit?\n--- Yes (1)\n--- No (Enter any number).");
                            Console.Write("Your option: ");
                            try
                            {
                                exit = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                return;
                            }
                        }
                    }
                    else if (userType.Equals("staff")) // staff page
                    {
                        string name = string.Format("{0} {1}", user.FirstName.ToUpper(), user.LastName.ToUpper());
                        Console.WriteLine("\n**** WELCOME STAFF '" + name + "' ****\n");
                        int exit = 0;
                        while (exit != 1)
                        {
                            Console.WriteLine("* PLEASE CHOOSE AN OPTION: ");
                            Console.WriteLine("1- ADD A BOOK\n2- REMOVE A BOOK\n3- DISPLAY RESERVATIONS\n" +
                                "4- UPDATE A RESERVATION\n5- REMOVE A USER");
                            Console.Write("* YOUR OPTION: ");
                            int option;
                            try
                            {
                                option = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                return;
                            }

                            switch (option)
                            {
                                case 1:
                                    Console.WriteLine("\n******** ADD BOOK PAGE ********\n");

                                    Book book = new Book();

                                    Console.Write("*** TITLE : ");
                                    book.Title = Console.ReadLine();
                                    if (book.Title.Length == 0)
                                    {
                                        Console.WriteLine("You have to insert the book's title!\nsat to - by default.");
                                        book.Title = "-";
                                    }
                                    Console.Write("*** AUTHOR: ");
                                    book.Author = Console.ReadLine();
                                    if (book.Author.Length == 0)
                                    {
                                        Console.WriteLine("You have to insert the book's author!\nsat to - by default.");
                                        book.Author = "-";
                                    }
                                    Console.Write("*** PUBLISHER: ");
                                    book.Publisher = Console.ReadLine();
                                    if (book.Publisher.Length == 0)
                                    {
                                        Console.WriteLine("You have to insert the book's publisher!\nsat to - by default.");
                                        book.Publisher = "-";
                                    }
                                    Console.Write("*** PUBLICATION YEAR : ");
                                    book.PublishYear = Convert.ToInt32(Console.ReadLine());
                                    if (book.PublishYear < 1 || book.PublishYear > DateTime.Now.Year)
                                    {
                                        Console.WriteLine("You have to insert the book's publishYear!\nsat to null by default.");
                                        book.PublishYear = null;
                                    }

                                    book.IsAvailable = true;

                                    try
                                    {
                                        Book addedBook = bookManager.Add(book);
                                        Console.WriteLine("\n*** THE BOOK HAS BEEN ADDED SUCCESSFULLY (Id = " +
                                                addedBook.Id + ") ***\n");
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\n*** SOMETHING WENT WRONG ***\n");
                                        _logger.LogError(e.StackTrace);

                                    }
                                    break;
                                case 2:
                                    Console.WriteLine("\n******** REMOVE BOOK PAGE ********\n");
                                    Console.WriteLine("\n******** BOOKS TABLE ********\n");
                                    List<Book> books = bookManager.GetList();
                                    if (books != null)
                                    {
                                        display.DisplayBook(books);
                                    }
                                    else
                                    {
                                        Console.WriteLine("NO BOOKS ARE FOUND");
                                    }
                                    Console.WriteLine("\n****************************************\n");

                                    Console.Write("*** BOOK ID : ");
                                    int bid;

                                    try
                                    {
                                        bid = Convert.ToInt32(Console.ReadLine());
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                        break;
                                    }


                                    Book bookToDelete = bookManager.Get(bid);
                                    if (bookToDelete == null)
                                    {
                                        Console.WriteLine("*** NOT FOUND !!! ***");
                                        break;
                                    }

                                    try
                                    {
                                        if (bookManager.Delete(bookToDelete))
                                        {
                                            Console.WriteLine("\n*** THE BOOK HAS BEEN REMOVED SUCCESSFULLY ***\n");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\n*** SOMETHING WENT WRONG ***\n");
                                        _logger.LogError(e.StackTrace);
                                    }
                                    break;
                                case 3:
                                    Console.WriteLine("\n******** RESERVATIONS TABLE ********\n");
                                    List<Reservation> reservations = reservationManager.GetList();

                                    if (reservations != null)
                                    {
                                        display.DisplayReservation(reservations);
                                    }
                                    else
                                    {
                                        Console.WriteLine("NO RESERVATIONS ARE FOUND");
                                    }
                                    Console.WriteLine("\n****************************************\n");
                                    break;
                                case 4:
                                    Console.WriteLine("\n******** UPDATE A RESERVATION ********\n");

                                    Console.Write("*** RESERVATION ID: ");
                                    int rid;
                                    try
                                    {
                                        rid = Convert.ToInt32(Console.ReadLine());
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                        break;
                                    }

                                    Reservation reservationToUpdate = reservationManager.Get(rid);

                                    if (reservationToUpdate == null)
                                    {
                                        Console.WriteLine("*** NOT FOUND !!! ***");
                                        break;
                                    }

                                    Book bookToReserve = bookManager.Get(reservationToUpdate.BookId);

                                    if (reservationToUpdate.IsReturned)
                                    {
                                        reservationToUpdate.IsReturned = false;
                                        bookToReserve.IsAvailable = false;
                                    }
                                    else
                                    {
                                        reservationToUpdate.IsReturned = true;
                                        bookToReserve.IsAvailable = true;
                                    }

                                    try
                                    {
                                        Reservation updatedReservation = reservationManager.Update(reservationToUpdate);
                                        bookManager.Update(bookToReserve);
                                        Console.WriteLine("\n*** THE RESERVATION HAS BEEN UPDATED SUCCESSFULLY ***\n");
                                    }
                                    catch (Exception e)
                                    {

                                        Console.WriteLine("\n*** SOMETHING WENT WRONG ***\n");
                                        _logger.LogError(e.StackTrace);
                                    }
                                    break;
                                case 5:
                                    Console.WriteLine("\n******** REMOVE USER PAGE ********\n");

                                    Console.Write("*** USERNAME : ");
                                    string rusername = Console.ReadLine().ToLower();

                                    User userToDelete = userManager.Get(rusername);
                                    if (userToDelete == null)
                                    {
                                        Console.WriteLine("*** NOT FOUND !!! ***");
                                        break;
                                    }

                                    try
                                    {
                                        if (userManager.Delete(userToDelete))
                                        {
                                            Console.WriteLine("\n*** THE USER HAS BEEN REMOVED SUCCESSFULLY ***\n");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\n*** SOMETHING WENT WRONG ***\n");
                                        _logger.LogError(e.StackTrace);
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Invalid option!");
                                    break;
                            }
                            Console.Write("\nDo you want to exit?\n--- Yes (1)\n--- No (Enter any number).");
                            Console.Write("Your option: ");
                            try
                            {
                                exit = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                                return;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("*** SOMETHING WENT WRONG !***");
                    }
                }
                else
                {
                    Console.WriteLine("*** INVALID PASSWORD !***");
                }
            }
            else
            {
                Console.WriteLine("*** INVALID USERNAME !***");
            }

        }
        public void SignUp()
        {
            Console.WriteLine("\n******** SIGN UP PAGE ********\n");

            User user = new User();

            Console.Write("*** USERNAME : ");
            user.Username = Console.ReadLine().ToLower();
            if (user.Username.Length == 0)
            {
                Console.WriteLine("You have to enter a username!");
                return;
            }

            Console.Write("*** PASSWORD : ");
            string inputPassword = Console.ReadLine();
            if (inputPassword.Length == 0)
            {
                Console.WriteLine("You have to enter a password!");
                return;
            }
            user.Password = hasher.Hash(inputPassword);

            Console.Write("*** FIRST NAME : ");
            user.FirstName = Console.ReadLine();
            //if (user.FirstName.Length == 0)
            //{
            //    Console.WriteLine("You have to enter a first name!\n set to - by default.");
            //    user.FirstName = "-";
            //}

            Console.Write("*** LAST NAME : ");
            user.LastName = Console.ReadLine();
            if (user.LastName.Length == 0)
            {
                Console.WriteLine("You have to enter a last name!\n set to - by default.");
                user.FirstName = "-";
            }

            Console.Write("*** TYPE OF USER (1) Student (2) Staff: ");
            int input = Convert.ToInt32(Console.ReadLine());
            if (input != 1 && input != 2)
            {
                Console.WriteLine("*** INVALID OPTION!! Your account is set to Student type by default. ***");
                user.Type = "student";
            }
            else if (input == 1)
            {
                user.Type = "student";
            }
            else
            {
                user.Type = "staff";
            }

            Console.WriteLine("*** BIRTH DATE : ");
            Console.Write("* YEAR : ");
            int year = Convert.ToInt32(Console.ReadLine());
            if (year + 18 > DateTime.Now.Year || year < DateTime.Now.Year - 120)
            {
                Console.WriteLine("You have to be older than 18!");
                return;
            }
            Console.Write("* MONTH : ");
            int month = Convert.ToInt32(Console.ReadLine());
            if (month < 1 || month > 12)
            {
                Console.WriteLine("You have to enter a valid month!\n Set to January by default");
                month = 1;
            }
            Console.Write("* DAY : ");
            int day = Convert.ToInt32(Console.ReadLine());
            if (DateTime.IsLeapYear(year) && month == 2)
            {
                if (day < 1 || day > 29)
                {
                    Console.WriteLine("You have to enter a valid day!\n Set to 1st by default");
                    day = 1;
                }
            }
            else if (month == 2)
            {
                if (day < 1 || day > 28)
                {
                    Console.WriteLine("You have to enter a valid day!\n Set to 1st by default");
                    day = 1;
                }
            }

            if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                if (day < 1 || day > 30)
                {
                    Console.WriteLine("You have to enter a valid day!\n Set to 1st by default");
                    day = 1;
                }
            }
            else
            {
                if (day < 1 || day > 31)
                {
                    Console.WriteLine("You have to enter a valid day!\n Set to 1st by default");
                    day = 1;
                }
            }
            user.BirthDate = new DateTime(year, month, day);

            Console.Write("*** PHONE NUMBER : ");
            user.Phone = Console.ReadLine();

            if (!(Regex.IsMatch(user.Phone, @"^0+5+\d{8}$")))
            {
                user.Phone = "";
                Console.WriteLine("*** INVALID PHONE NUMBER!! Set to null by default. ***");

            }

            try
            {
                User addedUser = userManager.Add(user);
                Console.WriteLine("\n*** YOUR ACCOUNT HAS BEEN CREATED SUCCESSFULLY (Id = "
                    + addedUser.Id + ") ***\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n*** SOMETHING WENT WRONG, PLEASE USE A UNIQUE USERNAME ***\n");
                _logger.LogError(ex.StackTrace);

            }
        }



        //MAYBE I SPLIT EACH PAGE INTO A SEPARATE METHOD?
        /*
        private void StudentPage(User user)
        {

        }

        private void StaffPage()
        {

        }
        */
    }
}
