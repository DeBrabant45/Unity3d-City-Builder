using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New raod structure", menuName = "CityBuilder/StructureData/RoadStructure")]
public class RoadStructureSO : StructureBaseSO
{
    [Tooltip("Road facing up and right")]
    public GameObject cornerPrefab;

    [Tooltip("Road facing up, right and down")]
    public GameObject threeWayPrefab;
    public GameObject fourWayPrefab;
    public RotationValue prefabRotation = RotationValue.R0;

    public void PrepareRoad(IEnumerable<StructureBaseSO> structuresAround)
    {
        foreach(var nearByStructure in structuresAround)
        {
            nearByStructure.PrepareStructure(new StructureBaseSO[] { this });
        }
    }

    public IEnumerable<StructureBaseSO> PrepareRoadForRemoval(IEnumerable<StructureBaseSO> structuresAround)
    {
        List<StructureBaseSO> listToReturn = new List<StructureBaseSO>();
        foreach (var nearByStructure in structuresAround)
        {
            if(nearByStructure.RoadProvider == this)
            {
                nearByStructure.RemoveRoadProivder();
                listToReturn.Add(nearByStructure);
            }
        }

        return listToReturn;
    }
}

public enum RotationValue
{
    R0,
    R90,
    R180,
    R270
}
