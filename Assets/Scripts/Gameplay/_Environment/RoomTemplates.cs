using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    // Placing all the rooms inside this one room templates game object (much cleaner and quicker than drag & dropping all the rooms inside of every spawn point)
    public GameObject[] _topRooms;
    public GameObject[] _bottomRooms;
    public GameObject[] _leftRooms;
    public GameObject[] _rightRooms;

    public GameObject _allDirectionsRoom;

    public List<GameObject> _rooms;
}
