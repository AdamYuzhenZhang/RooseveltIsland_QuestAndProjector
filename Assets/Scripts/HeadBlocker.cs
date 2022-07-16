using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBlocker : MonoBehaviour
{
    public bool blockIt;
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
        if (!isRunning) blockIt = true;
    }

    public void StopBlocking()
    {
        blockIt = false;
        blockerMat.color = new Color(0f,0f,0f,0f);
        m_Mesh.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (blockIt && !isRunning)
        {
            StartCoroutine(blockScreen());
            blockIt = false;
        }
    }

    IEnumerator blockScreen()
    {
        isRunning = true;
        m_Mesh.enabled = true;
        while (blockerMat.color.a < 1)
        {
            blockerMat.color = new Color(0f,0f,0f,blockerMat.color.a + 0.02f);
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.6f);
        while (blockerMat.color.a > 0)
        {
            blockerMat.color = new Color(0f,0f,0f,blockerMat.color.a - 0.02f);
            yield return new WaitForSeconds(0.03f);
        }

        m_Mesh.enabled = false;
        isRunning = false;
    }
}
