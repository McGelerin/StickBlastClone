namespace Runtime.Signals
{
    public readonly struct LevelEndSignal
    {
        private readonly bool _isLevelSuccess;
        public bool IsLevelSuccess => _isLevelSuccess;

        public LevelEndSignal(bool isLevelSuccess)
        {
            _isLevelSuccess = isLevelSuccess;
        }
    }
}