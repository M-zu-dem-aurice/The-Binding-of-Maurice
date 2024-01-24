using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private PlayerController _player;
    private RoomTemplates _roomTemplates;
    private GridController _grid;
    [SerializeField] private GameObject[] _spawnObjects;

    public int _roomID;
    [SerializeField] private float _probabilityToSpawnObjects;
    public int _roomWidth;
    public int _roomHeight;
    [SerializeField] private GameObject[] _previousOpenedDoors;
    public int _enemyAmount;
    [SerializeField] private Sprite _closedDoor;
    [SerializeField] private Sprite _openedDoor;


    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _roomTemplates = FindObjectOfType<RoomTemplates>();
        _grid = GetComponentInChildren<GridController>();
        _roomTemplates._rooms.Add(this.gameObject);                     // Add spawned room to the rooms list in script "RoomTemplates" -> last room that will be added to the list, is the exit room

        _roomID = Random.Range(1, 100000);
        _probabilityToSpawnObjects = 0.05f;
        _roomWidth = 18;
        _roomHeight = 10;
        _enemyAmount = 0;



        Invoke("SpawnObjects", 0.5f);
        Invoke("CountEnemiesInRoom", 1f);                                // Delayed, so that enemies that are deleted by "death zones" do not count
    }


    private void Update()
    {
        // Close doors if enemies are in the room and player is in the room
        if (_enemyAmount > 0 && _roomID == _player._currentRoomID)
        {
            foreach (GameObject _door in _previousOpenedDoors)
            {
                _door.GetComponent<BoxCollider2D>().isTrigger = false;
                _door.GetComponent<SpriteRenderer>().sprite = _closedDoor;
            }
        } else
        {
            foreach (GameObject _door in _previousOpenedDoors)
            {
                _door.GetComponent<BoxCollider2D>().isTrigger = true;
                _door.GetComponent<SpriteRenderer>().sprite = _openedDoor;
            }
        }
    }


    private void SpawnObjects()
    {
        foreach (Vector2 _spawnPoint in _grid._availablePoints)
        {
            // Spawn a (random) gameObject on every point of the grid, only if a certain probability is met
            float random = Random.Range(0f, 1f);

            if (_probabilityToSpawnObjects >= random)
            {
                // Spawn objects as children of the room object
                GameObject spawnObject = Instantiate(_spawnObjects[Random.Range(0, _spawnObjects.Length)], transform);
                spawnObject.GetComponent<Transform>().position = _spawnPoint;
            }
        }
    }


    private void CountEnemiesInRoom()
    {

        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Enemy"))
            {
                _enemyAmount += 1;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player._currentRoomID = _roomID;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player._currentRoomID = 0;
        }
    }


    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_roomWidth, _roomHeight, 0));
    }
    */
}
