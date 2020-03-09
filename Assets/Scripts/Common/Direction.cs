public enum Direction
{
	None = -1,
	Up = 0,
	Right = 1,
	Down = 2,
	Left = 3
}

public static class DirectionHelper
{
	public static Direction OppositeDirection(this Direction direction)
	{
		switch (direction)
		{
			case Direction.None:
				return Direction.None;
			case Direction.Up:
				return Direction.Down;
			case Direction.Right:
				return Direction.Left;
			case Direction.Down:
				return Direction.Up;
			case Direction.Left:
				return Direction.Right;
			default:
				return Direction.None;
		}
	}
	public static Direction NextDirection(this Direction direction)
	{
		switch (direction)
		{
			case Direction.None:
				return Direction.None;
			case Direction.Up:
				return Direction.Right;
			case Direction.Right:
				return Direction.Down;
			case Direction.Down:
				return Direction.Left;
			case Direction.Left:
				return Direction.Up;
			default:
				return Direction.None;
		}
	}
}