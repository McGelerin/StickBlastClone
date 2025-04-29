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

		
        public int CellX => _cellX;
        public int CellY => _cellY;
        
        public void Initialize(int x, int y, bool leftEdgeActive, bool downEdgeActive)
        {
            SetCoordinates(x, y);
            SetSpriteActive(leftEdgeActive, downEdgeActive);
        }
        
        private void SetSpriteActive(bool leftSpriteActive, bool downSpriteActive)
        {
            _leftEdgeSpriteRenderer.gameObject.SetActive(leftSpriteActive);
            _downEdgeSpriteRenderer.gameObject.SetActive(downSpriteActive);
        }
        
        private void SetCoordinates(int x, int y)
        {
            _cellX = x;
            _cellY = y;
        }
    }
}