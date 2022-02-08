using System;

namespace Bank_App
{
    class Program
    {

        //Helpers
        static string toFixed(double num)//Noramilizing doubles
        {
            string numStr = num + "";
            return numStr.IndexOf('.') == -1 ? numStr : numStr.Substring(0, numStr.IndexOf('.') + 2);
        }

        //ADMIN ACCESS
        static void showAdmin(account firstAccount, account currentAccount = null)
        {
            Console.Clear();
            Console.WriteLine("Welcome Menachem Lev, Admin");
            int accountIndex = 1;
            for (account pointer = firstAccount; pointer != null; pointer = pointer.next)
            {
                string movements = "";
                int movementIndex = 1;
                movement movPointer = pointer.firstMovement;
                for (; movPointer != null; movPointer = movPointer.next)
                {
                    movements += movementIndex++ + ")" + movPointer.amount + " ";
                }
                Console.WriteLine("\nAccount {0}: Owner name:{1}, username:{2} ,PIN:{3}, movements: {4}", accountIndex++, pointer.owner, pointer.username, pointer.pin, movements);
            }





            Console.WriteLine("\nPress any key to get out");
            Console.ReadKey();
            menu(firstAccount, currentAccount);
        }

        //CHECKING LOGIN
        static bool loginToYourBankAccount(account firstAccount, account currentAccount = null)
        {
            string username;
            int pin;
            bool rightPin = false; ;
            do
            {
                Console.WriteLine("\nType your username:(type -1 to get out)");
                username = Console.ReadLine().Trim();
                if (username == "-1")
                {
                    menu(firstAccount, currentAccount);
                    return false;
                }
                if (username == "MenachemLev_Admin")
                {
                    showAdmin(firstAccount);
                    return false;
                }
                currentAccount = account.findAccountByUsername(firstAccount, username);
                Console.WriteLine("\nType your password:");
                rightPin = int.TryParse(Console.ReadLine(), out pin) && pin == currentAccount?.pin;
                if (!rightPin)
                {
                    if (pin == -1)
                    {
                        currentAccount = null;
                        menu(firstAccount, currentAccount);
                        return false;
                    }
                    Console.WriteLine("Wrong username or pin. Try again");
                    currentAccount = null;
                }

            } while (!rightPin);
            welcomeUser(firstAccount, currentAccount);
            return true;

        }

        //WELCOMING USER AFTER LOGIN
        static void welcomeUser(account firstAccount, account currentAccount = null)
        {

            Console.Clear();
            Console.WriteLine("You're welcome, {0}", currentAccount.owner);
            Console.WriteLine("What would you like to do? \n1)Show movements \n2)Get loan \n3)Send money to a friend \n4)Show loan and interest\n5)Deposit\n6)Pay loan \n7)Logout \n8 Delete your account");
            int ans;
            bool validAnswer = int.TryParse(Console.ReadLine(), out ans) && ans >= 1 && ans <= 8;
            while (!validAnswer)
            {
                Console.WriteLine("Invalid choice! choose again! 1,2,3,4,5,6,7 or 8");
                validAnswer = int.TryParse(Console.ReadLine(), out ans) && ans >= 1 && ans <= 8;
            }
            Console.Clear();
            if (ans == 1) { printMovements(firstAccount, currentAccount); }
            else if (ans == 2)
            {
                getLoan(firstAccount, currentAccount);
            }
            else if (ans == 3)
            {

                sendMoney(firstAccount, currentAccount);
            }
            else if (ans == 4)
            {
                showLoan(firstAccount, currentAccount);
            }
            else if (ans == 5)
            {
                deposit(firstAccount, currentAccount);
            }
            else if (ans == 6)
            {
                payLoan(firstAccount, currentAccount);
            }
            else if (ans == 7)
            {
                menu(firstAccount, currentAccount);
            }
            else
            {
                deleteAccount(firstAccount, currentAccount);
            }

        }

        //Back to welcome menu or main menu
        static void backToMenu(account firstAccount, account currentAccount = null)
        {
            Console.WriteLine("\nPress 1 to get back to user menu. press 2 to exit to main menu");
            int ans;
            bool validAnswer = int.TryParse(Console.ReadLine(), out ans) && ans >= 1 && ans <= 2;
            while (!validAnswer)
            {
                Console.WriteLine("Invalid choice! choose again! 1 or 2");
                validAnswer = int.TryParse(Console.ReadLine(), out ans) && ans >= 1 && ans <= 2;
            }
            if (ans == 1)
            {

                welcomeUser(firstAccount, currentAccount);
            }
            else
            {
                menu(firstAccount, currentAccount);
            }
        }

        //SHOW LOANS
        static void showLoan(account firstAccount, account currentAccount = null)
        {
            double loan = currentAccount.loan;
            double interest = currentAccount.interest * currentAccount.loan * 0.01;
            Console.WriteLine("Your loan is {0}$. You interest is {1}$. You are in debt of {2}$", toFixed(loan), toFixed(interest), toFixed((double)loan + interest));
            backToMenu(firstAccount, currentAccount);

        }


