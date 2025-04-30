using Runtime.Data.Persistent.Level;
using Runtime.GameArea.Spawn;
using Runtime.Grid;
using Runtime.Infrastructures.Template;
using Runtime.Models;
using Runtime.Signals;
using UniRx;
using Zenject;

namespace Runtime.GameArea.LevelArea
{
    public class LevelCreator : SignalListener
    {
        private readonly GameProgressModel _gameProgressModel;
        private readonly LevelsContainer _levelsContainer;

        [Inject] private GridGenerator gridGenerator;

        public LevelCreator(GameProgressModel gameProgressModel, LevelsContainer levelsContainer)
        {
            _gameProgressModel = gameProgressModel;
            _levelsContainer = levelsContainer;
        }

        private void OnCreateLevelAreaSignal(CreateLevelAreaSignal createLevelAreaSignal) => CreateLevelArea();
        
        private void CreateLevelArea()
        {
            _signalBus.Fire(new SpawnObjectSignal());
            int level = _gameProgressModel.Level % _levelsContainer.LevelData.Count;
            _signalBus.Fire(new ChangeLoadingScreenActivationSignal(isActive: false, null));
        }
        
        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<CreateLevelAreaSignal>()
                .Subscribe(OnCreateLevelAreaSignal)
                .AddTo(_disposables);
        }
    }
}