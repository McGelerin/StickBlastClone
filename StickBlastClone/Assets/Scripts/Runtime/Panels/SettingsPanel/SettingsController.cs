using System;
using Runtime.Audio;
using Runtime.Audio.Data;
using Runtime.Audio.Model;
using Runtime.Audio.Signal.Audio;
using Runtime.Haptic.Model;
using Runtime.Identifiers;
using Runtime.Identifiers.Settings;
using Runtime.Infrastructures.Template;
using Runtime.Signals.Settings;
using UniRx;

namespace Runtime.Panels.Settings
{
    public class SettingsController : SignalListener
    {
        private readonly SettingsMediator _settingsMediator;
        private readonly IAudioModel _audioModel;
        private readonly IHapticModel _hapticModel;

        private PanelActivationState _activationState = PanelActivationState.Inactive;
        
        public SettingsController(SettingsMediator settingsMediator, IAudioModel audioModel, IHapticModel hapticModel)
        {
            _settingsMediator = settingsMediator;
            _audioModel = audioModel;
            _hapticModel = hapticModel;
        }

        public override void Initialize()
        {
            base.Initialize();

            //_settingsMediator.SetUpOnOffIcons(SettingsTypes.Music, _audioModel.IsMusicMuted);
            _settingsMediator.SetUpOnOffIcons(SettingsTypes.Sound, _audioModel.IsSoundMuted);
            _settingsMediator.SetUpOnOffIcons(SettingsTypes.Haptic, _hapticModel.IsHapticIdle);
        }

        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<GameSceneSettingsChangeSignal>().Subscribe(OnGameSceneSettingsChangeSignal).AddTo(_disposables);
        }

        private void OnGameSceneSettingsChangeSignal(GameSceneSettingsChangeSignal signal)
        {
            //play button click audio
            _signalBus.Fire(new AudioPlaySignal(AudioPlayers.Sound, Sounds.ButtonClick));
            
            switch (signal.GameSceneSettingsOption)
            {
                case GameSceneSettingsOption.Music:
                    OnMusicButtonClicked();
                    break;
                case GameSceneSettingsOption.Sound:
                    OnSoundButtonClicked();
                    break;
                case GameSceneSettingsOption.ToggleActivation:
                    OnToggleButtonClicked();
                    break;
                case GameSceneSettingsOption.Haptic:
                    OnHapticButtonClicked();
                    break;
                default:
                    throw new Exception("No settings option has been found!");
            }
        }
        
        private void OnMusicButtonClicked()
        {
            _settingsMediator.SetUpOnOffIcons(SettingsTypes.Music, !_audioModel.IsMusicMuted);
            _signalBus.Fire(new AudioButtonTypeSignal(AudioType.Music, !_audioModel.IsMusicMuted));
        }
        
        private void OnSoundButtonClicked()
        {
            _settingsMediator.SetUpOnOffIcons(SettingsTypes.Sound, !_audioModel.IsSoundMuted);
            _signalBus.Fire(new AudioButtonTypeSignal(AudioType.Sound, !_audioModel.IsSoundMuted));
            _signalBus.Fire(new AudioButtonTypeSignal(AudioType.Music, !_audioModel.IsMusicMuted));
        }
        
        private void OnHapticButtonClicked()
        {
            _settingsMediator.SetUpOnOffIcons(SettingsTypes.Haptic, !_hapticModel.IsHapticIdle);
            _hapticModel.SetHaptic(!_hapticModel.IsHapticIdle);
        }
        
        private async void OnToggleButtonClicked()
        {
            if (_activationState == PanelActivationState.Animating) return;
            PanelActivationState lastActivationState = _activationState;
            
            _activationState = PanelActivationState.Animating;
            
             await _settingsMediator.AnimateButtons(lastActivationState);

             _activationState = lastActivationState == PanelActivationState.Active
                                    ? PanelActivationState.Inactive
                                    : PanelActivationState.Active;
        }
    }
}