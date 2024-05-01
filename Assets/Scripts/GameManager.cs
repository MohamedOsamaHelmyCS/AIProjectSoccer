using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player GamePlayer;
    public List<PlayerStats> Players { get; private set; } = new();
    public Ball Ball;
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private GameObject _gameOverMenu;
    [SerializeField]
    private TextMeshProUGUI _timerText;
    private float _timer;
    [SerializeField]
    private float _timeLimit;
    private bool _gameState = true;
    private float Timer
    {
        get => _timer;
        set
        {
            _timer = Mathf.Clamp(value, 0, _timeLimit);
            _timerText.text = ((int)_timer).ToString();
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        Timer = _timeLimit;
        ResetGame();
        foreach (var player in Players)
        {
            player.Score = 0;
        }
        UpdateScore();
        _gameOverMenu.gameObject.SetActive(false);
        _gameState = true;
    }
    private void Update()
    {
        if (_gameState)
        {
            Timer = _timer - Time.deltaTime;
            foreach (var player in Players)
            {
                player.Player.PlayerControlUpdate();
            }
        }
        if (_timer <= 0.0f && _gameState)
        {
            _gameState = false;
            GameOver();
        }
    }

    private void FixedUpdate()
    {
        if (_gameState)
        {
            foreach (var player in Players)
            {
                player.Player.PlayerControlFixedUpdate();
            }
        }
    }

    private void GameOver()
    {
        _gameOverMenu.gameObject.SetActive(true);
    }
    public void AddPlayer(Player player)
    {
        Players.Add(new PlayerStats()
        {
            Player = player,
            Score = 0,
        });
    }
    public void GoalScored()
    {
        if (!_gameState) return;
        PlayerStats playerStats = Players.First(x => x.Player == Player.LastPlayerTouched);
        playerStats.Score++;
        UpdateScore();
        StartCoroutine(GoalCooldown());
    }
    private void UpdateScore()
    {
        _scoreText.text = $"{Players[0].Score} \t {Players[1].Score}";
    }
    private IEnumerator GoalCooldown()
    {
        _gameState = false;
        yield return new WaitForSeconds(2.0f);
        if (_timer <= 0.0f)
        {
            yield break;
        }
        _gameState = true;
        ResetGame();
    }
    private void ResetGame()
    {
        foreach (var player in Players)
        {
            player.Player.Reset();
        }
        Ball.Reset();
    }

    public void RespawnBall()
    {
        Ball.Reset();
    }
}