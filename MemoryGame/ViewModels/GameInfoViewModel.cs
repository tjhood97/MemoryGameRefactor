using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemoryGame.ViewModels
{
    public class GameInfoViewModel : ObservableObject
    {
        private int _matchAttempts;
        private int _score;

        private bool? _isGameWon;

        public int MatchAttempts
        {
            get
            {
                return _matchAttempts;
            }
            private set
            {
                _matchAttempts = value;
                OnPropertyChanged("MatchAttempts");
            }
        }

        public int Score
        {
            get
            {
                return _score;
            }
            private set
            {
                _score = value;
                OnPropertyChanged("Score");
            }
        }

        public Visibility LostMessage
        {
            get
            {
                if (_isGameWon == false)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }
        }

        public Visibility WinMessage
        {
            get
            {
                if (_isGameWon == true)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }
        }

        public void SetGameStatus(bool? isWin)
        {
            if (isWin == true)
            {
                _isGameWon = true;
                OnPropertyChanged("WinMessage");
            }
            else if (isWin == false)
            {
                _isGameWon = false;
                OnPropertyChanged("LostMessage");
            }
        }

        public void ResetInfo()
        {
            Score = 0;
            MatchAttempts = VMConstants.MaxAttempts;
            _isGameWon = null;
            OnPropertyChanged("LostMessage");
            OnPropertyChanged("WinMessage");
        }

        public void Award()
        {
            Score += VMConstants.PointAward;
            SoundManager.PlayCorrect();
        }

        public void Penalize()
        {
            Score -= VMConstants.PointDeduction;
            MatchAttempts--;
            SoundManager.PlayIncorrect();
        }
    }
}
