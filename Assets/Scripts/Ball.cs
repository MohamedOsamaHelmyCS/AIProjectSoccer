using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move with arrows/wd
/// 
/// </summary>
public class Ball : MonoBehaviour
{
    public Rigidbody2D Rigid { get; private set; }
    [SerializeField]
    private float _damage;
    [SerializeField]
    private Vector2 _originalPosition;
    private void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.Damage(_damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OutOfBounds"))
        {
            GameManager.Instance.RespawnBall();
        }
    }

    public void Reset()
    {
        transform.position = _originalPosition;
        Rigid.velocity = Vector3.zero;
    }
}