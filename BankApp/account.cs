using System;

namespace Bank_App
{
    class account
    {

        public double interest = 1;
        public string owner;
        public string username;
        public int pin;
        public double loan = 0;
        public movement firstMovement;
        public account next;
        public account(string owner, string username, int pin, movement firstMovement)
        {
            this.owner = owner;
            this.username = username;
            this.pin = pin;
            this.firstMovement = firstMovement;
            Random random = new Random();
            this.interest = random.NextDouble() * 3;
        }

        public static account findAccountByUsername(account first, string username)
        {
            if (first.username == username)
                return first;
            else if (first.next != null)
                return findAccountByUsername(first.next, username);
            else
                return null;
        }

        public static account findAccountByName(account first, string name)
        {
            if (first.owner == name)
                return first;
            else if (first.next != null)
                return findAccountByName(first.next, name);
            else
                return null;
        }

        public static void addMovement(int amount, ref account acc)
        {
            movement pointer = acc.firstMovement;
            for (; pointer.next != null; pointer = pointer?.next) { };
            pointer.next = new movement(amount);
        }

        public static int getBalance(account acc)
        {
            int balance = 0;
            movement pointer = acc.firstMovement;
            for (; pointer != null; pointer = pointer.next) { balance += pointer.amount; }
            return balance;
        }

        ~account()
        {
            Console.WriteLine("Good bye {0}...", owner);
        }
    }

}
