using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class CardBoot
    {
        #region Constructor
        char[] _suites = { '♠', '♥', '♣', '♦' };
        List<String> _cards;
        #endregion

        #region Properties
        public int CardsInBoot
        {
            get { return _cards.Count; }
        }
        #endregion

        #region Constructors
        public CardBoot()
        {
            _cards = new List<string>();
        }
        #endregion

        #region Methods

        public void DealHand(Player player)
        {
            player.Hand.Add(_cards[0]);
            player.Hand.Add(_cards[1]);
            _cards.RemoveRange(0, 2);
        }


        public static void ShowHand(BlackJack.Player player, bool showAll = false)
        {
            Console.Write($"Player: {player.Name} hand:");
            DisplayCard(player.Hand[0], player.isDealer || showAll);
            Console.Write(" ");
            DisplayCard(player.Hand[1], false);
            for (int j = 2; j < player.Hand.Count; j++)
            {
                Console.Write(" ");
                DisplayCard(player.Hand[j], player.isDealer || showAll);
            }
            Console.WriteLine();
        }

        public static void DisplayCard(String card, bool faceDown = false)
        {
            if (!faceDown)
            {
                if (card[0] == '0')
                    Console.Write(card.Substring(1, 1));
                else if (card[0] == '1')
                    Console.Write(card.Substring(0, 2));
                else
                    Console.Write(card.Substring(0, 1));
                Console.Write(card.Last());
            }
            else
                Console.Write("▒");
        }

        public void DrawCard(Player player)
        {
            player.Hand.Add(_cards[0]);
            Console.Write($"Player: {player.Name} Drew a ");
            DisplayCard(_cards[0]);
            Console.WriteLine();
            _cards.Remove(_cards[0]);
        }

        public void Initialize()
        {
            List<String> deck = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach(char s in _suites)
            {
                sb.Append("A  ");
                sb.Append(s);
                deck.Add(sb.ToString());
                sb.Clear();
                for(int i = 2; i<10; i++)
                {
                    sb.AppendFormat($"{i.ToString("00")} ");
                    sb.Append(s);
                    deck.Add(sb.ToString());
                    sb.Clear();
                }
                sb.Append("10 ");
                sb.Append(s);
                deck.Add(sb.ToString());
                sb.Clear();

                sb.Append("J  ");
                sb.Append(s);
                deck.Add(sb.ToString());
                sb.Clear();
          
                sb.Append("Q  ");
                sb.Append(s);
                deck.Add(sb.ToString());
                sb.Clear();
             
                sb.Append("K  ");
                sb.Append(s);
                deck.Add(sb.ToString());
                sb.Clear();
            }
            DateTime dt = DateTime.Now;
            int seed = dt.Year * 10000 + dt.Month * 100  + dt.Day + dt.Hour + dt.Minute + dt.Second;
            Random rnd = new Random(seed);
            for(int c=0; c<deck.Count; c++)
            {
                string temp = deck[c];
                int slot = rnd.Next(0, 51);
                if (slot != c)
                {
                    deck[c] = deck[slot];
                    deck[slot] = temp;
                }
            }
            _cards.AddRange(deck);
        }
        #endregion
    }
}
