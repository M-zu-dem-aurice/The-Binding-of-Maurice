using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    [SerializeField] private GameObject[] _collectablePowerUps;

    private void Awake()
    {
        GameObject spawnObject = Instantiate(_collectablePowerUps[Random.Range(0, _collectablePowerUps.Length)], transform);
        spawnObject.transform.position = new Vector2(transform.position.x, transform.position.y);
    }
}
