using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementAndShooting : MonoBehaviour
{
    private PlayerController _player;

    private Rigidbody2D _rb;
    private Animator _animatorHead;
    private Animator _animatorBody;

    [SerializeField] InputActionReference _movementInput;
    [SerializeField] private Vector2 _movement;

    [SerializeField] InputActionReference _shootingInput;
    [SerializeField] private Vector2 _shooting;

    [SerializeField] private Transform _tearsSpawnPoint;
    [SerializeField] private GameObject _tearsPrefab;

    public string _shootingDirection;

    private Vector2 _currentVelocity;
    private Vector2 _targetVelocity;



    private void OnEnable()
    {
        _movementInput.action.Enable();
        _shootingInput.action.Enable();
    }


    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();

        _rb = GetComponent<Rigidbody2D>();

        _animatorHead = transform.Find("PlayerHead").GetComponent<Animator>();      // Reference to the animator of the child object (PlayerHead)
        _animatorBody = transform.Find("PlayerBody").GetComponent<Animator>();      // Reference to the animator of the child object (PlayerBody)
    }


    private void Update()
    {
        // Read Input Values
        _movement = _movementInput.action.ReadValue<Vector2>();
        _shooting = _shootingInput.action.ReadValue<Vector2>();

        changeFacingDirection();            // Make player face movement direction
        playerShooting();                   // Manage Shooting

        // Animate Player Body
        _animatorBody.SetFloat("Velocity_X_Axis", Mathf.Abs(_movement.x));
        _animatorBody.SetFloat("Velocity_Y_Axis", Mathf.Abs(_movement.y));
    }


    private void FixedUpdate()
    {   // Manage Player Movement with a Lerp (linear interpolation: Combine a percentage of the previous vector with the next vector) -> Smooth Movement
        _currentVelocity = _rb.velocity;
        _targetVelocity = _movement * _player._movementSpeed * Time.fixedDeltaTime;
        _rb.velocity = (_currentVelocity * 0.8f) + (_targetVelocity * 0.2f);
    }

    private void changeFacingDirection()
    {
        if (_movement.x >= 0.01f)
        {
            transform.Find("PlayerHead").GetComponent<SpriteRenderer>().flipX = false;
            transform.Find("PlayerBody").GetComponent<SpriteRenderer>().flipX = false;
        }
        if (_movement.x <= -0.01f)
        {
            transform.Find("PlayerHead").GetComponent<SpriteRenderer>().flipX = true;
            transform.Find("PlayerBody").GetComponent<SpriteRenderer>().flipX = true;
        }
    }


    private void playerShooting()
    {
        if (_player._canShoot)                                                   // Make Shooting Available only after countdown is reached
        {
            if (_shooting.x == -1)
            {
                // Shoot left
                _shootingDirection = "Left";
                Instantiate(_tearsPrefab, _tearsSpawnPoint.position, transform.rotation);
                _player._canShoot = false;
                _animatorHead.SetTrigger("Shoot");
            }
            if (_shooting.x == 1)
            {
                // Shoot right
                _shootingDirection = "Right";
                Instantiate(_tearsPrefab, _tearsSpawnPoint.position, transform.rotation);
                _player._canShoot = false;
                _animatorHead.SetTrigger("Shoot");
            }
            if (_shooting.y == 1)
            {
                // Shoot up
                _shootingDirection = "Up";
                Instantiate(_tearsPrefab, _tearsSpawnPoint.position, transform.rotation);
                _player._canShoot = false;
                _animatorHead.SetTrigger("Shoot");
            }
            if (_shooting.y == -1)
            {
                // Shoot down
                _shootingDirection = "Down";
                Instantiate(_tearsPrefab, _tearsSpawnPoint.position, transform.rotation);
                _player._canShoot = false;
                _animatorHead.SetTrigger("Shoot");
            }
        }
    }


    private void OnDisable()
    {
        _movementInput.action.Disable();
        _shootingInput.action.Disable();
    }
}
