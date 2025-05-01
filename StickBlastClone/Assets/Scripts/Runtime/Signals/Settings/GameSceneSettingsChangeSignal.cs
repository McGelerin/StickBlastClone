using Runtime.Identifiers;

namespace Runtime.Signals.Settings
{
    public readonly struct GameSceneSettingsChangeSignal
    {
        private readonly GameSceneSettingsOption _gameSceneSettingsOption;

        public GameSceneSettingsOption GameSceneSettingsOption => _gameSceneSettingsOption;
        
        public GameSceneSettingsChangeSignal(GameSceneSettingsOption gameSceneSettingsOption)
        {
            _gameSceneSettingsOption = gameSceneSettingsOption;
        }
    }
}