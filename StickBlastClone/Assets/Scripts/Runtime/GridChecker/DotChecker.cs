using Runtime.Grid;
using Runtime.GridChecker.Signals;
using Runtime.Infrastructures.Template;
using Runtime.Models;
using UniRx;
using Zenject;

namespace Runtime.GridChecker
{
    public class DotChecker : SignalListener
    {
        [Inject] private GridGenerator _gridGenerator;
        private IGameProgressModel _gameProgressModel;

        public DotChecker(IGameProgressModel gameProgressModel)
        {
            _gameProgressModel = gameProgressModel;
        }


        private void OnDotCheckSignal(DotCheckSignal signal)
        {
            for (int x = 0; x <= _gridGenerator.HorizontalCellCount; x++)
            {
                for (int y = 0; y <= _gridGenerator.VerticalCellCount; y++)
                {
                    if (HasFilledEdge(x , y))
                    {
                        _gridGenerator.Edges[x,y].SetDotColor(_gameProgressModel.ThemeColor);
                    }
                    else
                    {
                        _gridGenerator.Edges[x,y].SetDotColor(_gameProgressModel.ThemeColor, true);
                    }
                }
            }
        }

        private bool HasFilledEdge(int x , int y)
        {
            var edges = _gridGenerator.Edges;
            if (edges[x,y].LeftEdgeIsOccupied || edges[x,y].DownEdgeIsOccupied)
            {
                if (!(x < _gridGenerator.HorizontalCellCount))
                {
                    if (edges[x,y].LeftEdgeIsOccupied && y < _gridGenerator.VerticalCellCount)
                    {
                        return true;
                    }
                }
                else if (!(y < _gridGenerator.VerticalCellCount))
                {
                    if (edges[x,y].DownEdgeIsOccupied && x < _gridGenerator.HorizontalCellCount)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;

                }
            }

            if (x > 0 && x <= _gridGenerator.HorizontalCellCount)
            {
                if (edges[x-1,y].DownEdgeIsOccupied)
                {
                    return true;
                }
            }

            if (y > 0 && y <= _gridGenerator.VerticalCellCount)
            {
                if (edges[x,y-1].LeftEdgeIsOccupied)
                {
                    return true;
                }
            }

            return false;
        }
        
        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<DotCheckSignal>()
                .Subscribe(OnDotCheckSignal)
                .AddTo(_disposables);
        }
    }
}