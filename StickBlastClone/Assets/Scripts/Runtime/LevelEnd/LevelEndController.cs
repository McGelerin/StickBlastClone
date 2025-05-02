using System.Collections.Generic;
using Runtime.Data.Persistent.PlaceholderDataSO;
using Runtime.GameArea.Spawn;
using Runtime.Grid;
using Runtime.Infrastructures.Template;
using Runtime.Input.Raycasting;
using Runtime.Models;
using Runtime.Signals;
using UniRx;

namespace Runtime.LevelEnd
{
    public class LevelEndController : SignalListener
    {
        private readonly IGameProgressModel _gameProgressModel;
        private readonly IScoreModel _scoreModel;
        private readonly SpawnAreaController _spawnAreaController;
        private readonly GridGenerator _gridGenerator;
        private readonly PlaceholderSO _placeholderSo;


        public LevelEndController(IGameProgressModel gameProgressModel, IScoreModel scoreModel, SpawnAreaController spawnAreaController, GridGenerator gridGenerator, PlaceholderSO placeholderSo)
        {
            _gameProgressModel = gameProgressModel;
            _scoreModel = scoreModel;
            _spawnAreaController = spawnAreaController;
            _gridGenerator = gridGenerator;
            _placeholderSo = placeholderSo;
        }

        private void OnCheckLevelEnd(CheckLevelEndSignal signal)
        {
            if (_scoreModel.Score >= _gameProgressModel.RequirementScore)
            {
                _signalBus.Fire(new GameWinLoseSignal(true));
            }
            else if (!CanAnyObjectBePlaced())
            {
                _signalBus.Fire(new GameWinLoseSignal(false));
            }
        }

        private bool CanAnyObjectBePlaced()
        {
            List<IClickable> placeholderGridObjects = _spawnAreaController.SpawnedPlaceholderGridObject;

            foreach (var placeholderGridObject in placeholderGridObjects)
            {
                List<List<PlaceholderDataVO>> placeholderSizeData = _placeholderSo.PlaceHolderData[placeholderGridObject.GetPlaceholderType()];
                
                for (int x = 0; x < _gridGenerator.HorizontalCellCount; x++)
                {
                    for (int y = 0; y < _gridGenerator.VerticalCellCount; y++)
                    {
                        if (CheckGrid(placeholderSizeData, x, y))
                        {
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

        private bool CheckGrid(List<List<PlaceholderDataVO>> placeholderSizeData, int gridPosX, int gridPosY)
        {
            GridEdge[,] edges = _gridGenerator.Edges;
            
            for (int x = 0; x < placeholderSizeData.Count; x++)
            {
                for (int y = 0; y < placeholderSizeData[x].Count; y++)
                {
                    int checkX = gridPosX + x;
                    int checkY = gridPosY + y;
                    
                    if (placeholderSizeData[x][y].HasDownEdge)
                    {
                        if (edges[checkX,checkY].DownEdgeIsOccupied)
                        {
                            return false;
                        }
                    }
                    
                    if (placeholderSizeData[x][y].HasLeftEdge)
                    {
                        if (edges[checkX,checkY].LeftEdgeIsOccupied)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        
        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<CheckLevelEndSignal>()
                .Subscribe(OnCheckLevelEnd)
                .AddTo(_disposables);
        }
    }
}