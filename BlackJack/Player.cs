using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Player
    {
        #region Declaration
        string _name = "Dealer";
        int _chips = 0;
        bool _isDealer = true;
        List<String> _hand;
        int _points = 0;
        int _chipsOnBet = 0;
        #endregion

        #region Constructor
        protected Player()
        {
            _hand = new List<string>();
        }

        public Player(String playerName, bool isDealer, int chips) : this()
        {
            _name = playerName;
            _isDealer = isDealer;
            _chips = chips;
        }
        #endregion

        #region Properties
        public bool hasBlackJack
        {
            get
            {
                if (this.ValueOfHand() == 21 && _hand.Count == 2)
                    return true;
                else
                    return false; }
        }

        public bool hasAnAce
        {
            get
            {
                bool hasAce = false;
                foreach(var c in _hand)
                {
                    if (c[0] == 'A')
                    {
                        hasAce = true;
                        break;
                    }
                }
                return hasAce;
            }
        }
        public int Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public bool isDealer
        {
            get { return _isDealer; }
        }

        public bool isBusted
        {
            get
            {
                if (this.ValueOfHand() > 21)
                    return true;
                else
                    return false;
            }
        }

        public int ChipCount
        {
            get { return _chips; }
            set { _chips = value; }
        }

        public int ChipsOnBet
        {
            get { return _chipsOnBet; }
            set { _chipsOnBet = value; }
        }


        public string Name
        {
            get { return _name; }
        }
        public List<String> Hand
        {
            get { return _hand; }
        }
  
        #endregion

        #region Methods
        public int AddChips(int chips)
        {
            _chips += chips;
            return _chips;
        }

        public void Discard()
        {
            _hand.Clear();
        }

        public void ShowUpCard()
        {
            if (_hand.Count > 1)
            {
                Console.Write($"Player: {_name}, up card is ");
                CardBoot.DisplayCard(_hand[1]);
            }
        }

        public int ValueOfHand()
        {
            int lowHand = 0;
            int highHand = 0;
            foreach(String card in _hand)
            {
                if (card[0] == 'A')
                {
                    lowHand += 1;
                    highHand += 11;
                }
                else if (card[0] == '1' || card[0] == 'J' || card[0] == 'Q' || card[0] == 'K')
                {
                    lowHand += 10;
                    highHand += 10;
                }
                else
                {
                    int v = Convert.ToInt32(card.Substring(0, 2));
                    lowHand += v;
                    highHand += v;
                }
            }
            if (highHand > 21)
                return lowHand;
            else
                return highHand;
        }

        #endregion
    }


}
