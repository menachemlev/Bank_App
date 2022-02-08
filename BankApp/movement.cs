using System;

namespace Bank_App
{
    public class movement
    {
        public int amount;
        public movement next;

        public movement()
        {
        }

        public movement(int amount)
        {
            this.amount = amount;
        }

        public static movement arrayToMovements(int[] movements)
        {
            movement firstMovement = new movement(movements[0]);
            movement pointer = firstMovement;
            for (int i = 1; i < movements.Length; i++)
            {
                pointer.next = new movement(movements[i]);
                pointer = pointer.next;
            }
            return firstMovement;
        }
        public static void printMovements(movement first)
        {
            movement pointer;
            int i = 1;
            string movements = "";
            int balance = 0, sumIn = 0, sumOut = 0;
            for (pointer = first; pointer != null; pointer = pointer.next)
            {
                int amount = pointer.amount;
                balance += amount;
                string action;
                if (amount >= 0)
                {
                    action = "deposited";
                    sumIn += amount;
                }
                else
                {
                    action = "withdrew";
                    sumOut += amount;
                }

                movements = "\t" + (i++ + ": You've " + action + " " + Math.Abs(amount) + "$\n") + movements;
            }
            Console.WriteLine("Your balance is {0}$\nYou've deposited {1}$\nYou've withdrew {2}$\nYour movement are:\n{3}", balance, sumIn, sumOut, movements);

        }
    }

}
