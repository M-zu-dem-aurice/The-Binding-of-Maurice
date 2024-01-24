using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExam : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private RoomController _room;
    private PlayerController _player;
    private Transform _playerTransform;

    private int _currentRoomID;
    [SerializeField] private Vector2 _movement;
    [SerializeField] private float _speed;
    [SerializeField] private float _health;
    [SerializeField] private int _damage;
    [SerializeField] private Sprite muchHealth;
    [SerializeField] private Sprite mediumHealth;
    [SerializeField] private Sprite lowHealth;

    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _room = GetComponentInParent<RoomController>();
        _player = FindObjectOfType<PlayerController>();
        _playerTransform = _player.transform;

        _currentRoomID = _room._roomID;
        _speed = Random.Range(1f, 2.5f);
        _health = 100f;
        _damage = 1;
    }

    private void Update()
    {
        // Make enemy always face the player (rotate gameObject in direction of the player)
        Vector2 direction = _playerTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rb.rotation = angle;

        direction.Normalize();      // Make value between -1 and +1
        _movement = direction;


        // Manage Health System
        if (_health <= 75f && _health > 50f)
        {
            _spriteRenderer.sprite = muchHealth;
        }
        else if (_health <= 50f && _health > 25f)
        {
            _spriteRenderer.sprite = mediumHealth;
        }
        else if (_health <= 25f && _health > 0f)
        {
            _spriteRenderer.sprite = lowHealth;
        }
        else if (_health <= 0f)
        {
            _room._enemyAmount -= 1;
            Destroy(gameObject);
        }   
    }


    private void FixedUpdate()
    {
        // Move Enemy
        if (_currentRoomID == _player._currentRoomID)
        {
            _rb.MovePosition((Vector2)transform.position + (_movement * _speed * Time.fixedDeltaTime));
        } 
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tears"))
        {
            _health -= _player._damage;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player._health -= _damage;
        }
    }
}
