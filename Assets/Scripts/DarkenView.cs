using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkenView : MonoBehaviour
{
    private Image img;
    public PlayerController PlayerController;
    private bool darkened;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponentInChildren<Image>();

    }

    private Coroutine coroutine;
    private void Update()
    {
        switch (PlayerController.currentVision)
        {
            case <= 0 when !darkened:
                darkened = true;
                coroutine = StartCoroutine(Darken());
                break;
            case > 0:
                darkened = false;
                if(coroutine != null) StopCoroutine(coroutine);
                var imgColor = img.color;
                imgColor.a = 0;
                img.color = imgColor;  
                break;
        }
    }

    IEnumerator Darken()
    {
        float t = 0f;

        while (t < 1)
        {
            float lerpValue = Mathf.InverseLerp(0, 1, t);
            var imgColor = img.color;
            imgColor.a = lerpValue;
            img.color = imgColor;  
            t += Time.deltaTime / 5f;
            yield return null;
        }
    }

}
