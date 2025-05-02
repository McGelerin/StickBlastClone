using System.Collections.Generic;
using Runtime.Grid;
using Runtime.GridChecker.Signals;
using Runtime.Infrastructures.Template;
using Runtime.Laser;
using Runtime.Signals;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using Zenject;
using Direction = Runtime.Identifiers.Direction;

namespace Runtime.GridChecker
{
    public class BlastChecker : SignalListener
    {
        [Inject] private GridGenerator _gridGenerator;
        
        List<int> _isRowBlast = new List<int>();
        List<int> _isColumnBlast = new List<int>();
        private float _vfxLifetime = .2f;

        private readonly LaserVFX.Factory _laserVFxFactory;

        public BlastChecker(LaserVFX.Factory laserVFxFactory)
        {
            _laserVFxFactory = laserVFxFactory;
        }

        private void OnBlastCheck(BlastCheckSignal signal)
        {
            _isRowBlast.Clear();
            _isColumnBlast.Clear();

            ColumnCheck();
            RowCheck();

            RemoveObjectFromGrid();
        }

        private void RemoveObjectFromGrid()
        {
            if (_isRowBlast.IsNullOrEmpty() && _isColumnBlast.IsNullOrEmpty()) return;

            if (!_isColumnBlast.IsNullOrEmpty())
            {
                RemoveColumn();
            }

            if (!_isRowBlast.IsNullOrEmpty())
            {
                RemoveRow();
            }
            
            _signalBus.Fire(new DotCheckSignal());
        }

        private void RemoveColumn()
        {
            GridCell[,] gridCells = _gridGenerator.Grid;
            GridEdge[,] edges = _gridGenerator.Edges;
            
            foreach (int index in _isColumnBlast)
            {
                for (int y = 0; y < _gridGenerator.VerticalCellCount; y++)
                {
                    gridCells[index, y].SetFill(false);
                    gridCells[index,y].SetColor(default,true);
                    
                    
                    edges[index, y].ClearOccupied(Direction.Down);
                    edges[index, y].SetColor(Direction.Down, default, true);

                    if (index == 0)
                    {
                        edges[index, y].ClearOccupied(Direction.Left);
                        edges[index, y].SetColor(Direction.Left, default, true);

                        if (!gridCells[index + 1 , y].IsFilled)
                        {
                            edges[index + 1 , y].ClearOccupied(Direction.Left);
                            edges[index + 1, y].SetColor(Direction.Left, default, true);
                        }
                    }
                    else if (index > 0 && index < _gridGenerator.VerticalCellCount - 1)
                    {
                        if (!gridCells[index - 1 , y].IsFilled)
                        {
                            edges[index , y].ClearOccupied(Direction.Left);
                            edges[index , y].SetColor(Direction.Left, default, true);
                        }

                        if (!gridCells[index + 1 , y].IsFilled)
                        {
                            edges[index + 1 , y].ClearOccupied(Direction.Left);
                            edges[index + 1, y].SetColor(Direction.Left, default, true);
                        }
                    }
                    else if (index == _gridGenerator.HorizontalCellCount - 1)
                    {
                        edges[index + 1 , y].ClearOccupied(Direction.Left);
                        edges[index + 1 , y].SetColor(Direction.Left, default, true);
                        
                        if (!gridCells[index - 1 , y].IsFilled)
                        {
                            edges[index , y].ClearOccupied(Direction.Left);
                            edges[index , y].SetColor(Direction.Left, default, true);
                        }
                    }
                }
                
                edges[index, _gridGenerator.VerticalCellCount].ClearOccupied(Direction.Down);
                edges[index, _gridGenerator.VerticalCellCount].SetColor(Direction.Down, default, true);

                //LaserVfx
                Vector3 laserPos = edges[index, _gridGenerator.VerticalCellCount / 2].transform.position;
                var laser = _laserVFxFactory.Create(false, _vfxLifetime);
                laser.transform.position = laserPos;
                _signalBus.Fire(new IncreaseScoreSignal(100));
            }
        }

