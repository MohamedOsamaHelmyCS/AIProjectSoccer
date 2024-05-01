using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public static Player LastPlayerTouched;
    public static bool ShouldDamage;
    private I_PlayerInput _inputHandler;
    public bool BallWithPlayer { get; private set; }
    private Ball _ball;
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _kickForce = 10.0f;
    [SerializeField]
    private E_PlayerType _playerType;
    [SerializeField]
    private Slider _healthSlider;
    [SerializeField]
    private Slider _powerSlider;
    private float _health;
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private Vector2 _originalPosition;
    private float _power;
    [SerializeField]
    private float _maxPower;

    [SerializeField] private float _jumpForce;
    [FormerlySerializedAs("_rb")] public Rigidbody2D rb;
    private bool _groundCheck = false;
    public float Health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = Mathf.Clamp(value, 0.0f, MaxHealth);
            _healthSlider.value = _health;
            if (_health == 0)
            {
                Die();
            }
        }
    }
    public float Power
    {
        get
        {
            return _power;
        }
        private set
        {
            _power = Mathf.Clamp(value, 0.0f, MaxPower);
            _powerSlider.value = _power;

        }
    }

    public float MaxPower => _maxPower;

    public float MaxHealth => _maxHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void AssignMaxPower(float max)
    {
        _maxPower = max;
        _powerSlider.maxValue = MaxPower;
    }
    public void AssignMaxHealth(float max)
    {
        _maxHealth = max;
        _healthSlider.maxValue = MaxHealth;
    }
    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
        AssignMaxPower(MaxPower);
        AssignMaxHealth(MaxHealth);
        Reset();
        // change input behaviour depending on input type
        switch (_playerType)
        {
            case E_PlayerType.Player1:
                _inputHandler = new HumanPlayer1Input();
                break;
            case E_PlayerType.Player2:
                _inputHandler = new HumanPlayer2Input();
                break;
            case E_PlayerType.AI:
                // pass in player, ai player, ball
                _inputHandler = new AIInput(GameManager.Instance.GamePlayer,this, GameManager.Instance.Ball);
                break;
            default:
                Debug.LogError("Input not supported");
                break;
        }
    }
    public void Reset()
    {
        transform.position = _originalPosition;
        gameObject.SetActive(true);
        Power = MaxPower;
        Health = MaxHealth;
    }
    public void PlayerControlFixedUpdate()
    {
        rb.velocity = new Vector2(_inputHandler.MovementInput().x * _speed, rb.velocity.y);
    }

    public void PlayerControlUpdate()
    {
        if (_inputHandler.KickInput())
        {
            KickBall();
        }
        if (_inputHandler.JumpInput())
        {
            Jump();
        }
        GainHealth(1.0f);
        GainPower(10.0f);
    }

    private void Jump()
    {
        if (_groundCheck)
        {
            _groundCheck = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + _jumpForce);
        }
    }

    private void GainHealth(float multiplier)
    {
        Health = _health + (Time.deltaTime * multiplier);
    }
    private void GainPower(float multiplier)
    {
        Power = _power + (Time.deltaTime * multiplier);
    }
    private float UsePower()
    {
        float oldPow = _power;
        Power = 0;
        return (oldPow / MaxPower);
    }
    private void KickBall()
    {
        if (BallWithPlayer)
        {
            ShouldDamage = true;
            if (transform.localScale.x < 0)
            {
                var force = Quaternion.Euler(0, 0, 45) * -transform.right;
                force.y = Mathf.Abs(force.y);
                _ball.Rigid.AddForce(force * (_kickForce * UsePower()), ForceMode2D.Impulse);
            }
            else
            {
                _ball.Rigid.AddForce(Quaternion.Euler(0, 0, 45) * transform.right * (_kickForce * UsePower()), ForceMode2D.Impulse);
            }
        }
    }
    public void InjectBall(Ball ball)
    {
        _ball = ball;
        BallWithPlayer = true;
        LastPlayerTouched = this;
    }
    public void BallLost()
    {
        BallWithPlayer = false;
    }
    public void Damage(float damageAmount)
    {
        if (LastPlayerTouched == this) return;
        if (!ShouldDamage) return;
        if (_ball.Rigid.velocity.magnitude <= 2.0f)
        {
            return;
        }

        ShouldDamage = false;
        Health = _health - damageAmount;
    }
    private void Die()
    {
        gameObject.SetActive(false);
    }

    public void Ground()
    {
        _groundCheck = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            Ground();
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OutOfBounds"))
        {
            Reset();
        }
    }
}

public enum E_PlayerType
{
    Player1,
    Player2,
    AI,
}