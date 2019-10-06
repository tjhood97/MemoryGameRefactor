using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MemoryGame.ViewModels
{
    public class TimerViewModel : ObservableObject
    {
        private readonly DispatcherTimer _playTimer;
        private TimeSpan _timePlayed;

        public TimeSpan Time
        {
            get
            {
                return _timePlayed;
            }
            set
            {
                _timePlayed = value;
                OnPropertyChanged("Time");
            }
        }

        public TimerViewModel(TimeSpan time)
        {
            _playTimer = new DispatcherTimer
            {
                Interval = time
            };
            _playTimer.Tick += PlayedTimer_Tick;
            _timePlayed = new TimeSpan();
        }

        public void Start()
        {
            _playTimer.Start();
        }

        public void Stop()
        {
            _playTimer.Stop();
        }

        private void PlayedTimer_Tick(object sender, EventArgs e)
        {
            Time = _timePlayed.Add(new TimeSpan(0, 0, 1));
        }
    }
}
