using System.Timers;

namespace Elevator.Server;

public class ElevatorHandler
{
    private readonly List<ElevatorRequest> _elevatorRequests = [];
    private readonly System.Timers.Timer _timer = new();
    private readonly ILogger<ElevatorHandler> _logger;
    private Direction _direction = Direction.None;
    private int _floor;

    public ElevatorHandler(ILogger<ElevatorHandler> logger)
    {
        _logger = logger;
        _timer.Interval = 1000;
        _timer.Enabled = false;
        _timer.Elapsed += TimerOnElapsed;
    }

    public void Start()
    {
        _logger.LogWarning("Starting Elevator");
        _timer.Enabled = true;
    }

    public void Stop()
    {
        _logger.LogWarning("Stopping Elevator");
        _timer.Enabled = false;
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        while (true)
        {
            if (_elevatorRequests.Count == 0) return;
            var up = _elevatorRequests.Where(x => x.Direction != Direction.Down).OrderBy(x => x.Floor).ToList();
            var down = _elevatorRequests.Where(x => x.Direction != Direction.Up).OrderByDescending(x => x.Floor).ToList();
            //var none = _elevatorRequests.Where(x => x.Direction == Direction.None).OrderBy(x => x.Floor).ToList();
            var above = _elevatorRequests.Where(x => x.Floor > _floor).OrderBy(x => x.Floor).ToList();
            var below = _elevatorRequests.Where(x => x.Floor < _floor).OrderByDescending(x => x.Floor).ToList();
            switch (_direction)
            {
                case Direction.None:
                {
                    var nextFloor = _elevatorRequests.FirstOrDefault(x => x.Floor == _floor);
                    if (nextFloor != null)
                    {
                        _logger.LogWarning("Going to floor: {floor}, Direction: {Dir}", nextFloor.Floor, nextFloor.Direction);
                        _elevatorRequests.Remove(nextFloor);
                        _direction = nextFloor.Direction;
                        return;
                    }

                    if (above.FirstOrDefault() == null)
                    {
                        nextFloor = below.First();
                        _logger.LogWarning("Going to floor: {floor}, Direction: {Dir}", nextFloor.Floor, nextFloor.Direction);
                        _elevatorRequests.Remove(nextFloor);
                        _direction = nextFloor.Direction;
                        return;
                    }

                    if (below.FirstOrDefault() == null)
                    {
                        nextFloor = above.First();
                        _logger.LogWarning("Going to floor: {floor}, Direction: {Dir}", nextFloor.Floor, nextFloor.Direction);
                        _elevatorRequests.Remove(nextFloor);
                        _direction = nextFloor.Direction;
                        return;
                    }

                    nextFloor = Math.Abs(above.First().Floor - _floor) > Math.Abs(below.First().Floor - _floor)
                        ? below.First()
                        : above.First();
                    _logger.LogWarning("Going to floor: {floor}, Direction: {Dir}", nextFloor.Floor, nextFloor.Direction);
                    _elevatorRequests.Remove(nextFloor);
                    _direction = nextFloor.Direction;
                    return;
                }
                case Direction.Up:
                {
                    var nextFloor = up.FirstOrDefault(x => x.Floor > _floor);
                    if (nextFloor == null)
                    {
                        _direction = Direction.None;
                        continue;
                    }

                    _logger.LogWarning("Going to floor: {floor}, Direction: {Dir}", nextFloor.Floor, nextFloor.Direction);
                    _elevatorRequests.Remove(nextFloor);
                    //_direction = nextFloor.Direction;
                    _floor = nextFloor.Floor;

                    return;
                }
                case Direction.Down:
                {
                    var nextFloor = down.FirstOrDefault(x => x.Floor < _floor);
                    if (nextFloor == null)
                    {
                        _direction = Direction.None;
                        continue;
                    }

                    _logger.LogWarning("Going to floor: {floor}, Direction: {Dir}", nextFloor.Floor, nextFloor.Direction);
                    _elevatorRequests.Remove(nextFloor);
                    //_direction = nextFloor.Direction;
                    _floor = nextFloor.Floor;

                    return;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void AddRequest(ElevatorRequest request)
    {
        if ((request.Direction == _direction || request.Direction == Direction.None) && request.Floor == _floor)
        {
            _logger.LogWarning("Request already satisfied");
            return;
        }

        _logger.LogWarning("Adding stop floor: {Floor}, Direction: {Dir}", request.Floor, request.Direction);
        _elevatorRequests.Add(request);
    }
}