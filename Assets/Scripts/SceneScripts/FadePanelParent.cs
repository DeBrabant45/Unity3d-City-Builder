using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

}
