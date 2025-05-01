using DG.Tweening;
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

        private Tween _leftColorTween;
        private Tween _downColorTween;

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
                    if (_leftEdgeSpriteRenderer.color != color)
                    {
                        _leftColorTween?.Kill(); // Öncekini iptal et
                        _leftColorTween = _leftEdgeSpriteRenderer.DOColor(color, 0.1f);
                    }
                    break;
                case Direction.Down:
                    if (_downEdgeSpriteRenderer.color != color)
                    {
                        _downColorTween?.Kill();
                        _downColorTween = _downEdgeSpriteRenderer.DOColor(color, 0.1f);
                    }
                    break;
            }
        }

        public void SetColor(Direction direction, Color32 color,  bool setDefault = false)
        {
            switch (direction)
            {
                case Direction.Left:
                    _leftEdgeSpriteRenderer.DOColor(setDefault ? _defaultEdgeColor : color, .2f);
                    break;
                case Direction.Down:
                    _downEdgeSpriteRenderer.DOColor(setDefault ? _defaultEdgeColor : color, .2f);
                    break;
            }
        }

        public void SetDotColor(Color32 color, bool setDefault = false)
        {
            _dotSpriteRenderer.DOColor(!setDefault ? color : _defaultEdgeColor, .2f);
        }
        
        private void SetCoordinates(int x, int y)
        {
            _cellX = x;
            _cellY = y;
        }
    }
}