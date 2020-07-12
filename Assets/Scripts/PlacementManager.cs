using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour, IPlacementManager
{
    public Transform ground;
    public Material transparentMaterial;

    private Dictionary<GameObject, Material[]> _originalMaterials = new Dictionary<GameObject, Material[]>();

    public GameObject CreateGhostStructure(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue = RotationValue.R0)
    {
        GameObject newStructure = PlaceStructureOnTheMap(gridPosition, buildingPrefab, rotationValue);
        Color colorToSet = Color.green;
        ModifyStructurePrefablook(newStructure, colorToSet);
        return newStructure;
    }

    public GameObject PlaceStructureOnTheMap(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue)
    {

        GameObject newStrcuture = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
        Vector3 rotation = Vector3.zero;
        switch (rotationValue)
        {
            case RotationValue.R0:
                break;
            case RotationValue.R90:
                rotation = new Vector3(0, 90, 0);
                break;
            case RotationValue.R180:
                rotation = new Vector3(0, 180, 0);
                break;
            case RotationValue.R270:
                rotation = new Vector3(0, 270, 0);
                break;
            default:
                break;
        }

        foreach (Transform child in newStrcuture.transform)
        {
            child.Rotate(rotation, Space.World);
        }

        return newStrcuture;
    }

    private void ModifyStructurePrefablook(GameObject newStructure, Color colorToSet)
    {
        foreach (Transform child in newStructure.transform)
        {
            var renderer = child.GetComponent<MeshRenderer>();
            if (_originalMaterials.ContainsKey(child.gameObject) == false)
            {
                _originalMaterials.Add(child.gameObject, renderer.materials);
            }
            Material[] materialsToSet = new Material[renderer.materials.Length];
            colorToSet.a = 0.5f;
            for (int i = 0; i < materialsToSet.Length; i++)
            {
                materialsToSet[i] = transparentMaterial;
                materialsToSet[i].color = colorToSet;
            }
            renderer.materials = materialsToSet;
        }
    }

    public void PlaceStructuresOnTheMap(IEnumerable<GameObject> structureCollection)
    {
        foreach (var structure in structureCollection)
        {
            ResetBuildingLook(structure);
        }
        _originalMaterials.Clear();
    }

    public void ResetBuildingLook(GameObject structure)
    {
        foreach (Transform child in structure.transform)
        {
            var renderer = child.GetComponent<MeshRenderer>();
            if (_originalMaterials.ContainsKey(child.gameObject))
            {
                renderer.materials = _originalMaterials[child.gameObject];
            }
        }
    }

    public void RemoveStructures(IEnumerable<GameObject> structureCollection)
    {
        foreach (var structure in structureCollection)
        {
            DestroySingleStructure(structure);
        }
        _originalMaterials.Clear();
    }

    public void SetBuildingForRemoval(GameObject structureToRemove)
    {
        Color colorToSet = Color.red;
        ModifyStructurePrefablook(structureToRemove, colorToSet);
    }

    public void DestroySingleStructure(GameObject structure)
    {
        Destroy(structure);
    }

    public GameObject MoveStructureOnTheMap(Vector3Int positionToPlaceStructure, GameObject gameObjectToReuse, GameObject prefab)
    {
        gameObjectToReuse.transform.position = positionToPlaceStructure;
        gameObjectToReuse.transform.rotation = prefab.transform.rotation;
        for (int i = 0; i < gameObjectToReuse.transform.childCount; i++)
        {
            gameObjectToReuse.transform.GetChild(i).rotation = prefab.transform.GetChild(i).rotation;
        }

        return gameObjectToReuse;
    }
}
