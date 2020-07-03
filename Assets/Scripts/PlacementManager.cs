using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour, IPlacementManager
{
    public Transform ground;
    public Material transparentMaterial;

    private Dictionary<GameObject, Material[]> _originalMaterials = new Dictionary<GameObject, Material[]>();

    public GameObject CreateGhostStructure(Vector3 gridPosition, GameObject buildingPrefab)
    {
        GameObject newStructure = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
        Color colorToSet = Color.green;
        ModifyStructurePrefablook(newStructure, colorToSet);
        return newStructure;
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
}
