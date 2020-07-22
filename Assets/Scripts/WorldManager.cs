using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private int _width;
    private int _length;
    private GridStructure _grid;

    public GameObject tree;
    public Transform natureParent;
    public int radius = 5;

    public GridStructure Grid { get => _grid; }

    public void PrepareWorld(int cellSize, int width, int length)
    {
        this._grid = new GridStructure(cellSize, width, length);
        this._width = width;
        this._length = length;
        PrepareTrees();
    }

    private void PrepareTrees()
    {
        TreeGenerator generator = new TreeGenerator(_width, _length, radius);
        foreach (Vector2 samplePosition in generator.Samples())
        {
            PlaceObjectOnTheMap(samplePosition, tree);
        }
    }

    private void PlaceObjectOnTheMap(Vector2 samplePosition, GameObject objectToCreate)
    {
        var positionInt = Vector2Int.CeilToInt(samplePosition);
        var positionGrid = _grid.CalculateGridPosition(new Vector3(positionInt.x, 0, positionInt.y));
        var element = Instantiate(objectToCreate, positionGrid, Quaternion.identity, natureParent);
        _grid.AddNatureToCell(positionGrid, element);
    }

    public void DestroyNatureAtLocation(Vector3 position)
    {
        var elementsToDestory = _grid.GetNaturesObjectsToRemove(position);
        foreach (var element in elementsToDestory)
        {
            Destroy(element);
        }
    }
}
