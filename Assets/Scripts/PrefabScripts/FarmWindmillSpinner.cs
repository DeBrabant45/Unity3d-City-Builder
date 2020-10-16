using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmWindmillSpinner : MonoBehaviour
{
    [SerializeField]
    private float movementSpeend = 0;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, movementSpeend, 0);
    }
}
