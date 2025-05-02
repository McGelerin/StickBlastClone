namespace Runtime.Signals
{
    public readonly struct GameWinLoseSignal
    {
        private readonly bool _isLevelSuccess;
        public bool IsLevelSuccess => _isLevelSuccess;

        public GameWinLoseSignal(bool isLevelSuccess)
        {
            _isLevelSuccess = isLevelSuccess;
        }
    }
}