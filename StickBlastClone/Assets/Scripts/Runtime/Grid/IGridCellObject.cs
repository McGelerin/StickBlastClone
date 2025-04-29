namespace Runtime.Grid
{
	public interface IGridCellObject
	{
		public int cellX { get; }
		public int cellY { get; }
		public void SetCoordinates(int x, int y);
	}
}