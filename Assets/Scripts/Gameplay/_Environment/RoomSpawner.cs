using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int _openingDirection;
    // 1 --> need top door
    // 2 --> need bottom door
    // 3 --> need left door
    // 4 --> need right door

    private RoomTemplates _roomTemplates;
    private int _random;
    private bool _hasSpawned;

    private float _waitTime;

    private void Awake()
    {
        _roomTemplates = FindObjectOfType<RoomTemplates>();
        _hasSpawned = false;

        Invoke("SpawnRoom", 0.1f);                              // Prevent rooms from spawning every single frame -> all rooms do not spawn at the same time (collisions would not properly be detected)

        _waitTime = 2f;
        Destroy(gameObject, _waitTime);                         // Destroy spawn point after the amount of seconds stored in _waitTime -> optimization
    }

    private void SpawnRoom()
    {
        if (!_hasSpawned)                                       // Make sure that spawn point only spawns one room
        {
            if (_openingDirection == 1)
            {
                // Spawn a room with a top door
                _random = Random.Range(0, _roomTemplates._topRooms.Length);
                Instantiate(_roomTemplates._topRooms[_random], transform.position, Quaternion.identity);

            }
            else if (_openingDirection == 2)
            {
                // Spawn a room with a bottom door
                _random = Random.Range(0, _roomTemplates._bottomRooms.Length);
                Instantiate(_roomTemplates._bottomRooms[_random], transform.position, Quaternion.identity);

            }
            else if (_openingDirection == 3)
            {
                // Spawn a room with a left door
                _random = Random.Range(0, _roomTemplates._leftRooms.Length);
                Instantiate(_roomTemplates._leftRooms[_random], transform.position, Quaternion.identity);

            }
            else if (_openingDirection == 4)
            {
                // Spawn a room with a right door
                _random = Random.Range(0, _roomTemplates._rightRooms.Length);
                Instantiate(_roomTemplates._rightRooms[_random], transform.position, Quaternion.identity);

            }

            _hasSpawned = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)     // Check if room has already been instantiated at that position
    {
        if (other.CompareTag("RoomSpawnPoint"))
        {
            // Prevent the issue that when two spawn points collide with each other, none of them has the time to spawn a room before it is destroyed
            if (other.GetComponent<RoomSpawner>()._hasSpawned == false && _hasSpawned == false)
            {
                Instantiate(_roomTemplates._allDirectionsRoom, transform.position, Quaternion.identity);    // Spawn all directions room instead, because it has openings to every direction
                _hasSpawned = true;                                                                         // Preventing spawn point from spawning another room
            }
            _hasSpawned = true;
        }
    }
}
