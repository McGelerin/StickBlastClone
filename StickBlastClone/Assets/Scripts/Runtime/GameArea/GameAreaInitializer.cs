using System;
using Cysharp.Threading.Tasks;
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

            //_signalBus.Fire(new ChangeLoadingScreenActivationSignal(isActive: false, null));
        }
    }
}