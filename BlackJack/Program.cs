namespace BlackJack
{
    /// <summary>
    /// this program is a black jack game
    /// </summary>
    public class BlackJack
    {
        /// <summary>
        /// This list stores the deck of cards and can be made to store multiple decks
        /// </summary>
        private readonly List<string> _deck;

        /// <summary>
        /// creates deck 
        /// </summary>
        /// <param name="numberOfDecks">
        /// increases the amount of cards by how many decks are in play
        /// </param>
        private BlackJack(int numberOfDecks)
        {
            _deck = new List<string>();
            string[] suits = {"hearts", "diamonds", "clubs", "spades"};
            string[] values = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"};
            for (var i = 0; i < numberOfDecks; i++)
            {
                foreach (var suit in suits) 
                {
                    foreach (var value in values) 
                    {
                        _deck.Add(value + " of " + suit);
                    }
                }
            }

            ShuffleDeck();
        }

        /// <summary>
        /// shuffles the deck
        /// </summary>
        private void ShuffleDeck()
        {
            var random = new Random();
            var n = _deck.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                (_deck[k], _deck[n]) = (_deck[n], _deck[k]);
            }
        }

        /// <summary>
        /// pulls a card from the deck for the player or dealer to use
        /// </summary>
        /// <returns>
        /// returns the card that was pulled
        /// if empty returns deck is empty
        /// </returns>
        private string DrawCard()
        {
            if (_deck.Count > 0)
            {
                var card = _deck[0];
                _deck.RemoveAt(0);
                return card;
            }
            
            return "Deck is empty";
        }

        /// <summary>
        /// gets the value of the card
        /// </summary>
        /// <param name="card">
        /// uses the the card that was pulled
        /// </param>
        /// <param name="aceValue">
        /// checks how many aces are in the hand and adjusts the value based off it
        /// </param>
        /// <returns>
        /// returns the value of the card
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Handle the case where the card value is not valid
        /// </exception>
        private int GetCardValue(string card, int aceValue)
        {
            var value = card.Split(' ')[0];
            switch (value)
            {
                case "A":
                    return aceValue;
                case "K":
                case "Q":
                case "J":
                    return 10;
                default:
                {
                    if (int.TryParse(value, out var numericValue)) 
                    {
                        return numericValue;
                    }
                    
                    throw new ArgumentException("Invalid card value");
                }
            }
        }
       
       /// <summary>
       /// adjusts the value of the ace as needed for player
       /// </summary>
       /// <param name="playerValue">
       /// takes the player value
       /// </param>
       /// <param name="numberOfAcesP">
       /// checks to make sure there is an ace to be able to adjust the hand value
       /// </param>
       /// <returns>
       /// value of ace
       /// </returns>
       private int CheckAceValueP(int playerValue, int numberOfAcesP)
       {
           if (playerValue > 21 && numberOfAcesP > 0) {
               return 1;
           }

           return 11;
       }
       
       /// <summary>
       /// adjusts the value of the ace as needed for dealer
       /// </summary>
       /// <param name="dealerValue">
       /// takes the dealers value
       /// </param>
       /// <param name="numberOfAcesD">
       /// checks to make sure there is an ace to be able to adjust the hand value
       /// </param>
       /// <returns>
       /// value of ace
       /// </returns>
       private int CheckAceValueD(int dealerValue, int numberOfAcesD)
       {
           if (dealerValue > 21 && numberOfAcesD > 0) {
               return 1;
           }

           return 11;
       }
       
       /// <summary>
       /// keeps value of count of deck
       /// </summary>
       static int _count;
       
       /// <summary>
       /// adjust the count based on the value of the card dealt
       /// </summary>
       /// <param name="card">
       /// used card to determine how to adjust count
       /// </param>
       static void AdjustCount(string card) {
           var value = card.Split(" ")[0];
           if (value.Equals("2") || value.Equals("3") || value.Equals("4") || value.Equals("5") || value.Equals("6")) {
               _count++;
           } else if (value.Equals("10") || value.Equals("J") || value.Equals("Q") || value.Equals("K") || value.Equals("A")) {
               _count--;
           }
       }

       /// <summary>
       /// check win condition
       /// </summary>
       /// <param name="playerValue">
       /// take value of player hand
       /// </param>
       /// <param name="dealerValue">
       /// take value of dealer hand
       /// </param>
       /// <returns>
       /// returns a value based on outcome which determines what is said in the main
       /// </returns>
       static int CheckWin(int playerValue, int dealerValue)
       {
           if (dealerValue > 21)
           {
               return 2;
           }

           if (dealerValue < playerValue)
           {
               return 3;
           }

           if (dealerValue > playerValue)
           {
               return 4;
           }

           return 1;
       }

       /// <summary>
       /// checks if player has black jack
       /// </summary>
       /// <param name="playerValue">
       /// takes players value to see if it equals 21
       /// </param>
       /// <param name="dealerValue">
       /// takes dealer value to see if it also equals 21
       /// </param>
       /// <returns>
       /// returns a value based on if player has blackjack or not
       /// </returns>
       private static int CheckBlackJackP(int playerValue, int dealerValue)
       {
           if (playerValue > dealerValue)
           {
               return 1;
           }

           return 2;
       }
          
        
       static void Main()
       {
           //Create a new blackJack game
           var game = new BlackJack(6);
           
           //Player starting money
           Console.WriteLine("How much would you like to buy in for?");
           double.TryParse(Console.ReadLine(), out double playerMoney);

           while (true)
           {
               Console.Clear();

               var playerValue1 = 0;
               var playerValue2 = 0;
               var dealerValue = 0;
               var numberOfAcesP = 0;
               var numberOfAcesD = 0;
               double betAmount;
               
               //player bet amount
               Console.WriteLine("Bet minimum is $25. You have $" + playerMoney + ". How much would you like to bet?");
               do
               {
                   if (double.TryParse(Console.ReadLine(), out betAmount))
                   {
                       if (betAmount < 25)
                       {
                           Console.WriteLine("Invalid amount. Please increase bet.");
                       }
                       else if (betAmount > playerMoney)
                       {
                           Console.WriteLine("Insufficient funds. Please lower bet.");
                       }
                       else
                       {
                           Console.WriteLine("You chose to bet " + betAmount + ". Good Luck!");
                           break;
                       }
                   }
                   else
                   {
                       Console.WriteLine("Invalid input. Please enter a valid numeric amount.");
                   }
               } while (true);

               //draw two cards for player
               var playerCard1 = game.DrawCard();
               AdjustCount(playerCard1);
               var playerCard2 = game.DrawCard();
               AdjustCount(playerCard2);
               
               //draw two cards for dealer
               var dealerCard1 = game.DrawCard();
               AdjustCount(dealerCard1);
               var dealerCard2 = game.DrawCard();
               AdjustCount(dealerCard2);
               
               //get value of player hand
               playerValue1 += game.GetCardValue(playerCard1, game.CheckAceValueP(playerValue1, numberOfAcesP));
               playerValue2 += game.GetCardValue(playerCard2, game.CheckAceValueP(playerValue2, numberOfAcesP));
               var playerValue = playerValue2 + playerValue1;
               if (game.GetCardValue(playerCard1, game.CheckAceValueP(playerValue, numberOfAcesP)) == 11) numberOfAcesP++;
               if (game.GetCardValue(playerCard2 + playerCard1, game.CheckAceValueP(playerValue, numberOfAcesP)) == 11) numberOfAcesP++;

               while (playerValue > 21 && numberOfAcesP > 0)
               {
                   playerValue -= 10;
                   numberOfAcesP--;
               }

               //get value of dealer hand
               dealerValue += game.GetCardValue(dealerCard1, game.CheckAceValueD(dealerValue, numberOfAcesD));
               dealerValue += game.GetCardValue(dealerCard2, game.CheckAceValueD(dealerValue, numberOfAcesD));
               if (game.GetCardValue(dealerCard1, game.CheckAceValueD(dealerValue, numberOfAcesD)) == 11) numberOfAcesD++;
               if (game.GetCardValue(dealerCard2, game.CheckAceValueD(dealerValue, numberOfAcesD)) == 11) numberOfAcesD++;

               while (dealerValue > 21 && numberOfAcesD > 0)
               {
                   dealerValue -= 10;
                   numberOfAcesD--;
               }
               
               //print player cards
               Console.WriteLine("Player's cards: " + playerCard1 + ", " + playerCard2 + " (" + playerValue + ")");
               
               //print dealer cards
               Console.WriteLine("Dealer's cards: " + dealerCard1 + ", [hidden]");

               while (true)
               {
                   //determine if player has blackjack
                   if (playerValue == 21)
                   {
                       Console.WriteLine("Dealer's cards: " + dealerCard1 + ", " + dealerCard2 + " (" + dealerValue + ")");
                       if (CheckBlackJackP(playerValue, dealerValue) == 1)
                       {
                           betAmount *= 1.5;
                           playerMoney += betAmount;
                           Console.WriteLine("You win! Total funds now at $" + playerMoney + ".");
                       } else if (CheckBlackJackP(playerValue, dealerValue) == 2)
                       {
                           Console.WriteLine("It's a tie. Total funds now at $" + playerMoney + ".");
                       }

                       break;
                   }
                   
                   //determine if dealer has blackjack
                   if (dealerValue == 21)
                   {
                       playerMoney -= betAmount;
                       Console.WriteLine("Dealer's cards: " + dealerCard1 + ", " + dealerCard2 + " (" + dealerValue + ")");
                       Console.WriteLine("Dealer has Black Jack! You lose. Total funds now at $" + playerMoney + ".");
                       break;
                   }

                   if (playerValue1 == playerValue2 && betAmount <= playerMoney)
                   {
                       Console.WriteLine("Would you like to split? (y/n)");
                       var split = Console.ReadLine();
                       if (split != null && split.Equals("y", StringComparison.OrdinalIgnoreCase))
                       {
                           var playerCard3 = game.DrawCard();
                           AdjustCount(playerCard3);
                           playerValue1 += game.GetCardValue(playerCard3,
                               game.CheckAceValueP(playerValue1, numberOfAcesP));
                           if (game.GetCardValue(playerCard3, game.CheckAceValueP(playerValue1, numberOfAcesP)) == 11)
                               numberOfAcesP++;

                           Console.WriteLine(
                               "Hand one: " + playerCard1 + ", " + playerCard3 + " (" + playerValue1 + ")");

                           //determine if player has blackjack on hand one
                           if (playerValue1 == 21)
                           {
                               Console.WriteLine("Dealer's cards: " + dealerCard1 + ", " + dealerCard2 + " (" +
                                                 dealerValue + ")");
                               if (CheckBlackJackP(playerValue1, dealerValue) == 1)
                               {
                                   betAmount *= 1.5;
                                   playerMoney += betAmount;
                                   Console.WriteLine("You win! Total funds now at $" + playerMoney + ".");
                               }
                               else if (CheckBlackJackP(playerValue1, dealerValue) == 2)
                               {
                                   Console.WriteLine("It's a tie. Total funds now at $" + playerMoney + ".");
                               }
                           }
                           else
                           {
                               while (true)
                               {
                                   //ask player if they want to hit or stand
                                   Console.WriteLine("Do you want to hit, stand or double down? (h/s/d)");
                                   var choice = Console.ReadLine();
                                   if (choice != null && choice.Equals("h", StringComparison.OrdinalIgnoreCase))
                                   {
                                       var playerCard = game.DrawCard();
                                       AdjustCount(playerCard);
                                       playerValue1 += game.GetCardValue(playerCard,
                                           game.CheckAceValueP(playerValue1, numberOfAcesP));
                                       if (game.GetCardValue(playerCard,
                                               game.CheckAceValueP(playerValue1, numberOfAcesP)) == 11) numberOfAcesP++;
                                       while (playerValue1 > 21 && numberOfAcesP > 0)
                                       {
                                           playerValue1 -= 10;
                                           numberOfAcesP--;
                                       }

                                       Console.WriteLine("Player drew: " + playerCard + " (" + playerValue1 + ")");
                                       if (playerValue1 > 21)
                                       {
                                           playerMoney -= betAmount;
                                           Console.WriteLine("Player bust! You lose. Total funds now at $" +
                                                             playerMoney + ".");
                                           break;
                                       }
                                   }
                                   else if (choice != null && choice.Equals("s", StringComparison.OrdinalIgnoreCase))
                                   {
                                       break;
                                   }
                                   else if (choice != null && choice.Equals("d", StringComparison.OrdinalIgnoreCase))
                                   {
                                       if ((betAmount * 2) > playerMoney)
                                       {
                                           Console.WriteLine("Insufficient funds. Please choose a different option.");
                                           break;
                                       }

                                       betAmount *= 2;
                                       var playerCard = game.DrawCard();
                                       AdjustCount(playerCard);
                                       playerValue1 += game.GetCardValue(playerCard,
                                           game.CheckAceValueP(playerValue1, numberOfAcesP));
                                       if (game.GetCardValue(playerCard,
                                               game.CheckAceValueP(playerValue1, numberOfAcesP)) == 11) numberOfAcesP++;
                                       while (playerValue1 > 21 && numberOfAcesP > 0)
                                       {
                                           playerValue1 -= 10;
                                           numberOfAcesP--;
                                       }

                                       Console.WriteLine("Player drew: " + playerCard + " (" + playerValue1 + ")");
                                       if (playerValue1 > 21)
                                       {
                                           playerMoney -= betAmount;
                                           Console.WriteLine("Player bust! You lose. Total funds now at $" +
                                                             playerMoney + ".");
                                       }

                                       numberOfAcesP = 0;
                                       break;
                                   }
                                   else
                                   {
                                       Console.WriteLine("Invalid choice. Please enter 'h' or 's' or 'd'.");
                                   }
                               }
                           }
                           
                           var betSplit = betAmount;
                           var playerCard4 = game.DrawCard();
                           playerValue2 += game.GetCardValue(playerCard4,
                               game.CheckAceValueP(playerValue2, numberOfAcesP));
                           if (game.GetCardValue(playerCard4, game.CheckAceValueP(playerValue2, numberOfAcesP)) == 11)
                               numberOfAcesP++;

                           Console.WriteLine(
                               "Hand two: " + playerCard2 + ", " + playerCard4 + " (" + playerValue2 + ")");

                           //determine if player has blackjack on hand two
                           if (playerValue2 == 21)
                           {
                               Console.WriteLine("Dealer's cards: " + dealerCard1 + ", " + dealerCard2 + " (" +
                                                 dealerValue + ")");
                               if (CheckBlackJackP(playerValue2, dealerValue) == 1)
                               {
                                   betAmount *= 1.5;
                                   playerMoney += betSplit;
                                   Console.WriteLine("You win! Total funds now at $" + playerMoney + ".");
                               }
                               else if (CheckBlackJackP(playerValue2, dealerValue) == 2)
                               {
                                   Console.WriteLine("It's a tie. Total funds now at $" + playerMoney + ".");
                               }
                           }
                           else
                           {
                               while (true)
                               {
                                   //ask player if they want to hit or stand
                                   Console.WriteLine("Do you want to hit, stand or double down? (h/s/d)");
                                   var choice = Console.ReadLine();
                                   if (choice != null && choice.Equals("h", StringComparison.OrdinalIgnoreCase))
                                   {
                                       var playerCard = game.DrawCard();
                                       AdjustCount(playerCard);
                                       playerValue2 += game.GetCardValue(playerCard,
                                           game.CheckAceValueP(playerValue2, numberOfAcesP));
                                       if (game.GetCardValue(playerCard,
                                               game.CheckAceValueP(playerValue2, numberOfAcesP)) == 11) numberOfAcesP++;
                                       while (playerValue2 > 21 && numberOfAcesP > 0)
                                       {
                                           playerValue2 -= 10;
                                           numberOfAcesP--;
                                       }

                                       Console.WriteLine("Player drew: " + playerCard + " (" + playerValue2 + ")");
                                       if (playerValue2 > 21)
                                       {
                                           playerMoney -= betSplit;
                                           Console.WriteLine("Player bust! You lose. Total funds now at $" +
                                                             playerMoney + ".");
                                           break;
                                       }
                                   }
                                   else if (choice != null && choice.Equals("s", StringComparison.OrdinalIgnoreCase))
                                   {
                                       break;
                                   }
                                   else if (choice != null && choice.Equals("d", StringComparison.OrdinalIgnoreCase))
                                   {
                                       if ((betSplit * 2) > playerMoney)
                                       {
                                           Console.WriteLine("Insufficient funds. Please choose a different option.");
                                           break;
                                       }

                                       betSplit *= 2;
                                       var playerCard = game.DrawCard();
                                       AdjustCount(playerCard);
                                       playerValue2 += game.GetCardValue(playerCard,
                                           game.CheckAceValueP(playerValue2, numberOfAcesP));
                                       if (game.GetCardValue(playerCard,
                                               game.CheckAceValueP(playerValue2, numberOfAcesP)) == 11) numberOfAcesP++;
                                       while (playerValue2 > 21 && numberOfAcesP > 0)
                                       {
                                           playerValue2 -= 10;
                                           numberOfAcesP--;
                                       }

                                       Console.WriteLine("Player drew: " + playerCard + " (" + playerValue2 + ")");
                                       if (playerValue2 > 21)
                                       {
                                           playerMoney -= betSplit;
                                           Console.WriteLine("Player bust! You lose. Total funds now at $" +
                                                             playerMoney + ".");
                                       }

                                       break;
                                   }
                                   else
                                   {
                                       Console.WriteLine("Invalid choice. Please enter 'h' or 's' or 'd'.");
                                   }
                               }
                           }

                           Console.WriteLine("Dealer's cards: " + dealerCard1 + ", " + dealerCard2 + " (" + dealerValue + ")"); 
                           //dealer must hit while under 17
                           while (dealerValue < 17 )
                           {
                               var dealerCard = game.DrawCard();
                               AdjustCount(dealerCard);
                               dealerValue += game.GetCardValue(dealerCard,
                                   game.CheckAceValueD(dealerValue, numberOfAcesD));
                               if (game.GetCardValue(dealerCard, game.CheckAceValueD(dealerValue, numberOfAcesD)) ==
                                   11)
                                   numberOfAcesD++;
                               while (dealerValue > 21 && numberOfAcesD > 0)
                               {
                                   dealerValue -= 10;
                                   numberOfAcesD--;
                               }

                               Console.WriteLine("Dealer drew: " + dealerCard + " (" + dealerValue + ")");
                           }
                           

                           //determine winner for player hand 1
                           CheckWin(playerValue1, dealerValue);
                           if (CheckWin(playerValue1, dealerValue) == 1)
                           {
                               Console.WriteLine("It's a tie. Total funds now at $" + playerMoney + ".");
                           }
                           else if (CheckWin(playerValue1, dealerValue) == 2)
                           {
                               playerMoney += betAmount;
                               Console.WriteLine("Dealer bust! You win $" + betAmount + ". Total funds now at $" +
                                                 playerMoney + ".");
                           }
                           else if (CheckWin(playerValue1, dealerValue) == 3)
                           {
                               playerMoney += betAmount;
                               Console.WriteLine("You win $" + betAmount + "! Total funds now at $" + playerMoney +
                                                 ".");
                           }
                           else if (CheckWin(playerValue1, dealerValue) == 4)
                           {
                               playerMoney -= betAmount;
                               Console.WriteLine("You lose. Total funds now at $" + playerMoney + ".");
                           }

                           //determine winner for player hand 2
                               CheckWin(playerValue2, dealerValue);
                               if (CheckWin(playerValue2, dealerValue) == 1)
                               {
                                   Console.WriteLine("It's a tie. Total funds now at $" + playerMoney + ".");
                               }
                               else if (CheckWin(playerValue2, dealerValue) == 2)
                               {
                                   playerMoney += betSplit;
                                   Console.WriteLine("Dealer bust! You win $" + betSplit + ". Total funds now at $" +
                                                     playerMoney + ".");
                               }
                               else if (CheckWin(playerValue2, dealerValue) == 3)
                               {
                                   playerMoney += betSplit;
                                   Console.WriteLine("You win $" + betSplit + "! Total funds now at $" + playerMoney +
                                                     ".");
                               }
                               else if (CheckWin(playerValue2, dealerValue) == 4)
                               {
                                   playerMoney -= betSplit;
                                   Console.WriteLine("You lose. Total funds now at $" + playerMoney + ".");
                               }
                           

                           break;
                       } 
                   }

                   while (true)
                   {
                       //ask player if they want to hit or stand
                       Console.WriteLine("Do you want to hit, stand or double down? (h/s/d)");
                       var choice = Console.ReadLine();
                       if (choice != null && choice.Equals("h", StringComparison.OrdinalIgnoreCase))
                       {
                           var playerCard = game.DrawCard();
                           AdjustCount(playerCard);
                           playerValue += game.GetCardValue(playerCard,
                               game.CheckAceValueP(playerValue, numberOfAcesP));
                           if (game.GetCardValue(playerCard, game.CheckAceValueP(playerValue, numberOfAcesP)) == 11)
                               numberOfAcesP++;
                           while (playerValue > 21 && numberOfAcesP > 0)
                           {
                               playerValue -= 10;
                               numberOfAcesP--;
                           }

                           Console.WriteLine("Player drew: " + playerCard + " (" + playerValue + ")");
                           if (playerValue > 21)
                           {
                               playerMoney -= betAmount;
                               Console.WriteLine("Player bust! You lose. Total funds now at $" + playerMoney + ".");
                               Console.WriteLine("Dealer's cards: " + dealerCard1 + ", " + dealerCard2 + " (" + dealerValue + ")");
                               break;
                           }
                       }
                       else if (choice != null && choice.Equals("s", StringComparison.OrdinalIgnoreCase))
                       {
                           break;
                       }
                       else if (choice != null && choice.Equals("d", StringComparison.OrdinalIgnoreCase))
                       {
                           if ((betAmount * 2) > playerMoney)
                           {
                               Console.WriteLine("Insufficient funds. Please choose a different option.");
                               break;
                           }

                           betAmount *= 2;
                           var playerCard = game.DrawCard();
                           AdjustCount(playerCard);
                           playerValue += game.GetCardValue(playerCard, game.CheckAceValueP(playerValue, numberOfAcesP));
                           if (game.GetCardValue(playerCard, game.CheckAceValueP(playerValue, numberOfAcesP)) == 11) numberOfAcesP++;
                           while (playerValue > 21 && numberOfAcesP > 0)
                           {
                               playerValue -= 10;
                               numberOfAcesP--;
                           }

                           Console.WriteLine("Player drew: " + playerCard + " (" + playerValue + ")");
                           if (playerValue > 21)
                           {
                               playerMoney -= betAmount;
                               Console.WriteLine("Player bust! You lose. Total funds now at $" + playerMoney + ".");
                           }

                           break;
                       }
                       else
                       {
                           Console.WriteLine("Invalid choice. Please enter 'h' or 's' or 'd'.");
                       }
                   }

                   if (playerValue !> 21)
                   {
                       break;
                   }
                   
                   Console.WriteLine("Dealer's cards: " + dealerCard1 + ", " + dealerCard2 + " (" + dealerValue + ")");
                   //dealer must hit while under 17
                   while (dealerValue < 17 || (dealerValue > 17 && numberOfAcesD > 0)) 
                   {
                       var dealerCard = game.DrawCard();
                       AdjustCount(dealerCard);
                       dealerValue += game.GetCardValue(dealerCard,
                           game.CheckAceValueD(dealerValue, numberOfAcesD));
                       if (game.GetCardValue(dealerCard, game.CheckAceValueD(dealerValue, numberOfAcesD)) == 11)
                           numberOfAcesD++;
                       while (dealerValue > 21 && numberOfAcesD > 0)
                       {
                           dealerValue -= 10;
                           numberOfAcesD--;
                       }

                       Console.WriteLine("Dealer drew: " + dealerCard + " (" + dealerValue + ")");
                   }

                   CheckWin(playerValue, dealerValue);
                   if (CheckWin(playerValue, dealerValue) == 1)
                   {
                       Console.WriteLine("It's a tie. Total funds now at $" + playerMoney + ".");
                   }
                   else if (CheckWin(playerValue, dealerValue) == 2)
                   {
                       playerMoney += betAmount;
                       Console.WriteLine("Dealer bust! You win $" + betAmount + ". Total funds now at $" +
                                         playerMoney + ".");
                   }
                   else if (CheckWin(playerValue, dealerValue) == 3)
                   {
                       playerMoney += betAmount;
                       Console.WriteLine("You win $" + betAmount + "! Total funds now at $" + playerMoney + ".");
                   }
                   else if (CheckWin(playerValue, dealerValue) == 4)
                   {
                       playerMoney -= betAmount; 
                       Console.WriteLine("You lose. Total funds now at $" + playerMoney + "."); 
                   } 
                   break;
               }

               if (playerMoney < 25)
               {
                   Console.WriteLine("Insufficient funds. Game over.");
                   break;
               }
               
               Console.WriteLine("Do you want to play again? (y/n)");
               var playAgain = Console.ReadLine();
               if (playAgain != null && playAgain.Equals("n", StringComparison.OrdinalIgnoreCase))
               {
                   break;
               } else if (playAgain != null && playAgain.Equals("c", StringComparison.OrdinalIgnoreCase))
               {
                   Console.WriteLine("What is the count?");
                   if (int.TryParse(Console.ReadLine(), out var guessCount))
                   {
                       if (guessCount == _count)
                       {
                           Console.WriteLine("Correct! Would you like to play again? (y/n)");
                           var playAgainAgain = Console.ReadLine();
                           if (playAgainAgain != null && playAgainAgain.Equals("n", StringComparison.OrdinalIgnoreCase)) break;
                       }
                       else
                       {
                           Console.WriteLine("Incorrect. Count is " + _count + ". Game Over");
                           break;
                       }
                   }
                   else
                   {
                       Console.WriteLine("Invalid input. Game over.");
                       break;
                   }
               }
           }
       }
    }
}