        //GET LOAN FROM BANK
        static bool getLoan(account firstAccount, account currentAccount = null)
        {
            Console.WriteLine("How much money you want to loan(max 10 times balance)?");
            int amount;
            bool validAmount = int.TryParse(Console.ReadLine(), out amount) && amount > 0;
            while (!validAmount || amount > account.getBalance(currentAccount) * 10)
            {
                if (amount == -1)
                {

                    welcomeUser(firstAccount, currentAccount);
                    return false;
                }
                if (!validAmount)
                {
                    Console.WriteLine("Invalid amount! Loan above zero dollars!(Type -1 to cancel)");
                }
                if (amount > account.getBalance(currentAccount) * 10)
                {
                    Console.WriteLine("You can't loan that much! Your balance is {0}$. Loan differend amount(Type -1 to cancel)", account.getBalance(currentAccount));
                }
                validAmount = int.TryParse(Console.ReadLine(), out amount) && amount > 0;
            }

            account.addMovement(amount, ref currentAccount);
            currentAccount.loan += amount;

            Console.WriteLine("Operation completed! You've loaned {0}$", amount);


            backToMenu(firstAccount, currentAccount);

            return true;
        }

        //Pay loan
        static bool payLoan(account firstAccount, account currentAccount = null)
        {
            double totalLoan = currentAccount.loan + currentAccount.interest * currentAccount.loan * 0.01;
            if (totalLoan <= 0)
            {
                Console.WriteLine("You have no loan to pay. You are back to user menu. Press any key to continue:");
                Console.ReadKey();
                welcomeUser(firstAccount, currentAccount);
                return false;
            }
            Console.WriteLine("How much would you like to pay of your loan(in $)?");
            int amount;
            bool validAmount = int.TryParse(Console.ReadLine(), out amount) && amount > 0;
            while (!validAmount || amount > account.getBalance(currentAccount))
            {
                if (amount == -1)
                {

                    welcomeUser(firstAccount, currentAccount);
                    return false;
                }
                if (!validAmount)
                {
                    Console.WriteLine("Invalid amount! Pay above zero dollars!(Type -1 to cancel)");
                }
                if (amount > account.getBalance(currentAccount))
                {
                    Console.WriteLine("You can't pay that much! Your balance is {0}$. Pay differend amount(Type -1 to cancel)", account.getBalance(currentAccount));
                }
                validAmount = int.TryParse(Console.ReadLine(), out amount) && amount > 0;
            }

            if (amount > totalLoan)
            {
                Console.WriteLine("Ok. For your loan is only {0}$. You'll pay that much instead of {1}$. Thank you", toFixed(totalLoan), amount);
                amount = (int)Math.Ceiling(totalLoan);//Amount is limited to total loan
            }

            account.addMovement(-amount, ref currentAccount);
            currentAccount.loan = Math.Ceiling(totalLoan) - amount;
            Console.WriteLine("Operation completed! You've paid {0}$ or your loan. Your total remaining loan is {1}$", amount, toFixed(Math.Ceiling(totalLoan) - amount));
            backToMenu(firstAccount, currentAccount);
            return true;

        }

        //Deposit
        static bool deposit(account firstAccount, account currentAccount = null)
        {
            Console.WriteLine("How much money you want to deposit?(max 2000$)");
            int amount;
            bool validAmount = int.TryParse(Console.ReadLine(), out amount) && amount > 0;
            while (!validAmount || amount > 2000)
            {
                if (amount == -1)
                {

                    welcomeUser(firstAccount, currentAccount);
                    return false;
                }
                if (!validAmount)
                {
                    Console.WriteLine("Invalid amount! Deposit above zero dollars!(Type -1 to cancel)");
                }
                if (amount > 2000)
                {
                    Console.WriteLine("You can't deposit that much! Deposit lower amount(Type -1 to cancel)");
                }
                validAmount = int.TryParse(Console.ReadLine(), out amount) && amount > 0;
            }

            account.addMovement(amount, ref currentAccount);
            Console.WriteLine("Operation completed! You've deposited {0}$", amount);
            backToMenu(firstAccount, currentAccount);
            return true;
        }

        //TRANSFER
        static bool sendMoney(account firstAccount, account currentAccount = null)
        {
            Console.WriteLine("Who would like send money to? by name");
            string input = Console.ReadLine();
            account receiver = account.findAccountByName(firstAccount, input);
            while (receiver == null)
            {
                Console.WriteLine("OOps. Wrong name. Try entering it again:");
                input = Console.ReadLine();
                receiver = account.findAccountByName(firstAccount, input);
            }

            Console.WriteLine("Ok. We got it. How much would you like to send?");
            int amount;
            bool validAmount = int.TryParse(Console.ReadLine(), out amount) && amount > 0;
            while (!validAmount || amount > account.getBalance(currentAccount))
            {
                if (amount == -1)
                {

                    welcomeUser(firstAccount, currentAccount);
                    return false;
                }
                if (!validAmount)
                {
                    Console.WriteLine("Invalid amount! Send above zero dollars!(Type -1 to cancel)");
                }
                if (amount > account.getBalance(currentAccount))
                {
                    Console.WriteLine("You can't send that much! Your balance is {0}$. Send differend amount(Type -1 to cancel)", account.getBalance(currentAccount));
                }
                validAmount = int.TryParse(Console.ReadLine(), out amount) && amount > 0;
            }

            //Operate transfer
            account.addMovement(amount, ref receiver);
            account.addMovement(-amount, ref currentAccount);

            Console.WriteLine("Operation completed! You've transfered {0}$ to {1}", amount, input);
            backToMenu(firstAccount, currentAccount);
            return true;
        }

