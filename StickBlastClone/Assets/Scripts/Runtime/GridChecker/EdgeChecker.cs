using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lofelt.NiceVibrations;
using Runtime.Audio;
using Runtime.Audio.Data;
using Runtime.Audio.Signal.Audio;
using Runtime.Data.Persistent.PlaceholderDataSO;
using Runtime.GameArea.Spawn;
using Runtime.Grid;
using Runtime.GridChecker.Signals;
using Runtime.Haptic.Signal;
using Runtime.Input.Raycasting;
using Runtime.Models;
using Runtime.PlaceHolderObject;
using Runtime.Signals;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;
using Direction = Runtime.Identifiers.Direction;

namespace Runtime.GridChecker
{
    public class EdgeChecker : IInitializable
    {
        [Inject] private GridGenerator gridGenerator;
        private GridEdge[,] gridEdges;
        private List<CachePos> _cacheGridEdgePos = new List<CachePos>();
        
        private Vector3 edgeGridOrigin;
        
        private PlaceholderSO _placeholderSo;
        private SignalBus _signalBus;
        private IGameProgressModel _gameProgressModel;
        
        public EdgeChecker(PlaceholderSO placeholderSo, SignalBus signalBus, IGameProgressModel gameProgressModel)
        {
            _placeholderSo = placeholderSo;
            _signalBus = signalBus;
            _gameProgressModel = gameProgressModel;
        }

        public void Initialize()
        {
            gridEdges = gridGenerator.Edges;
            edgeGridOrigin = gridGenerator.Edges[0, 0].transform.position;
        }

        public void CheckEdge(IClickable clickable)
        {
            if (CanPlaceObject(clickable.GetPlaceholderType(), clickable.GetPosition()))
            {
                OpenCloseHighlight(true);
            }
            else
            {
                OpenCloseHighlight(false);
                _cacheGridEdgePos.Clear();
            }
        }

        public async UniTask PlaceObjectOnGrid(IClickable clickable)
        {
            if (_cacheGridEdgePos.IsNullOrEmpty())
            {
                clickable.OnDragEnd(false);
                _signalBus.Fire(new AudioPlaySignal(AudioPlayers.Sound, Sounds.InputClick));
                _signalBus.Fire(new VibrateSignal(HapticPatterns.PresetType.MediumImpact));
            }
            else
            {
                foreach (var cachePos in _cacheGridEdgePos)
                {
                    if (cachePos.hasDownEdge)
                    {
                        var gridEdge = gridEdges[cachePos.posX,cachePos.posY];
                        gridEdge.SetOccupied(Direction.Down);
                        gridEdge.SetColor(Direction.Down, _gameProgressModel.ThemeColor);
                    }
                    if (cachePos.hasLeftEdge)
                    {
                        var gridEdge = gridEdges[cachePos.posX,cachePos.posY];
                        gridEdge.SetOccupied(Direction.Left);
                        gridEdge.SetColor(Direction.Left, _gameProgressModel.ThemeColor);
                    }
                }
                
                _signalBus.Fire(new SpawnedObjectClearSignal(clickable));
                clickable.OnDragEnd(true);
                _cacheGridEdgePos.Clear();
                _signalBus.Fire(new IncreaseScoreSignal(20));
                _signalBus.Fire(new DotCheckSignal());
                _signalBus.Fire(new CheckFillAreaSignal(false));

                await UniTask.NextFrame();
                _signalBus.Fire(new CheckLevelEndSignal());
            }
        }
        
        private bool CanPlaceObject(PlaceHolderGridObjectType placeHolderGridObjectType, Vector3 worldPos)
        {
            var placeholderSizeData = _placeholderSo.PlaceHolderData[placeHolderGridObjectType];
            List<CachePos> cacheGridEdgePos = new List<CachePos>();
            Vector2Int gridPos = WorldToGridPosition(worldPos, edgeGridOrigin, 1, 1 , placeHolderGridObjectType);
            
            for (int x = 0; x < placeholderSizeData.Count; x++)
            {
                for (int y = 0; y < placeholderSizeData[x].Count; y++)
                {
                    CachePos cachePos = new CachePos();
                    int checkX = gridPos.x + x;
                    int checkY = gridPos.y + y;

                    if (!IsInsideGrid(checkX, checkY))
                    {
                        return false;
                    }
                    
                    cachePos.posX = checkX;
                    cachePos.posY = checkY;
                    
                    if (placeholderSizeData[x][y].HasDownEdge)
                    {
                        if (gridEdges[checkX,checkY].DownEdgeIsOccupied)
                        {
                            return false;
                        }
                        
                        cachePos.hasDownEdge = true;
                    }
                    
                    if (placeholderSizeData[x][y].HasLeftEdge)
                    {
                        if (gridEdges[checkX,checkY].LeftEdgeIsOccupied)
                        {
                            return false;
                        }

                        cachePos.hasLeftEdge = true;
                    }
                    
                    cacheGridEdgePos.Add(cachePos);
                }
            }
            
            if (_cacheGridEdgePos.IsNullOrEmpty())
            {
                _cacheGridEdgePos.AddRange(cacheGridEdgePos);
            }
            else if (_cacheGridEdgePos != cacheGridEdgePos)
            {
                OpenCloseHighlight(false);
                _cacheGridEdgePos.Clear();
                _cacheGridEdgePos.AddRange(cacheGridEdgePos);
            }
            
            return true;
        }
        
        //Extentsion yapılabilir
        private Vector2Int WorldToGridPosition(Vector3 worldPos, Vector3 gridOrigin, float cellWidth, float cellHeight, PlaceHolderGridObjectType placeHolderGridObjectType)
        {
            Vector2 originOffset = _placeholderSo.PlaceholderOriginOffsets[placeHolderGridObjectType];
            
            int x = Mathf.FloorToInt((worldPos.x +  originOffset.x - gridOrigin.x) / cellWidth);
            int y = Mathf.FloorToInt((worldPos.z +  originOffset.y - gridOrigin.z) / cellHeight);
            return new Vector2Int(x, y);
        }
        
        private bool IsInsideGrid(int x, int y)
        {
            return x >= 0 && x < gridEdges.GetLength(0)  &&
                   y >= 0 && y < gridEdges.GetLength(1);
        }

        private void OpenCloseHighlight(bool isOpen)
        {
            foreach (var t in _cacheGridEdgePos)
            {
                if (t.hasDownEdge)
                {
                    gridEdges[t.posX, t.posY]
                        .OpenCloseHighlight(Direction.Down, isOpen);
                }
                
                if (t.hasLeftEdge)
                {
                    gridEdges[t.posX, t.posY]
                        .OpenCloseHighlight(Direction.Left, isOpen);
                }
            }
        }
        
    }

    class CachePos
    {
        public int posX;
        public int posY;
        public bool hasLeftEdge;
        public bool hasDownEdge;
    }
}