        private void RemoveRow()
        {
            GridCell[,] gridCells = _gridGenerator.Grid;
            GridEdge[,] edges = _gridGenerator.Edges;
            
            foreach (int index in _isRowBlast)
            {
                for (int x = 0; x < _gridGenerator.HorizontalCellCount; x++)
                {
                    gridCells[x, index].SetFill(false);
                    gridCells[x, index].SetColor(default,true);
                    
                    edges[x, index].ClearOccupied(Direction.Left);
                    edges[x , index].SetColor(Direction.Left, default, true);

                    switch (index)
                    {
                        case 0:
                        {
                            edges[x,index].ClearOccupied(Direction.Down);
                            edges[x , index].SetColor(Direction.Down, default, true);

                            if (!gridCells[x, index + 1].IsFilled)
                            {
                                edges[x, index + 1].ClearOccupied(Direction.Down);
                                edges[x , index + 1].SetColor(Direction.Down, default, true);
                            }

                            break;
                        }
                        case > 0 when index < _gridGenerator.VerticalCellCount - 1:
                        {
                            if (!gridCells[x, index - 1].IsFilled)
                            {
                                edges[x, index].ClearOccupied(Direction.Down);
                                edges[x , index].SetColor(Direction.Down, default, true);
                            }

                            if (!gridCells[x, index + 1].IsFilled)
                            {
                                edges[x, index + 1].ClearOccupied(Direction.Down);
                                edges[x , index + 1].SetColor(Direction.Down, default, true);
                            }

                            break;
                        }
                        default:
                        {
                            if (index == _gridGenerator.VerticalCellCount - 1)
                            {
                                edges[x, index + 1].ClearOccupied(Direction.Down);
                                edges[x , index + 1].SetColor(Direction.Down, default, true);
                        
                                if (!gridCells[x, index - 1].IsFilled)
                                {
                                    edges[x, index].ClearOccupied(Direction.Down);
                                    edges[x , index].SetColor(Direction.Down, default, true);
                                }
                            }

                            break;
                        }
                    }
                }
                
                edges[_gridGenerator.HorizontalCellCount , index].ClearOccupied(Direction.Left);
                edges[_gridGenerator.HorizontalCellCount , index].SetColor(Direction.Left, default, true);
                
                //LaserVFX
                Vector3 laserPos = edges[_gridGenerator.HorizontalCellCount / 2 , index].transform.position;
                var laser = _laserVFxFactory.Create(true, _vfxLifetime);
                laser.transform.position = laserPos;
                _signalBus.Fire(new IncreaseScoreSignal(100));
            }
        }
        
        private void ColumnCheck()
        {
            bool isBlastable;
            for (int x = 0; x < _gridGenerator.HorizontalCellCount; x++)
            {
                isBlastable = true;
                for (int y = 0; y < _gridGenerator.VerticalCellCount; y++)
                {
                    if (!_gridGenerator.Grid[x,y].IsFilled)
                    {
                        isBlastable = false;
                        break;
                    }
                }

                if (isBlastable)
                {
                    _isColumnBlast.Add(x);
                }
            }
        }
        
        private void RowCheck()
        {
            bool isBlastable;
            for (int y = 0; y < _gridGenerator.VerticalCellCount; y++)
            {
                isBlastable = true;
                for (int x = 0; x < _gridGenerator.HorizontalCellCount; x++)
                {
                    if (!_gridGenerator.Grid[x,y].IsFilled)
                    {
                        isBlastable = false;
                        break;
                    }
                }

                if (isBlastable)
                {
                    _isRowBlast.Add(y);
                }
            }
        }
        
        
        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<BlastCheckSignal>()
                .Subscribe(OnBlastCheck)
                .AddTo(_disposables);
        }
    }
}