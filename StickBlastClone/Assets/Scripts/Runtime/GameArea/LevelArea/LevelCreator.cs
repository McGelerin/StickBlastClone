using Runtime.Data.Persistent.Level;
using Runtime.GameArea.Spawn;
using Runtime.Grid;
using Runtime.GridChecker.Signals;
using Runtime.Identifiers;
using Runtime.Infrastructures.Template;
using Runtime.Models;
using Runtime.Signals;
using UniRx;
using Zenject;

namespace Runtime.GameArea.LevelArea
{
    public class LevelCreator : SignalListener
    {
        private readonly IGameProgressModel _gameProgressModel;
        private readonly LevelsContainer _levelsContainer;

        [Inject] private GridGenerator gridGenerator;

        public LevelCreator(IGameProgressModel gameProgressModel, LevelsContainer levelsContainer)
        {
            _gameProgressModel = gameProgressModel;
            _levelsContainer = levelsContainer;
        }

        private void OnCreateLevelAreaSignal(CreateLevelAreaSignal createLevelAreaSignal) => CreateLevelArea();
        
        private void CreateLevelArea()
        {
            _signalBus.Fire(new SpawnObjectSignal());

            SetupLevelLayoutOnGrid();
            
            _signalBus.Fire(new ChangeLoadingScreenActivationSignal(isActive: false, null));
        }
        
        private void SetupLevelLayoutOnGrid()
        {
            int level = _gameProgressModel.Level % _levelsContainer.LevelData.Count;
            var gridData = _levelsContainer.LevelData[level].CustomCellDrawing;

            for (int x = 0; x < gridData.GetLength(0); x++)
            {
                for (int y = 0; y < gridData.GetLength(1); y++)
                {
                    if (gridData[x,y])
                    {
                        var transposeY = gridData.GetLength(1) - 1 - y;
                        SetEdgeData(x, transposeY);
                    }
                }
            }
            
            _signalBus.Fire(new DotCheckSignal());
            _signalBus.Fire(new CheckFillAreaSignal());
        }
        
        private void SetEdgeData(int x, int y)
        {
            GridEdge[,] edges = gridGenerator.Edges;
            
            edges[x,y].SetOccupied(Direction.Left);
            edges[x,y].SetOccupied(Direction.Down);
            
            edges[x + 1,y].SetOccupied(Direction.Left);
            edges[x,y + 1].SetOccupied(Direction.Down);
            
            edges[x,y].SetColor(Direction.Left, _gameProgressModel.ThemeColor);
            edges[x,y].SetColor(Direction.Down, _gameProgressModel.ThemeColor);
            
            edges[x + 1, y].SetColor(Direction.Left, _gameProgressModel.ThemeColor);
            edges[x,y + 1].SetColor(Direction.Down, _gameProgressModel.ThemeColor);
        }
        
        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<CreateLevelAreaSignal>()
                .Subscribe(OnCreateLevelAreaSignal)
                .AddTo(_disposables);
        }
    }
}