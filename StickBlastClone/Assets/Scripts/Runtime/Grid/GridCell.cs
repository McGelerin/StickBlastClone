using System.Collections.Generic;
using Runtime.Identifiers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Grid
{
	public class GridCell : SerializedMonoBehaviour
	{
		[SerializeField] private SpriteRenderer _bottomSpriteRenderer;
		
		[HideInInspector] [SerializeField] private int _cellX;
		[HideInInspector] [SerializeField] private int _cellY;
		
		public int CellX => _cellX;
		public int CellY => _cellY;
		
		public bool IsFilled { get; private set; }

		public void Initialize(float width, float height, int x, int y)
		{
			SetBottomSpriteScale(width, height);
			SetCoordinates(x, y);
		}

		private void SetFill(bool isFill)
		{
			IsFilled = isFill;
		}
		
		private void SetBottomSpriteScale(float width, float height)
		{
			_bottomSpriteRenderer.transform.localScale = new Vector3(width, height, 1f);
		}
		
		private void SetCoordinates(int x, int y)
		{
			_cellX = x;
			_cellY = y;
		}
	}
}