using System;
using Runtime.Audio;
using Runtime.Audio.Data;
using Runtime.Audio.Signal.Audio;
using Runtime.Identifiers;
using Runtime.Signals;
using Zenject;

namespace Runtime.Panels.Header
{
    public class HeaderMediator  : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly HeaderView _headerView;

        public HeaderMediator(SignalBus signalBus,  HeaderView headerView)
        {
            _signalBus = signalBus;
            _headerView = headerView;
        }

        public void Initialize()
        {
            _headerView.RestartButton.onClick.AddListener(FireRestartGameSignal);
        }

        private void FireRestartGameSignal()
        {
            _signalBus.Fire(new AudioPlaySignal(AudioPlayers.Sound, Sounds.ButtonClick));
            _signalBus.Fire(new LoadSceneSignal(SceneID.GameScene));
        }
        
        public void Dispose()
        {
            _headerView.RestartButton.onClick.RemoveAllListeners();
        }
    }
}