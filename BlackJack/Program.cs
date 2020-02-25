using System;
using System.Text;

public class BlackJackGame
{
    private static BlackJack.CardBoot boot = new BlackJack.CardBoot();

    public static void Main()
    {
        bool continuePlay = true;

        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "BlackJack Game";
        Console.Write("\r\n\r\nEnter player's name: ");

        // Both players start with 100 chips.
        var player = new BlackJack.Player(Console.ReadLine(), false, 100);
        var dealer = new BlackJack.Player("Dealer", true, 100);

        while (continuePlay)
        {           
            Console.Clear();
            player.Discard();
            dealer.Discard();
           

            if (boot.CardsInBoot < 20)
            {
                // Add a deck to the boot.
                boot.Initialize();
                Console.WriteLine("Adding deck to the boot.");
            }


            // Show player bank roll
            Console.WriteLine($"{player.Name} Chips Balance: {player.ChipCount}");

            // Make a bet
            Console.Write("Enter number of chips to bet: ");
            player.ChipsOnBet = Convert.ToInt16(Console.ReadLine());
            boot.DealHand(player);
            BlackJack.CardBoot.ShowHand(player);            
            boot.DealHand(dealer);
            BlackJack.CardBoot.ShowHand(dealer);

            // Check for black jack
            if (!checkForBlackJack(player, dealer))
            {                
                PlayerAction(player);
                Console.WriteLine("\r\n\r\n");

                // check for player busting before giving the other player a chance to play.
                if (!player.isBusted)
                {
                    PlayerAction(dealer);
                    Console.WriteLine("\r\n\r\n");
                }
                SummarizeGame(player, dealer);
            }

            Console.WriteLine("This hand is over.");
            Console.Write("\r\nPlay again? Y or N? ");
            String cont = Console.ReadLine();
            if (cont.Trim().ToUpper() == "Y")
                continuePlay = true;
            else
                continuePlay = false;         
        }

        Console.WriteLine($"{player.Name} has {player.Points} points.");
        Console.WriteLine($"{dealer.Name} has {dealer.Points} points.");
        Console.WriteLine("Game over. Thank you for playing.");

    }

    private static void PlayerAction(BlackJack.Player currentPlayer)
    {
        bool playerTurnContinue = true;
        string opt = "";

        while (playerTurnContinue)
        {
            Console.Write($"\r\n{currentPlayer.Name}'s turn. ");

            if (currentPlayer.isDealer)
            {
                int val = currentPlayer.ValueOfHand();

                if ((currentPlayer.hasAnAce & val < 18) || (!currentPlayer.hasAnAce && val < 17))
                    opt = "H";
                else
                    opt = "S";
            }
            else
            {
                // Prompt player to enter Hit or Stand.
                Console.Write("Hit (H) or Stand (S): ");
                opt = Console.ReadLine().Trim();
            }

            switch (opt.ToUpper())
            {
                case "H":
                    Console.WriteLine($"{currentPlayer.Name} hits. ");
                    boot.DrawCard(currentPlayer);
                    BlackJack.CardBoot.ShowHand(currentPlayer);
                    break;
                case "S":
                    if (currentPlayer.ValueOfHand() < 16)
                        Console.WriteLine($"{currentPlayer.Name} is not allowed to stands when hand value is less than 16.");
                    else
                    {
                        Console.WriteLine($"{currentPlayer.Name} stands.");
                        Console.WriteLine($"{currentPlayer.Name}'s turn is over.");
                        playerTurnContinue = false;
                    }

                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }

            //  Greater than 21 is a bust
            if (currentPlayer.ValueOfHand() > 21)
            {
                Console.WriteLine("Busted!");
                Console.WriteLine($"{currentPlayer.Name}'s turn is over.");               
                playerTurnContinue = false;
            }
            // 
            else if (currentPlayer.Hand.Count == 5)
            {
                Console.WriteLine($"{currentPlayer.Name} got 5 cards in hand already.");
                Console.WriteLine($"{currentPlayer.Name}'s turn is over.");
                playerTurnContinue = false;
            }


        }
    }

    private static bool checkForBlackJack(BlackJack.Player player, BlackJack.Player dealer)
    {
        Console.WriteLine();
        if (dealer.hasBlackJack && player.hasBlackJack)
        {
            Console.WriteLine("Player and Dealer got BlackJack. Tie Game!");
            dealer.Points += 1;
            player.Points += 1;

            return true;
        }
        else if (dealer.hasBlackJack && !player.hasBlackJack)
        {
            Console.WriteLine($"{dealer.Name} got BlackJack. {dealer.Name} won!");
            dealer.ShowUpCard();
            dealer.Points += 2;
            player.ChipCount = player.ChipCount - (int)Math.Floor(player.ChipsOnBet * 1.5);
            return true;
        }
        else if (!dealer.hasBlackJack && player.hasBlackJack)
        {
            Console.WriteLine($"{player.Name} has BlackJack. {player.Name} won!");
            player.Points += 2;
            player.ChipCount = player.ChipCount + (int)Math.Floor(player.ChipsOnBet * 1.5);
            return true;
        }
        return false;
    }

    private static void SummarizeGame(BlackJack.Player player, BlackJack.Player dealer)
    {
        Console.WriteLine();
        if (!dealer.isBusted && player.isBusted)
        {
            Console.WriteLine($"{dealer.Name} won.");
        }
        else if (dealer.isBusted && !player.isBusted)
        {
            Console.WriteLine($"{player.Name} won.");
            player.ChipCount = player.ChipCount + player.ChipsOnBet;
        }
        else if (dealer.isBusted && player.isBusted)
        {
            Console.WriteLine("Tie game.");
        }
        else if (!dealer.isBusted && !player.isBusted)
        {
            if (player.ValueOfHand() > dealer.ValueOfHand())
            {
                Console.WriteLine($"{player.Name} won.");
                player.ChipCount = player.ChipCount + player.ChipsOnBet;
            }
            else if (player.ValueOfHand() < dealer.ValueOfHand())
            {
                Console.WriteLine($"{dealer.Name} won.");                
                player.ChipCount = player.ChipCount - player.ChipsOnBet;
            }
        }
        else if (player.ValueOfHand() == dealer.ValueOfHand())
        {
            Console.WriteLine("Tie game.");
        }
    }
}