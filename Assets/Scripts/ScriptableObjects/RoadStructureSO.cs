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
}

public enum RotationValue
{
    R0,
    R90,
    R180,
    R270
}
