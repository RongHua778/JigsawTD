using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLine : ReusableObject
{
    bool showing = false;
    [SerializeField] LineRenderer lineSR;
    [SerializeField] float pathSpeed = default;

    // Update is called once per frame
    void Update()
    {
        if (showing)
        {
            lineSR.material.SetTextureOffset("_MainTex", new Vector2(-Time.time * pathSpeed, 0));
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        showing = false;
    }

    public void ShowPath(Vector3[] points = null)
    {
        showing = true;
        lineSR.enabled = true;
        if (points != null)
        {
            lineSR.positionCount = points.Length;
            lineSR.SetPositions(points);
        }
    }

    public void HidePath()
    {
        showing = false;
        lineSR.enabled = false;
    }
}
