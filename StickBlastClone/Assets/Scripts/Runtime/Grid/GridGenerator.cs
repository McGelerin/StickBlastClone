using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Runtime.Grid
{
	public class GridGenerator : SerializedMonoBehaviour
	{
		[Range(0,6)][OnValueChanged(nameof(GenerateGrid))] [OdinSerialize] private int horizontalCellCount;
		[Range(0,6)][OnValueChanged(nameof(GenerateGrid))] [OdinSerialize] private int verticalCellCount;
		private float cellWidth = 1f;
		private float cellHeight = 1f;
		
		[OdinSerialize] private Transform _gridCellHolder;
		[OdinSerialize] private Transform _gridEdgeHolder;
		
		[OdinSerialize] private GridCell _gridCellPrefab;
		[OdinSerialize] private GridEdge _gridEdgePrefab;
		
		[HideInInspector] [OdinSerialize] private GridCell[,] _grid;
		[HideInInspector] [OdinSerialize] private GridEdge[,] _edges;

		public GridCell[,] Grid => _grid;
		public GridEdge[,] Edges => _edges;

		public int HorizontalCellCount => horizontalCellCount;
		public int VerticalCellCount => verticalCellCount;


		// [Button(ButtonSizes.Medium)]
		// private void UpdateCells()
		// {
		// 	foreach (GridCell cell in _grid)
		// 	{
		// 		cell.OnCellChanged();
		// 	}
		// }

		private void GenerateGrid()
		{
			DestroyOldCells();

			PopulateGrid();
			PopulateEdge();
		}

		private void PopulateGrid()
		{
			_grid = new GridCell[HorizontalCellCount, VerticalCellCount];

			for (int x = 0; x < HorizontalCellCount; x++)
			{
				for (int y = 0; y < VerticalCellCount; y++)
				{
					GridCell cell = Instantiate(_gridCellPrefab, transform.position, 
					                            _gridCellPrefab.transform.rotation, 
					                            transform);
					_grid[x, y] = cell;
					
					cell.gameObject.name = $"Cell {x} , {y}";
					cell.transform.parent = _gridCellHolder.transform;
					
					cell.transform.position = new Vector3((-(HorizontalCellCount - 1) * cellWidth / 2f) + (cellWidth * x),
					                                      0f,
					                                      (-(VerticalCellCount - 1) * cellHeight / 2f) + (cellHeight * y));
					
					cell.Initialize(x ,y);
				}
			}
		}
		
		private void PopulateEdge()
		{
			_edges = new GridEdge[HorizontalCellCount + 1, VerticalCellCount + 1];

			for (int x = 0; x <= HorizontalCellCount; x++)
			{
				for (int y = 0; y <= VerticalCellCount; y++)
				{
					GridEdge edge = Instantiate(_gridEdgePrefab, 
						transform.position, 
						_gridCellPrefab.transform.rotation, 
						transform);
					_edges[x, y] = edge;
					
					edge.gameObject.name = $"Edge {x} , {y}";
					edge.transform.parent = _gridEdgeHolder.transform;
					
					edge.transform.position = new Vector3((-(HorizontalCellCount - 1) * cellWidth / 2f) + (cellWidth * x),
						0f,
						(-(VerticalCellCount - 1) * cellHeight / 2f) + (cellHeight * y));
					
					edge.Initialize(x ,y , y != VerticalCellCount ,x != HorizontalCellCount);
				}
			}
		}

		private void DestroyOldCells()
		{
			List<Transform> _cellsToDestroy = new List<Transform>();
			
			foreach (Transform child in transform)
			{
				foreach (Transform grids in child)
				{
					_cellsToDestroy.Add(grids);
				}
			}

			foreach (Transform child in _cellsToDestroy)
			{
				DestroyImmediate(child.gameObject);
			}
		}
	}
}