using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFade : MonoBehaviour
{
    public float fadeInTime;

    private Image fadePanel;
    private Color currentColor = Color.black;

    // Use this for initialization
    void Start()
    {
        fadePanel = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad < fadeInTime)
        {
            //Fade in
            float alphaChange = Time.deltaTime / fadeInTime;
            //taking the alpha channel off and reduce by alphaChange
            currentColor.a -= alphaChange;
            //Now set the fade color
            fadePanel.color = currentColor;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
