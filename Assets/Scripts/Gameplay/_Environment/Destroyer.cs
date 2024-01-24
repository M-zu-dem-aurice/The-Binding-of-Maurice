using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.gameObject.CompareTag("RoomSpawnPoint") || other.gameObject.CompareTag("Environment") || other.gameObject.CompareTag("Enemy"))    // Destroy all spawn points and environment game objects that collide with it
        {
            Destroy(other.gameObject);                  // Room Spawn point: so that another entry room does not spawn on top of the first one
                                                        // Environment: so that it can be used to keep doors free from obstacles
        }
        
    }
}
