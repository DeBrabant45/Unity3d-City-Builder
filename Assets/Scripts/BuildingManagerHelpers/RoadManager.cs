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

    public static bool CheckIfNeighborHasRoadWithinDictionary(Vector3Int? neighborPosition, Dictionary<Vector3Int, GameObject> structuresToBeModified)
    {
        return structuresToBeModified.ContainsKey(neighborPosition.Value);
    }

    internal static RoadStructureHelper CheckIfStraightRoadFits(int neighborStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
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

    internal static RoadStructureHelper CheckIfThreewayRoadFits(int neighborStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
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

    internal static RoadStructureHelper CheckIfFourwayRoadFits(int neighborStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
    {
        if (neighborStatus == ((int)Direction.Up | (int)Direction.Right | (int)Direction.Down | (int)Direction.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).fourWayPrefab, RotationValue.R0);
        }

        return roadToReturn;
    }

    internal static RoadStructureHelper CheckIfCornerRoadFits(int neighborStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
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
}
