using Runtime.Identifiers;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Runtime.Grid
{
    public class GridEdge : SerializedMonoBehaviour
    {
        [HideInInspector] [SerializeField] private int _cellX;
        [HideInInspector] [SerializeField] private int _cellY;
        
        [SerializeField] private SpriteRenderer _leftEdgeSpriteRenderer;
        [SerializeField] private SpriteRenderer _downEdgeSpriteRenderer;
        [SerializeField] private SpriteRenderer _dotSpriteRenderer;

        [OdinSerialize]private bool _leftEdgeIsOccupied;
        public bool LeftEdgeIsOccupied => _leftEdgeIsOccupied;
        
        [OdinSerialize]private bool _downEdgeIsOccupied;
        public bool DownEdgeIsOccupied => _downEdgeIsOccupied;
        
        public int CellX => _cellX;
        public int CellY => _cellY;

        public Color32 DefaultEdgeColor => _defaultEdgeColor;
        [SerializeField] private Color32 _defaultEdgeColor;
        
        public Color32 HighlightEdgeColor => _highlightEdgeColor;
        [SerializeField] private Color32 _highlightEdgeColor;
        
        public void Initialize(int x, int y, bool leftEdgeActive, bool downEdgeActive)
        {
            SetCoordinates(x, y);
            SetSpriteActive(leftEdgeActive, downEdgeActive);
            
            if (!leftEdgeActive)
            {
                SetOccupied(Direction.Left);
            }
            else
            {
                _leftEdgeIsOccupied = false;
            }

            if (!downEdgeActive)
            {
                SetOccupied(Direction.Down);
            }
            else
            {
                _downEdgeIsOccupied = false;
            }
        }
        
        private void SetSpriteActive(bool leftSpriteActive, bool downSpriteActive)
        {
            _leftEdgeSpriteRenderer.gameObject.SetActive(leftSpriteActive);
            _downEdgeSpriteRenderer.gameObject.SetActive(downSpriteActive);
        }
        
        public void SetOccupied(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    _leftEdgeIsOccupied = true;
                    break;
                case Direction.Down:
                    _downEdgeIsOccupied = true;
                    break;
            }
        }

        public void ClearOccupied(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    _leftEdgeIsOccupied = false;
                    break;
                case Direction.Down:
                    _downEdgeIsOccupied = false;
                    break;
            }
        }

        public void OpenCloseHighlight(Direction direction, bool isOpen)
        {
            Color32 color = isOpen ? _highlightEdgeColor : _defaultEdgeColor;

            switch (direction)
            {
                case Direction.Left:
                    _leftEdgeSpriteRenderer.color = color;
                    break;
                case Direction.Down:
                    _downEdgeSpriteRenderer.color = color;
                    break;
            }
        }

        public void SetColor(Direction direction, Color32 color)
        {
            switch (direction)
            {
                case Direction.Left:
                    _leftEdgeSpriteRenderer.color = color;
                    break;
                case Direction.Down:
                    _downEdgeSpriteRenderer.color = color;
                    break;
            }
        }

        public void SetDotColor(Color32 color, bool setDefault = false)
        {
            if (!setDefault)
            {
                _dotSpriteRenderer.color = color;
            }
            else
            {
                _dotSpriteRenderer.color = _defaultEdgeColor;
            }
        }
        
        private void SetCoordinates(int x, int y)
        {
            _cellX = x;
            _cellY = y;
        }
    }
}