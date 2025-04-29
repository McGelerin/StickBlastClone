using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Runtime.Grid
{
	public class GridGenerator : SerializedMonoBehaviour
	{
		[OnValueChanged(nameof(GenerateGrid))] [OdinSerialize] private int horizontalCellCount;
		[OnValueChanged(nameof(GenerateGrid))] [OdinSerialize] private int verticalCellCount;
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
			
			this.CustomCellDrawing = new bool[horizontalCellCount, verticalCellCount];
		}

		private void PopulateGrid()
		{
			_grid = new GridCell[horizontalCellCount, verticalCellCount];

			for (int x = 0; x < horizontalCellCount; x++)
			{
				for (int y = 0; y < verticalCellCount; y++)
				{
					GridCell cell = Instantiate(_gridCellPrefab, transform.position, 
					                            _gridCellPrefab.transform.rotation, 
					                            transform);
					_grid[x, y] = cell;
					
					cell.gameObject.name = $"Cell {x} , {y}";
					cell.transform.parent = _gridCellHolder.transform;
					
					cell.transform.position = new Vector3((-(horizontalCellCount - 1) * cellWidth / 2f) + (cellWidth * x),
					                                      0f,
					                                      (-(verticalCellCount - 1) * cellHeight / 2f) + (cellHeight * y));
					
					cell.Initialize(cellWidth, cellHeight, x ,y);
				}
			}
		}
		
		private void PopulateEdge()
		{
			_edges = new GridEdge[horizontalCellCount + 1, verticalCellCount + 1];

			for (int x = 0; x <= horizontalCellCount; x++)
			{
				for (int y = 0; y <= verticalCellCount; y++)
				{
					GridEdge edge = Instantiate(_gridEdgePrefab, 
						transform.position, 
						_gridCellPrefab.transform.rotation, 
						transform);
					_edges[x, y] = edge;
					
					edge.gameObject.name = $"Edge {x} , {y}";
					edge.transform.parent = _gridEdgeHolder.transform;
					
					edge.transform.position = new Vector3((-(horizontalCellCount - 1) * cellWidth / 2f) + (cellWidth * x),
						0f,
						(-(verticalCellCount - 1) * cellHeight / 2f) + (cellHeight * y));
					
					edge.Initialize(x ,y , y != verticalCellCount ,x != horizontalCellCount);
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
		
		[TableMatrix(HorizontalTitle = "Custom Cell Drawing", DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false, RowHeight = 24)]
		public bool[,] CustomCellDrawing;
		
		private static bool DrawColoredEnumElement(Rect rect, bool value)
		{
			if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
			{
				value = !value;
				GUI.changed = true;
				Event.current.Use();
			}

			UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

			return value;
		}
	}
}