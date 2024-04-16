namespace Elevator.Server;

public class ElevatorRequest
{
    public int Floor { get; set; }
    public Direction Direction { get; set; }
}

public enum Direction
{
    Up,
    Down,
    None
}