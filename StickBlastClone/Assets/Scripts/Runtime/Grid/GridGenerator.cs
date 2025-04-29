using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Runtime.Grid
{
	public class GridGenerator : SerializedMonoBehaviour
	{
		[OnValueChanged(nameof(GenerateGrid))] [OdinSerialize] private int HorizontalCellCount;
		[OnValueChanged(nameof(GenerateGrid))] [OdinSerialize] private int VerticalCellCount;
		[OnValueChanged(nameof(GenerateGrid))] [OdinSerialize] private float CellWidth;
		[OnValueChanged(nameof(GenerateGrid))] [OdinSerialize] private float CellHeight;
		[OdinSerialize] private GridCell _gridCellPrefab;
		
		[HideInInspector] [OdinSerialize] private GridCell[,] _grid;

		public GridCell[,] grid => _grid;

		// private void Awake()
		// {
		// 	for (int x = 0; x < HorizontalCellCount; x++)
		// 	{
		// 		for (int y = 0; y < VerticalCellCount + 1; y++) //index 0 is for pathfinding
		// 		{
		// 			GridCell cell = _grid[x, y];
		// 			
		// 			cell.cellDirectionLookup[Direction.Left] = x > 0 ? _grid[x - 1, y] : null;
		// 			cell.cellDirectionLookup[Direction.Right] = x < HorizontalCellCount - 1 ? _grid[x + 1, y] : null;
		// 			cell.cellDirectionLookup[Direction.Down] = y - 1 > 0 ? _grid[x, y - 1] : null;
		// 			cell.cellDirectionLookup[Direction.Up] = y < VerticalCellCount - 1 ? _grid[x, y + 1] : null;
		//
		// 			if (cell.cellY == 0)
		// 			{
		// 				cell.gameObject.SetActive(false);
		// 			}
		// 		}
		// 	}
		// }
		
		[Button(ButtonSizes.Medium)]
		private void UpdateCells()
		{
			foreach (GridCell cell in _grid)
			{
				cell.OnCellChanged();
			}
		}

		private void GenerateGrid()
		{
			DestroyOldCells();

			PopulateGrid();
		}

		private void PopulateGrid()
		{
			_grid = new GridCell[HorizontalCellCount, VerticalCellCount + 1];

			for (int x = 0; x < HorizontalCellCount; x++)
			{
				for (int y = 0; y < VerticalCellCount + 1; y++) //index 0 is for pathfinding
				{
					GridCell cell = Instantiate(_gridCellPrefab, 
					                            transform.position, 
					                            _gridCellPrefab.transform.rotation, 
					                            transform);
					_grid[x, y] = cell;
					
					cell.gameObject.name = $"Cell {x} , {y}";
					cell.transform.parent = transform;
					
					cell.transform.position = new Vector3((-(HorizontalCellCount - 1) * CellWidth / 2f) + (CellWidth * x),
					                                      0f,
					                                      (-(VerticalCellCount - 1) * CellHeight / 2f) + (CellHeight * y));
					
					cell.Initialize(CellWidth, CellHeight, x ,y);
				}
			}
		}

		private void DestroyOldCells()
		{
			List<Transform> _cellsToDestroy = new List<Transform>();
 			
			foreach (Transform child in transform)
			{
				_cellsToDestroy.Add(child);
			}

			foreach (Transform child in _cellsToDestroy)
			{
				DestroyImmediate(child.gameObject);
			}
		}
	}
}