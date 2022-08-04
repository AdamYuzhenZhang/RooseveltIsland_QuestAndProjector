using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBlocker : MonoBehaviour
{
    private MeshRenderer m_Mesh;
    private Material blockerMat;
    private bool isRunning;

    private void Start()
    {
        m_Mesh = GetComponent<MeshRenderer>();
        blockerMat = m_Mesh.sharedMaterial;
        m_Mesh.enabled = false;
    }

    public void BlockTheView()
    {
        StartCoroutine(blockScreen());
    }

    public void StopBlocking()
    {
        m_Mesh.enabled = false;
        StartCoroutine(StopBlock());
    }
    
    public void ManuallyBlocked()
    {
        m_Mesh.enabled = true;
        // while (blockerMat.color.a < 1)
        // {
        //     blockerMat.color = new Color(0f, 0f, 0f, blockerMat.color.a + 0.02f*Time.deltaTime);
        // }
        StartCoroutine(ManuallyBlock());
    }

    IEnumerator blockScreen()
    {
        m_Mesh.enabled = true;
        while (blockerMat.color.a < 1)
        {
            blockerMat.color = new Color(0f, 0f, 0f, blockerMat.color.a + 0.02f);
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.6f);
        while (blockerMat.color.a > 0)
        {
            blockerMat.color = new Color(0f, 0f, 0f, blockerMat.color.a - 0.02f);
            yield return new WaitForSeconds(0.03f);
        }

        m_Mesh.enabled = false;
    }

    IEnumerator ManuallyBlock()
    {
        while (blockerMat.color.a < 1)
        {
            blockerMat.color = new Color(0f, 0f, 0f, blockerMat.color.a + 0.02f);
            yield return new WaitForSeconds(0.03f);
        }
    }

    IEnumerator StopBlock()
    {
        while (blockerMat.color.a > 0)
        {
            blockerMat.color = new Color(0f, 0f, 0f, blockerMat.color.a - 0.02f);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
