using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBlocker : MonoBehaviour
{
    public Image image;

    public bool blockIt;


    public void BlockTheView()
    {
        blockIt = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (blockIt)
        {
            StartCoroutine(blockScreen());
            blockIt = false;
        }
    }

    IEnumerator blockScreen()
    {
        while (image.color.a < 1)
        {
            image.color = new Color(0f,0f,0f,image.color.a + 0.02f);
            yield return new WaitForSeconds(0.05f);
        }
        
        while (image.color.a > 0)
        {
            image.color = new Color(0f,0f,0f,image.color.a - 0.02f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
