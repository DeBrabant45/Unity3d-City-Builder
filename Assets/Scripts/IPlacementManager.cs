using System.Collections.Generic;
using UnityEngine;

public interface IPlacementManager
{
    GameObject CreateGhostStructure(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue = RotationValue.R0);
    void DestroySingleStructure(GameObject structure);
    void PlaceStructuresOnTheMap(IEnumerable<GameObject> structureCollection);
    void RemoveStructures(IEnumerable<GameObject> structureCollection);
    void ResetBuildingLook(GameObject structure);
    void SetBuildingForRemoval(GameObject structureToRemove);
    GameObject PlaceStructureOnTheMap(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue);
}