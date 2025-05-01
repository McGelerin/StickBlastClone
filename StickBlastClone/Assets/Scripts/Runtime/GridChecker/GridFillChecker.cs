using Runtime.Grid;
using Runtime.GridChecker.Signals;
using Runtime.Infrastructures.Template;
using Runtime.Models;
using UniRx;
using UnityEngine;
using Zenject;

namespace Runtime.GridChecker
{
    public class GridFillChecker : SignalListener
    {
        [Inject] private GridGenerator _gridGenerator;
        private IGameProgressModel _gameProgressModel;

        public GridFillChecker(IGameProgressModel gameProgressModel)
        {
            _gameProgressModel = gameProgressModel;
        }

        private void OnCheckFillAreaSignal(CheckFillAreaSignal signal)
        {
            for (int x = 0; x < _gridGenerator.HorizontalCellCount; x++)
            {
                for (int y = 0; y < _gridGenerator.VerticalCellCount; y++)
                {
                    if (_gridGenerator.Grid[x,y].IsFilled) continue;

                    if (_gridGenerator.Edges[x,y].LeftEdgeIsOccupied && _gridGenerator.Edges[x,y].DownEdgeIsOccupied)
                    {
                        if (_gridGenerator.Edges[x , y +1].DownEdgeIsOccupied && _gridGenerator.Edges[x + 1, y].LeftEdgeIsOccupied)
                        {
                            FillArea(x, y , _gameProgressModel.ThemeColor);
                        }
                    }
                }
            }
            
            _signalBus.Fire(new CheckRowColumnSignal());
        }

        private void FillArea(int x, int y, Color32 color)
        {
            _gridGenerator.Grid[x,y].SetFill(true);
            _gridGenerator.Grid[x,y].SetColor(color);
        }
        
        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<CheckFillAreaSignal>()
                .Subscribe(OnCheckFillAreaSignal)
                .AddTo(_disposables);
        }
    }
}