using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingPaper : MonoBehaviour
{
    private Rigidbody2D _rb;
    private EnemyEvilProf _prof;
    private PlayerController _player;

    [SerializeField] private string _direction;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _prof = GetComponentInParent<EnemyEvilProf>();
        _player = FindObjectOfType<PlayerController>();

        _direction = _prof._shootingDirection;
        _speed = 200f;
        _damage = 1;
        
        transform.SetParent(null);                          // So that the paper moves independently (from parent which is the EvilProf)

        // Rotate paper so it always faces the player
        if (_direction == "Left")
        {
            transform.Rotate(0, 0, 180);
        }
        if (_direction == "Up")
        {
            transform.Rotate(0, 0, 90);
        }
        if (_direction == "Down")
        {
            transform.Rotate(0, 0, 270);
        }
    }


    private void FixedUpdate()
    {
        if (_direction == "Left")
        {
            _rb.velocity = new Vector2(-1, 0) * _speed * Time.fixedDeltaTime;
        }
        if (_direction == "Right")
        {
            _rb.velocity = new Vector2(1, 0) * _speed * Time.fixedDeltaTime;
        }
        if (_direction == "Up")
        {
            _rb.velocity = new Vector2(0, 1) * _speed * Time.fixedDeltaTime;
        }
        if (_direction == "Down")
        {
            _rb.velocity = new Vector2(0, -1) * _speed * Time.fixedDeltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player._health -= _damage;
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }

        // Do not allow to fly through doors
        if (other.gameObject.CompareTag("TopDoor") || other.gameObject.CompareTag("BottomDoor") || other.gameObject.CompareTag("LeftDoor") || other.gameObject.CompareTag("RightDoor"))
        {
            Destroy(gameObject);
        }
    }
}
