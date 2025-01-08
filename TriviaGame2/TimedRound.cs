using System;
using System.Timers;

namespace TriviaGame2
{
    public class TimedRound
    {
        private System.Timers.Timer _timer;
        private int _timeLeft;

        public event Action TimeExpired; // Event to notify when time is up

        public TimedRound(int duration)
        {
            _timeLeft = duration; // Set the duration for the round
        }

        public void Start()
        {
            _timer = new System.Timers.Timer(1000); // Set the timer to tick every second
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_timeLeft > 0)
            {
                _timeLeft--;
            }
            else
            {
                _timer.Stop();
                TimeExpired?.Invoke(); // Raise the TimeExpired event
            }
        }

        public int TimeLeft => _timeLeft; // Property to get the remaining time

        public void Stop()
        {
            _timer?.Stop();
        }
    }
}