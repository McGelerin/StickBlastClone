using System.Collections.Generic;
using Runtime.Identifiers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Grid
{
	public class GridCell : SerializedMonoBehaviour
	{
		[SerializeField] private Transform _bottomSpriteHolder;
		
		[HideInInspector] [SerializeField] private int _cellX;
		[HideInInspector] [SerializeField] private int _cellY;
		
		public int cellX => _cellX;
		public int cellY => _cellY;

		public void Initialize(float width, float height, int x, int y)
		{
			SetBottomSpriteScale(width, height);
			SetCoordinates(x, y);
			OnCellChanged();
		}

		private void SetBottomSpriteScale(float width, float height)
		{
			_bottomSpriteHolder.transform.localScale = new Vector3(width, height, 1f);
		}

		public void OnCellChanged()
		{
			if (Application.isPlaying) return;
		}
		

		private void SetCoordinates(int x, int y)
		{
			_cellX = x;
			_cellY = y;
		}
	}
}