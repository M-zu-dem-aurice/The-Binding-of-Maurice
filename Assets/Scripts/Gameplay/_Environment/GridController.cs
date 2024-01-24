using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private RoomController _room;


    [System.Serializable]
    public struct Grid
    {
        public int columns, rows;
        public float verticalOffset, horizontalOffset;
    }

    private Grid _grid;
    public List<Vector2> _availablePoints = new List<Vector2>();        // List to store all positions in the grid


    void Awake()
    {
        _room = GetComponentInParent<RoomController>();

        _grid.verticalOffset = 4f;
        _grid.horizontalOffset = 8.5f;
        _grid.columns = _room._roomWidth - 2;
        _grid.rows = _room._roomHeight - 3;
        GenerateGrid();
    }

    
    private void GenerateGrid()
    {
        _grid.verticalOffset += _room.transform.localPosition.y;
        _grid.horizontalOffset += _room.transform.localPosition.x;

        
        for (int y = 0; y < _grid.rows; y++)
        {
            for (int x = 0; x < _grid.columns; x++)
            {
                _availablePoints.Add(new Vector2(x - (_grid.columns - _grid.horizontalOffset), y - (_grid.rows - _grid.verticalOffset)));
            }
        }
    } 
}
