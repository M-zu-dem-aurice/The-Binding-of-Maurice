using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tears : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerMovementAndShooting _playerShooting;

    [SerializeField] private float _shootingSpeed;
    private float _accelerationXAxis;
    private float _accelerationYAxis;
    private string _shootingDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _playerShooting = FindObjectOfType<PlayerMovementAndShooting>();

        _shootingSpeed = 200f;                  // Standard tear velocity (when player is not moving)

        _shootingDirection = _playerShooting._shootingDirection;


        // Make velocity of tear faster, when player is moving towards shooting direction and slower, when player is moving contrary to shooting direction
        if (_shootingDirection == "Left")
        {
            _accelerationXAxis = _shootingSpeed + (-15 * _playerShooting.GetComponent<Rigidbody2D>().velocity.x);
        }
        else
        {
            _accelerationXAxis = _shootingSpeed + (15 * _playerShooting.GetComponent<Rigidbody2D>().velocity.x);
        }

        if (_shootingDirection == "Down")
        {
            _accelerationYAxis = _shootingSpeed + (-15 * _playerShooting.GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            _accelerationYAxis = _shootingSpeed + (15 * _playerShooting.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    private void FixedUpdate()
    {
        if (_shootingDirection == "Left")
        {
            _rb.velocity = new Vector2(-1, 0) * _accelerationXAxis * Time.fixedDeltaTime;
        }
        if (_shootingDirection == "Right")
        {
            _rb.velocity = new Vector2(1, 0) * _accelerationXAxis * Time.fixedDeltaTime;
        }
        if (_shootingDirection == "Up")
        {
            _rb.velocity = new Vector2(0, 1) * _accelerationYAxis * Time.fixedDeltaTime;
        }
        if (_shootingDirection == "Down")
        {
            _rb.velocity = new Vector2(0, -1) * _accelerationYAxis * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }

        // Do not allow tears to fly through doors
        if (other.gameObject.CompareTag("TopDoor") || other.gameObject.CompareTag("BottomDoor") || other.gameObject.CompareTag("LeftDoor") || other.gameObject.CompareTag("RightDoor"))
        {
            Destroy(gameObject);
        }
    }
}
