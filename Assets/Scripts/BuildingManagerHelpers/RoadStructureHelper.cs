using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadStructureHelper
{
    public RotationValue RoadPrefabRotation { get; set; }
    public GameObject RoadPrefab { get; set; }

    public RoadStructureHelper(GameObject roadPrefab, RotationValue roadPrefabRotation)
    {
        this.RoadPrefabRotation = roadPrefabRotation;
        this.RoadPrefab = roadPrefab;
    }
}