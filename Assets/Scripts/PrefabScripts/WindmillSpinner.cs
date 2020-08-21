using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSpinner : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, .5f, 0);
    }
}
