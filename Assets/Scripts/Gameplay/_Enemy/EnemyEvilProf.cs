using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvilProf : MonoBehaviour
{
    private Rigidbody2D _rb;
    private RoomController _room;
    private PlayerController _player;

    private Animator _animatorHead;
    private Animator _animatorBody;

    private int _currentRoomID;
    [SerializeField] private float _speed;
    [SerializeField] private float _health;
    [SerializeField] private int _damage;
    [SerializeField] private Vector2 _prevPos;
    [SerializeField] private Vector2 _newPos;
    [SerializeField] private Vector2 _velocity;

    [SerializeField] private Vector2 _targetPosition;
    [SerializeField] private float _distanceToPlayer;
    private bool _isFollowingPlayer;
    private bool _isFollowingOnXAxis;

    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] private GameObject _flyingPaper;
    [SerializeField] private float _minShootWaitTime, _maxShootWaitTime;
    private float _waitTime;
    public string _shootingDirection;

    [SerializeField] private GameObject _heart;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _room = GetComponentInParent<RoomController>();
        _player = FindObjectOfType<PlayerController>();

        _animatorHead = transform.GetChild(0).GetComponent<Animator>();
        _animatorBody = transform.GetChild(1).GetComponent<Animator>();

        _currentRoomID = _room._roomID;
        _speed = 2f;
        _health = 150f;
        _damage = 2;

        _prevPos = transform.position;
        _newPos = transform.position;

        _distanceToPlayer = 2.25f;
        _isFollowingPlayer = false;
        _isFollowingOnXAxis = false;

        _minShootWaitTime = 1f;
        _maxShootWaitTime = 2f;

        StartCoroutine(ChangeFollowBool());
        StartCoroutine(ChangeFollowDir());
    }


    private void Update()
    {
        
        if (_currentRoomID == _player._currentRoomID)
        {
            DetectPlayer();
            MoveToPlayer();
        }
        

        if (_health <= 0)
        {
            // With a probability of 0.5 spawn a collectable heart, when enemy dies
            if (Random.Range(0f, 1f) < 0.5f)
            {
                GameObject spawnObject = Instantiate(_heart, transform.parent.gameObject.transform);
                spawnObject.transform.position = transform.position;
            }
            _room._enemyAmount -= 1;
            Destroy(gameObject);
        }

        
        // Animate
        _animatorHead.SetFloat("Movement_X_Axis", _velocity.x);
        _animatorHead.SetFloat("Movement_Y_Axis", _velocity.y);
        _animatorBody.SetFloat("Movement_X_Axis", _velocity.x);
        _animatorBody.SetFloat("Movement_Y_Axis", _velocity.y);
    }


    private void FixedUpdate()
    {
        CalculateVelocity();
    }


    private void MoveToPlayer()
    {
        if (_isFollowingPlayer)
        {
            _targetPosition = _player.transform.position + ((transform.position - _player.transform.position).normalized * _distanceToPlayer);      // Keep a certain distance while following the player

            if (_isFollowingOnXAxis)    // Make enemy follow player only along one axis
            {
                // Follow player only on x axis
                transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(_targetPosition.x, transform.position.y), Time.deltaTime * _speed);
            }
            else
            {
                // Follow player only on y axis
                transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(transform.position.x, _targetPosition.y), Time.deltaTime * _speed);
            }

        }
    }


    private void DetectPlayer()
    {
        if (Physics2D.Raycast(transform.position, Vector2.up, 5f, _collisionLayer))
        {
            if (Time.time > _waitTime)
            {
                _isFollowingOnXAxis = false;
                _shootingDirection = "Up";
                // Spawn objects as children of the room object
                GameObject spawnObject = Instantiate(_flyingPaper, transform);
                spawnObject.GetComponent<Transform>().position = transform.position;
                _waitTime = Time.time + Random.Range(_minShootWaitTime, _maxShootWaitTime);
            }
        }
        if (Physics2D.Raycast(transform.position, Vector2.down, 5f, _collisionLayer))
        {
            if (Time.time > _waitTime)
            {
                _isFollowingOnXAxis = false;
                _shootingDirection = "Down";
                // Spawn objects as children of the room object
                GameObject spawnObject = Instantiate(_flyingPaper, transform);
                spawnObject.GetComponent<Transform>().position = transform.position;
                _waitTime = Time.time + Random.Range(_minShootWaitTime, _maxShootWaitTime);
            }
        }
        if (Physics2D.Raycast(transform.position, Vector2.left, 7.5f, _collisionLayer))
        {
            if (Time.time > _waitTime)
            {
                _isFollowingOnXAxis = false;
                _shootingDirection = "Left";
                // Spawn objects as children of the room object
                GameObject spawnObject = Instantiate(_flyingPaper, transform);
                spawnObject.GetComponent<Transform>().position = transform.position;
                _waitTime = Time.time + Random.Range(_minShootWaitTime, _maxShootWaitTime);
            }
        }
        if (Physics2D.Raycast(transform.position, Vector2.right, 7.5f, _collisionLayer))
        {
            if (Time.time > _waitTime)
            {
                _isFollowingOnXAxis = false;
                _shootingDirection = "Right";
                // Spawn objects as children of the room object
                GameObject spawnObject = Instantiate(_flyingPaper, transform);
                spawnObject.GetComponent<Transform>().position = transform.position;
                _waitTime = Time.time + Random.Range(_minShootWaitTime, _maxShootWaitTime);
            }
        }
    }


    private IEnumerator ChangeFollowBool()
    {
        // Add some randomness to enemy movement
        float random = Random.Range(0.25f, 1.25f);
        _isFollowingPlayer = !_isFollowingPlayer;

        yield return new WaitForSeconds(random);

       StartCoroutine(ChangeFollowBool());
    }


    private IEnumerator ChangeFollowDir()
    {
        // Add some randomness to enemy movement
        float random = Random.Range(2f, 4f);
        _isFollowingOnXAxis = !_isFollowingOnXAxis;

        yield return new WaitForSeconds(random);

        StartCoroutine(ChangeFollowDir());
    }


    private void CalculateVelocity()
    {
        _newPos = transform.position;                               // each frame track the new position
        _velocity = (_newPos - _prevPos) / Time.fixedDeltaTime;     // velocity = dist/time
        _prevPos = _newPos;                                         // update position for next frame calculation
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player._health -= _damage;
        }
    }


    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tears"))
        {
            _health -= _player._damage;
            Destroy(other.gameObject);


            // Damage animation
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.red;

            yield return new WaitForSeconds(.2f);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
