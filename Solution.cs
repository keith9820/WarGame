using System;

namespace WarGame
{
    class Program
    {
        #region field declarations
        //.NET's Random class is used to generate random numbers
        static Random rand = new Random();

        //These fields are counters:
        static int numWars, numBattles, numWins, numLosses, numDraws = 0;
        #endregion

        //program execution starts here
        static void Main(string[] args)
        {
            PrintHeader();

            for (int i = 1; ; i++) //this is an infinite loop (not condition)
            {
                int ret = ProcessCommandLine();
                if (ret == 0)
                    break;
            }
        }

        /// <summary>
        /// This mehod prints the avaliable commands and processes the user input.
        /// </summary>
        /// <returns>
        /// If the quit command is selected, this program return 0, otherwise 1.
        /// This is a way of communicating back to the calling method that the user wants to exit.
        /// </returns>
        private static int ProcessCommandLine()
        {
            #region Print the commands menu (the coloring is just for readability)
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Command ([");
            Console.ResetColor();
            Console.Write("P");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("]lay, [");
            Console.ResetColor();

            Console.Write("S");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("]coreboard, [");
            Console.ResetColor();

            Console.Write("Q");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("]uit): ");
            Console.ResetColor();
            #endregion

            //read the user's selection
            string ret = Console.ReadLine().ToLower(); //notice the use of ToLower() so p & P are the same

            switch (ret)
            {
                case "": //if the user just hits the return key, assume Play command
                case "p":
                    WageWar();
                    break;
                case "s":
                    PrintScore();
                    PrintHeader();
                    break;
                case "q":
                    if (numWars > 0)
                        PrintScore(true);
                    return 0;
                default:  //if anything else was entered, it's invalid
                    Console.WriteLine("** Invalid Command **");
                    break;
            }
            return 1;
        }

        private static void WageWar()
        {
            numWars++; //increment the number of wars counter

            #region Print the game (war) number
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nGame# {0}:\n========", numWars);
            Console.ResetColor();
            
            #endregion

            BattleStatus status = DoBattle();

            while (status == BattleStatus.Draw) //when it's a tie keep doing batle until someone wins
            {
                #region print "Tie!  TO WAR!!!!"
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Tie!  TO WAR!!!");
                Console.ResetColor();
                #endregion
                status = DoBattle();
            }
            if (status == BattleStatus.Win)
            {
                #region print "You WIN!"
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You WIN!");
                Console.ResetColor();
                #endregion
            }
            else
            {
                #region print "You LOSE!"
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You LOSE!");
                Console.ResetColor();
                #endregion
            }
            Console.WriteLine("----------------------------------------------------");
        }

        /// <summary>
        /// Do battle.  Each player draws a card which is simulated by generating a random number for each card
        /// The values are compared, the appropriate counter is incremented (wins, losses, draws), and the battle status is returned.
        /// </summary>
        /// <returns>
        /// BattleStatus enum value which represents the outcome of a Battle: Win, Lose, Draw
        /// </returns>
        private static BattleStatus DoBattle()
        {
            int player1Card = rand.Next(2, 14); //rand was declared above and is a field of type Random
            int player2Card = rand.Next(2, 14);

            numBattles++;

            Console.WriteLine("  >You draw a {0}", DisplayCard(player1Card));
            Console.WriteLine("  >Dealer draws a {0}", DisplayCard(player2Card));

            if (player1Card > player2Card) //Player #1 has a higher (winning) card
            {
                numWins++;
                return BattleStatus.Win;
            }

            if (player1Card < player2Card) //Player #1 has a lower (losing) card
            {
                numLosses++;
                return BattleStatus.Lose;
            }

            //only get here if player1Card == player2Card
            numDraws++;
            return BattleStatus.Draw;
        }

        private static void PrintHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
****************************************************
**            Welcome to WAR                      **
****************************************************");
            Console.ResetColor();
        }

        /// <summary>
        /// This helper method converts numbers to face card equivalents
        /// </summary>
        /// <param name="num">The number to be converted to a string</param>
        /// <returns>A string representation of a card.</returns>
        private static string DisplayCard(int num)
        {
            switch (num)
            {
                case 14:
                    return "Ace";
                case 13:
                    return "King";
                case 12:
                    return "Queen";
                case 11:
                    return "Jack";
                default:
                    return num.ToString();
            }
        }

        //The PrintScore method is overloaded to accept a boolean.  If true, an exit message is printed.
        private static void PrintScore()
        {
            PrintScore(false);
        }
        private static void PrintScore(bool showExitMessage)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"
*****************************************
**             SCOREBOARD              **
*****************************************");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n{0,8}:  {1,4}", "Wars", numWars);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0,8}:  {1,4}", "Battles", numBattles);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0,8}:  {1,4}", "Wins", numWins);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0,8}:  {1,4}", "Losses", numLosses);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("{0,8}:  {1,4}", "Draw", numDraws);
            Console.ResetColor();

            if (showExitMessage)
                Console.Write("\n(Game Over.  Press the enter key to exit.)");
            else
                Console.Write("\n(press the enter key to return)");

            Console.ReadLine();
        }
    }

    public enum BattleStatus
    {
        Win,
        Lose,
        Draw
    }
}
