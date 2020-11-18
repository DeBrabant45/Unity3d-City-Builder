using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableKeyboardInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.WebGLInput.captureAllKeyboardInput = false;
    }
}
