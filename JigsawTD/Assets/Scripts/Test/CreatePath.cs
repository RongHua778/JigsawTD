using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Utility;

public class CreatePath : MonoBehaviour
{
    public PathCreator pathCreator;
    public Transform[] waypoints;
    void Start()
    {
        pathCreator = this.GetComponent<PathCreator>();
        if (waypoints.Length > 0)
        {
            // Create a new bezier path from the waypoints.
            BezierPath bezierPath = new BezierPath(waypoints, false, PathSpace.xyz);
            GetComponent<PathCreator>().bezierPath = bezierPath;
        }
    }
}
