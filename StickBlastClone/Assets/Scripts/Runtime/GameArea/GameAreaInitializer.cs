using System;
using Cysharp.Threading.Tasks;
using Runtime.Audio;
using Runtime.Audio.Data;
using Runtime.Audio.Signal.Audio;
using Runtime.Signals;
using Zenject;

namespace Runtime.GameArea
{
    public class GameAreaInitializer : IInitializable
    {
        [Inject] private SignalBus _signalBus;
        
        public async void Initialize()
        {
            GC.Collect();
            await UniTask.NextFrame();
            
            _signalBus.Fire(new CreateLevelAreaSignal());
            _signalBus.Fire(new AudioPlaySignal(AudioPlayers.Music, Sounds.Gameplay1));
        }
    }
}