        //Delete account
        static void deleteAccount(account firstAccount, account currentAccount = null)
        {
            account pointer = firstAccount;
            if (pointer == currentAccount) menu(firstAccount.next, null);
            else
            {
                while (pointer.next != currentAccount) pointer = pointer.next;
                pointer.next = pointer.next?.next;
                menu(firstAccount, null);

            }
        }

        //PRINT MOVEMENTS
        static void printMovements(account firstAccount, account currentAccount = null)
        {
            movement.printMovements(currentAccount.firstMovement);
            backToMenu(firstAccount, currentAccount);

        }

        //OPENING BANK ACCOUNT
        static void openBankAccount(ref account firstAccount, account currentAccount = null)
        {
            string username, owner;
            int pin;
            bool goodPin = false;
            Console.Clear();
            Console.WriteLine("Ok. Let's open your bank account:");
            Console.WriteLine("\nEnter your full name:");
            owner = Console.ReadLine();
            Console.WriteLine("\nEnter your username:");
            username = Console.ReadLine().Trim();
            while (username == "-1")
            {
                Console.WriteLine("Invalid username. try again");
                username = Console.ReadLine().Trim();
            }

            Console.WriteLine("\nEnter your PIN(must be a positive integer(1,2,3,4...)):");
            goodPin = int.TryParse(Console.ReadLine(), out pin) && pin > 0;
            while (!goodPin)
            {
                if (pin <= 0)
                {
                    Console.WriteLine("PIN must be a positive integer(1,2,3,4...)");
                }
                Console.WriteLine("PIN is in valid. Try again:");
                goodPin = int.TryParse(Console.ReadLine(), out pin);
            }

            account pointer = firstAccount;
            for (; pointer.next != null; pointer = pointer.next) ;
            pointer.next = new account(owner, username, pin, movement.arrayToMovements(new int[] { 100 }));
            Console.WriteLine("Ok. Account has been created. And you got 100$ as a gift :)");
            Console.WriteLine("\nPress 1 to get back to main menu. press 2 to exit");
            int ans;
            bool validAnswer = int.TryParse(Console.ReadLine(), out ans) && ans >= 1 && ans <= 2;
            while (!validAnswer)
            {
                Console.WriteLine("Invalid choice! choose again! 1 or 2");
                validAnswer = int.TryParse(Console.ReadLine(), out ans) && ans >= 1 && ans <= 2;
            }
            if (ans == 1)
            {

                menu(firstAccount, currentAccount);
            }
            else
            {
                Console.WriteLine("Bye!");
            }
        }

        //BEGIN _ MAIN MENU
        static void menu(account firstAccount, account currentAccount = null)
        {
            int ans;
            Console.Clear();
            Console.WriteLine("Hello! What would you like to do? \n1)Login in to your bank account \n2)Open a new bank account \n3)Exit program");
            bool validAnswer = int.TryParse(Console.ReadLine(), out ans) && ans >= 1 && ans <= 3;
            while (!validAnswer)
            {
                Console.WriteLine("Invalid choice! choose again! 1,2 or 3");
                validAnswer = int.TryParse(Console.ReadLine(), out ans) && ans >= 1 && ans <= 3;
            }
            if (ans == 1)
                loginToYourBankAccount(firstAccount, currentAccount);
            else if (ans == 2)
                openBankAccount(ref firstAccount, currentAccount);
            else;

        }

        //PROGRAM
        static void Main(string[] args)
        {
            account account1 = new account("Menachem Lev", "ml", 1111, movement.arrayToMovements(new int[] { 200, 450, -400, 3000, -650, -130, 70, 1300 }));
            account account2 = new account("Yeuda Lev", "yl", 2222, movement.arrayToMovements(new int[] { 5000, 3400, -150, -790, -3210, -1000, 8500, -30 }));
            account account3 = new account("Noah Lev", "nl", 3333, movement.arrayToMovements(new int[] { 200, -200, 340, -300, -20, 50, 400, -460 }));
            account account4 = new account("Dvora Lev", "dl", 4444, movement.arrayToMovements(new int[] { 430, 1000, 700, 50, 90, -300 }));

            account1.next = account2;
            account2.next = account3;
            account3.next = account4;
            account currentAccount = null;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            menu(account1, currentAccount);
        }
    }
}
