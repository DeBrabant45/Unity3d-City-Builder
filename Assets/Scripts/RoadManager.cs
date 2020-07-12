using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadManager
{
    public static int GetRoadNeighborStatus(Vector3 gridPosition, GridStructure grid, Dictionary<Vector3Int, GameObject> structuresToBeModified)
    {
        int roadNeighborsStatus = 0;
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var neighborPosition = grid.GetPositionOfNeighborIfExists(gridPosition, direction);
            if (neighborPosition.HasValue)
            {
                if(CheckIfNeighborHasRoadOnTheGrid(grid, neighborPosition) || CheckIfNeighborHasRoadWithinDictionary(neighborPosition, structuresToBeModified))
                {
                    roadNeighborsStatus += (int)direction;
                }
            }
        }

        return roadNeighborsStatus;
    }

    public static bool CheckIfNeighborHasRoadOnTheGrid(GridStructure grid, Vector3Int? neighborPosition)
    {
        if (grid.IsCellTaken(neighborPosition.Value))
        {
            var neighborStructureData = grid.GetStructureDataFromTheGrid(neighborPosition.Value);
            if (neighborStructureData != null && neighborStructureData.GetType() == typeof(RoadStructureSO))
            {
                return true;
            }
        }

        return false;
    }

    public static Dictionary<Vector3Int, GameObject> GetRoadNeighborsPosition(GridStructure grid, Vector3Int position)
    {
        Dictionary<Vector3Int, GameObject> dictionaryToReturn = new Dictionary<Vector3Int, GameObject>();
        List<Vector3Int?> neighborPossibleLocations = new List<Vector3Int?>();
        neighborPossibleLocations.Add(grid.GetPositionOfNeighborIfExists(position, Direction.Up));
        neighborPossibleLocations.Add(grid.GetPositionOfNeighborIfExists(position, Direction.Down));
        neighborPossibleLocations.Add(grid.GetPositionOfNeighborIfExists(position, Direction.Left));
        neighborPossibleLocations.Add(grid.GetPositionOfNeighborIfExists(position, Direction.Right));

        foreach (var possiblePosition in neighborPossibleLocations)
        {
            if(possiblePosition.HasValue)
            {
                if(CheckIfNeighborHasRoadOnTheGrid(grid, possiblePosition.Value) && dictionaryToReturn.ContainsKey(possiblePosition.Value) == false)
                {
                    dictionaryToReturn.Add(possiblePosition.Value, grid.GetStructureFromTheGrid(possiblePosition.Value));
                }
            }
        }

        return dictionaryToReturn;
    }

    public static bool CheckIfNeighborHasRoadWithinDictionary(Vector3Int? neighborPosition, Dictionary<Vector3Int, GameObject> structuresToBeModified)
    {
        if (structuresToBeModified == null)
            return false;

        return structuresToBeModified.ContainsKey(neighborPosition.Value);
    }

    public static RoadStructureHelper CheckIfStraightRoadFits(int neighborStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
    {
        if (neighborStatus == ((int)Direction.Up | (int)Direction.Down) || neighborStatus == (int)Direction.Up || neighborStatus == (int)Direction.Down)
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).prefab, RotationValue.R90);
        }
        else if (neighborStatus == ((int)Direction.Right | (int)Direction.Left) || (neighborStatus == (int)Direction.Right)
            || (neighborStatus == (int)Direction.Left) || neighborStatus == 0)
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).prefab, RotationValue.R0);
        }
        return roadToReturn;
    }

    public static RoadStructureHelper CheckIfThreewayRoadFits(int neighborStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
    {
        if (neighborStatus == ((int)Direction.Up | (int)Direction.Right | (int)Direction.Down))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).threeWayPrefab, RotationValue.R0);
        }
        else if (neighborStatus == ((int)Direction.Left | (int)Direction.Up | (int)Direction.Right))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).threeWayPrefab, RotationValue.R270);
        }
        else if (neighborStatus == ((int)Direction.Down | (int)Direction.Left | (int)Direction.Up))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).threeWayPrefab, RotationValue.R180);
        }
        else if (neighborStatus == ((int)Direction.Right | (int)Direction.Down | (int)Direction.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).threeWayPrefab, RotationValue.R90);
        }

        return roadToReturn;
    }

    public static RoadStructureHelper CheckIfFourwayRoadFits(int neighborStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
    {
        if (neighborStatus == ((int)Direction.Up | (int)Direction.Right | (int)Direction.Down | (int)Direction.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).fourWayPrefab, RotationValue.R0);
        }

        return roadToReturn;
    }

    public static RoadStructureHelper CheckIfCornerRoadFits(int neighborStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
    {
        if(neighborStatus == ((int)Direction.Up | (int)Direction.Right))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).cornerPrefab, RotationValue.R0);
        }
        else if(neighborStatus == ((int)Direction.Down | (int)Direction.Right))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).cornerPrefab, RotationValue.R90);
        }
        else if (neighborStatus == ((int)Direction.Down | (int)Direction.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).cornerPrefab, RotationValue.R180);
        }
        else if (neighborStatus == ((int)Direction.Up | (int)Direction.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).cornerPrefab, RotationValue.R270);
        }

        return roadToReturn;
    }

    public static void ModifyRoadCellsOnTheGrid(Dictionary<Vector3Int, GameObject> neighborDictionary, StructureBaseSO structureData, Dictionary<Vector3Int, GameObject> structuresToBeModified, GridStructure grid, IPlacementManager placementManager)
    {
        foreach (var keyValuePair in neighborDictionary)
        {
            grid.RemoveStrucutreFromTheGrid(keyValuePair.Key);
            placementManager.DestroySingleStructure(keyValuePair.Value);
            var roadStructure = GetCorrectRoadPrefab(keyValuePair.Key, structureData, structuresToBeModified, grid);
            var structure = placementManager.PlaceStructureOnTheMap(keyValuePair.Key, roadStructure.RoadPrefab, roadStructure.RoadPrefabRotation);
            grid.PlaceStructureOnTheGrid(structure, keyValuePair.Key, structureData);
        }
        neighborDictionary.Clear();
    }

    public static RoadStructureHelper GetCorrectRoadPrefab(Vector3 gridPosition, StructureBaseSO structureDat, Dictionary<Vector3Int, GameObject> structuresToBeModified, GridStructure grid)
    {
        var neighborStatus = RoadManager.GetRoadNeighborStatus(gridPosition, grid, structuresToBeModified);
        RoadStructureHelper roadToReturn = null;
        roadToReturn = RoadManager.CheckIfStraightRoadFits(neighborStatus, roadToReturn, structureDat);
        if (roadToReturn != null)
            return roadToReturn;
        roadToReturn = RoadManager.CheckIfCornerRoadFits(neighborStatus, roadToReturn, structureDat);
        if (roadToReturn != null)
            return roadToReturn;
        roadToReturn = RoadManager.CheckIfThreewayRoadFits(neighborStatus, roadToReturn, structureDat);
        if (roadToReturn != null)
            return roadToReturn;
        roadToReturn = RoadManager.CheckIfFourwayRoadFits(neighborStatus, roadToReturn, structureDat);
        if (roadToReturn != null)
            return roadToReturn;

        return roadToReturn;
    }
}
