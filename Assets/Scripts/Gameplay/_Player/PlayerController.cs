using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animatorHead;
    private Animator _animatorBody;

    [SerializeField] private InputActionReference _pauseMenuInput;
    [SerializeField] private GameObject _pauseMenuObject;

    public float _movementSpeed;
    public int _health;
    public int _amountOfHearts;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeart;
    [SerializeField] private Sprite _emptyHeart;
    public float _damage;
    public float _fireRate;
    public float _shootingCoolDown;
    public bool _canShoot;
    public int _currentRoomID;



    private void OnEnable()
    {
        _pauseMenuInput.action.Enable();
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _animatorHead = transform.Find("PlayerHead").GetComponent<Animator>();      // Reference to the animator of the child object (PlayerHead)
        _animatorBody = transform.Find("PlayerBody").GetComponent<Animator>();      // Reference to the animator of the child object (PlayerBody)

        _movementSpeed = 250f;
        _health = 3;
        _amountOfHearts = 3;
        _damage = 15f;
        _fireRate = 0.9f;
        _shootingCoolDown = _fireRate;
        _canShoot = true;
        _currentRoomID = 0;
    }


    private void Update()
    {
        // Open Pause Menu when Escape key is triggered
        if (_pauseMenuInput.action.triggered)
        {
            Time.timeScale = 0f;
            _pauseMenuObject.SetActive(true);
        }

        CheckForShootingCoolDown();         // Manage Shooting Cooldown 


        // Draw heart containers
        if (_health > _amountOfHearts)
        {
            _health = _amountOfHearts;
        }

        for (int i = 0; i < _hearts.Length; i++)
        {
            if (i < _health)
            {
                _hearts[i].sprite = _fullHeart;
            }
            else
            {
                _hearts[i].sprite = _emptyHeart;
            }

            if (i < _amountOfHearts)
            {
                _hearts[i].enabled = true;
            }
            else
            {
                _hearts[i].enabled = false;
            }
        }

        // Player dies
        if (_health <= 0)
        {
            gameObject.SetActive(false);
            Invoke("ReturnToMainMenu", 3f);
        }
    }


    private void CheckForShootingCoolDown()
    {
        if (_shootingCoolDown <= 0 && !_canShoot)
        {
            _canShoot = true;
            _shootingCoolDown = _fireRate;
        }
        else if (_shootingCoolDown > 0 && !_canShoot)
        {
            _shootingCoolDown -= Time.deltaTime;
        }
    }


    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Animate Damage Input
            _animatorHead.SetTrigger("TakingDamage");
            _animatorBody.SetTrigger("TakingDamage");

            // Push Player Back
            Vector2 dir = other.GetContact(0).point - (Vector2)transform.position;      // Calculate Angle Between the collision point and the player
            dir = -dir.normalized;                                                      // Get the opposite (-Vector2) and normalize it
            _rb.AddForce(dir * 1000);                                                   // Add force in the direction of dir and multiply it by force 
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Animate Damage Input
            _animatorHead.SetTrigger("TakingDamage");
            _animatorBody.SetTrigger("TakingDamage");

            // Push Player Back
            Vector2 dir = (Vector2)other.transform.position - (Vector2)transform.position;      // Calculate Angle Between the collision point and the player
            dir = -dir.normalized;                                                      // Get the opposite (-Vector2) and normalize it
            _rb.AddForce(dir * 1000);                                                   // Add force in the direction of dir and multiply it by force 
        }

        // If player hits open door, move player a bit to the desired direction (avoid hitting trigger of the adjacent door) + move camera to other room
        if (other.gameObject.CompareTag("TopDoor"))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 2);
            Camera.main.transform.position =
                new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 10, Camera.main.transform.position.z);
        }
        if (other.gameObject.CompareTag("BottomDoor"))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 2);
            Camera.main.transform.position =
                new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 10, Camera.main.transform.position.z);
        }
        if (other.gameObject.CompareTag("LeftDoor"))
        {
            transform.position = new Vector2(transform.position.x - 2, transform.position.y);
            Camera.main.transform.position =
                new Vector3(Camera.main.transform.position.x - 17.75f, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        if (other.gameObject.CompareTag("RightDoor"))
        {
            transform.position = new Vector2(transform.position.x + 2, transform.position.y);
            Camera.main.transform.position =
                new Vector3(Camera.main.transform.position.x + 17.75f, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
    }


    private void OnDisable()
    {
        _pauseMenuInput.action.Enable();
    }
}
