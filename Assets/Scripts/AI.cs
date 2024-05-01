using UnityEngine;

public class AI : Player
{
    
}

public class AIInput : I_PlayerInput
{
    private Player _player;
    private Player _aiPlayer;
    private Ball _ball;
    private string _state = "Idle";
    private Vector3 _previousMovement;
    private Vector3 _randomMovement;
    private int _count;
    private int _maxCount = 20;


    public AIInput(Player player, Player aiPlayer, Ball ball)
    {
        _player = player;
        _aiPlayer = aiPlayer;
        _ball = ball;
    }

    private bool CheckState()
    {
        if (_aiPlayer.BallWithPlayer)
        {
            _state = "BallWithPlayer";
            _count = 0;
            return true;
        }


        if (Vector3.Distance(_player.transform.position, _aiPlayer.transform.position) <= 5)
        {
            _state = "NearPlayer";
            _count = 0;
            return true;
        }
        if (Vector3.Distance(_player.transform.position, _aiPlayer.transform.position) <= 1)
        {
            _state = "CloseToPlayer";
            _count = 0;
            return true;
        }

        return false;
    }
    public Vector3 MovementInput()
    {
        switch (_state)
        {
            default:
            case "Idle":
                if (CheckState())
                {
                    return _previousMovement;
                }
                if (_count >= _maxCount)
                {
                    _count = 0;
                }
                if (_count == 0)
                {
                    _randomMovement = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f);
                }
                _previousMovement = Vector3.Lerp(_previousMovement, _randomMovement, Time.fixedDeltaTime * 10);
                _count++;
                return _previousMovement;
            case "NearPlayer":
                if (CheckState())
                {
                    return _previousMovement;
                }
                if (Vector3.Distance(_player.transform.position, _aiPlayer.transform.position) > 5)
                {
                    _state = "Idle";
                    _count = 0;
                    return _previousMovement;
                }
                if (_count >= _maxCount)
                {
                    _count = 0;
                }
                if (_count == 0)
                {
                    var direction = GetDirection(_player.transform.position, _aiPlayer.transform.position);
                    if (direction == -1)
                    {
                        _randomMovement = new Vector3(Random.Range(-1.0f, 0.0f), 0.0f);
                    }
                    else
                    {
                        _randomMovement = new Vector3(Random.Range(0.0f, 1.0f), 0.0f);
                    }
                }
                _previousMovement = Vector3.Lerp(_previousMovement, _randomMovement, Time.fixedDeltaTime * 20);
                _count++;
                return _previousMovement;
            case "CloseToPlayer":
                if (Vector3.Distance(_player.transform.position, _aiPlayer.transform.position) >= 2)
                {
                    _state = "Idle";
                    _count = 0;
                    return _previousMovement;
                }
                if (_count >= _maxCount)
                {
                    _count = 0;
                }
                if (_count == 0)
                {
                    var direction = GetDirection(_player.transform.position, _aiPlayer.transform.position);
                    if (direction == -1)
                    {
                        _randomMovement = new Vector3(Random.Range(-0.3f, 0.0f), 0.0f);
                    }
                    else
                    {
                        _randomMovement = new Vector3(Random.Range(0.0f, 0.3f), 0.0f);
                    }
                }
                _previousMovement = Vector3.Lerp(_previousMovement, _randomMovement, Time.fixedDeltaTime * 10);
                _count++;
                return _previousMovement;
            case "BallWithPlayer":
                if (!_aiPlayer.BallWithPlayer)
                {
                    _state = "Idle";
                    _count = 0;
                    return _previousMovement;
                }
                if (Vector3.Distance(_aiPlayer.transform.position, _player.transform.position) < 2.0f)
                {
                    _state = "ShootBall";
                    _count = 0;
                    return _previousMovement;
                }
                if (_count >= _maxCount)
                {
                    _count = 0;
                }
                if (_count == 0)
                {
                    var direction = GetDirection(_player.transform.position, _aiPlayer.transform.position);
                    if (direction == -1)
                    {
                        _randomMovement = new Vector3(Random.Range(-0.3f, 0.0f), 0.0f);
                    }
                    else
                    {
                        _randomMovement = new Vector3(Random.Range(0.0f, 0.3f), 0.0f);

                    }
                }
                _previousMovement = Vector3.Lerp(_previousMovement, _randomMovement, Time.fixedDeltaTime * 10);
                _count++;
                return _previousMovement;
            case "IsJumping":
                if (_aiPlayer.rb.velocity.y <= 0.1 && _aiPlayer.rb.velocity.y >= -0.1f)
                {
                    _state = "Idle";
                    _count = 0;
                    return _previousMovement;
                }
                if (_count >= _maxCount)
                {
                    _count = 0;
                }
                if (_count == 0)
                {
                    _randomMovement = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f);
                }
                _previousMovement = Vector3.Lerp(_previousMovement, _randomMovement, Time.fixedDeltaTime * 10);
                _count++;
                return _previousMovement;
            
        }
    }

    private int GetDirection(Vector3 pos1, Vector3 pos2)
    {
        Vector3 direction = pos1 - pos2;
        int dir = 0;
        if (direction.x < 0)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

        return dir;
    }

    public bool JumpInput()
    {
        if (_ball.Rigid.velocity.y > 1 && _ball.Rigid.velocity.x > 1 && Vector3.Distance(_ball.transform.position, _aiPlayer.transform.position) >= 2)
        {
            _state = "IsJumping";
            _count = 0;
            return true;
        }
        return false;
    }

    public bool KickInput()
    {
        if (_aiPlayer.BallWithPlayer)
        {
            if (_aiPlayer.Power / _aiPlayer.MaxPower > 0.5f)
            {
                return true;
            }
        }
        return false;
    }
}