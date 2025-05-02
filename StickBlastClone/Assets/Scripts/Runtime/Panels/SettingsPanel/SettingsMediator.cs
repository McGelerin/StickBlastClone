using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Identifiers;
using Runtime.Identifiers.Settings;
using Runtime.Signals.Settings;
using Zenject;

namespace Runtime.Panels.Settings
{
    public class SettingsMediator  : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly SettingsView _settingsView;

        public SettingsMediator(SignalBus signalBus,  SettingsView settingsView)
        {
            _signalBus = signalBus;
            _settingsView = settingsView;
        }

        public void Initialize()
        {
            //_settingsView.MusicButton.onClick.AddListener(() => FireGameSceneSettingsChangeSignal(GameSceneSettingsOption.Music));
            _settingsView.SoundButton.onClick.AddListener(() => FireGameSceneSettingsChangeSignal(GameSceneSettingsOption.Sound));
            _settingsView.HapticButton.onClick.AddListener(() => FireGameSceneSettingsChangeSignal(GameSceneSettingsOption.Haptic));
            _settingsView.TogglePanelButton.onClick.AddListener(() => FireGameSceneSettingsChangeSignal(GameSceneSettingsOption.ToggleActivation));
        }

        private void FireGameSceneSettingsChangeSignal(GameSceneSettingsOption gameSceneSettingsOption)
        {
            _signalBus.Fire(new GameSceneSettingsChangeSignal(gameSceneSettingsOption));
        }
        
        public void SetUpOnOffIcons(SettingsTypes settingsType, bool isMuted)
        {
            _settingsView.SettingsOffGameObjects[settingsType].SetActive(isMuted);
        }
        
        public void Dispose()
        {
            //_settingsView.MusicButton.onClick.RemoveAllListeners();
            _settingsView.SoundButton.onClick.RemoveAllListeners();
            _settingsView.HapticButton.onClick.RemoveAllListeners();
            _settingsView.TogglePanelButton.onClick.RemoveAllListeners();
        }

        public async UniTask AnimateButtons(PanelActivationState lastActivationState)
        {
            int buttonTransformsCount = _settingsView.ButtonTransforms.Count;
            
            if (lastActivationState == PanelActivationState.Inactive)
            {
                
                for (int i = 0; i < buttonTransformsCount; i++)
                   await _settingsView.ButtonTransforms[i].transform.DOMoveX(155f, 0.1f);
            }
            else
            {

                for (int i = buttonTransformsCount - 1; i >= 0; i--)
                    await _settingsView.ButtonTransforms[i].transform.DOMoveX(-300f, 0.1f);
            }
        }
    }
}