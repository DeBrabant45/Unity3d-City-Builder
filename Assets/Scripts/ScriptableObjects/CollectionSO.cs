using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New collection", menuName = "CityBuilder/CollectionSO")]
public class CollectionSO : ScriptableObject
{
    public RoadStructureSO roadStructure;
    public List<SingleStructureBaseSO> singleStructures;
    public List<ManufacturerBaseSO> manufacturers;
    public List<ZoneStructureSO> zoneStructures;

    public List<StructureBaseSO> residentialStructures;
    public List<StructureBaseSO> commercialStoreStructures;
    public List<StructureBaseSO> commercialBusinessStructures;
    public List<StructureBaseSO> agricultureStructures;

    public List<StructureBaseSO> utilitiesStructures;
    public List<StructureBaseSO> emergencyStructures;
    public List<StructureBaseSO> governmentStructures;

    public List<StructureBaseSO> manufactureStructures;

    public List<StructureBaseSO> roadStructures;
}
