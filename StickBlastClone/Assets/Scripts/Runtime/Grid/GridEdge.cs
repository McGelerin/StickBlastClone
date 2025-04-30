using Runtime.Identifiers;
using Sirenix.OdinInspector;
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

        public bool LeftEdgeIsOccupied { get; private set; }
        public bool DownEdgeIsOccupied { get; private set; }
        
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
            LeftEdgeIsOccupied = false;
            DownEdgeIsOccupied = false;
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
                    LeftEdgeIsOccupied = true;
                    break;
                case Direction.Down:
                    DownEdgeIsOccupied = true;
                    break;
            }
        }

        public void ClearOccupied(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    LeftEdgeIsOccupied = false;
                    break;
                case Direction.Down:
                    DownEdgeIsOccupied = false;
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

        // private void SetDotColor(Color32 Color)
        // {
        //     
        // }
        
        private void SetCoordinates(int x, int y)
        {
            _cellX = x;
            _cellY = y;
        }
    }
}