namespace Runtime.Signals
{
    public readonly struct IncreaseScoreSignal
    {
        private readonly int _increaseScore;
        public int IncreaseScore => _increaseScore;

        public IncreaseScoreSignal(int increaseScore)
        {
            _increaseScore = increaseScore;
        }
    }
}