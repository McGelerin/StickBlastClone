using DG.Tweening;
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
		
		private Tween tween;
		
		public void Initialize(int x, int y)
		{
			SetCoordinates(x, y);
		}

		public void SetFill(bool isFill)
		{
			IsFilled = isFill;
		}

		public void OpenCloseFill(Color32 color , bool isOpen)
		{
			tween.Kill();

			if (isOpen)
			{
				_bottomSpriteRenderer.color = color;
				tween = _bottomSpriteRenderer.transform.DOScale(Vector3.one, .3f);
			}
			else
			{
				tween = _bottomSpriteRenderer.transform.DOScale(Vector3.zero, .1f);
			}
		}
		
		private void SetCoordinates(int x, int y)
		{
			_cellX = x;
			_cellY = y;
		}
	}
}