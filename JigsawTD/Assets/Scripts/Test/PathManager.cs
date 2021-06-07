using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField]
    Transform[] wayPoints;

    [SerializeField]
    PathPoint pathPoint = default;

    List<Vector2> path;

    private void Start()
    {
        GenerateVectorPath();

    }

    private void GenerateVectorPath()
    {
        path = new List<Vector2>();
        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            Vector2 pos = wayPoints[i].position + 0.5f * (wayPoints[i + 1].position - wayPoints[i].position);
            path.Add(pos);
        }

        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            PathPoint pathpoint = Instantiate(pathPoint, wayPoints[i].position, Quaternion.identity);
            pathpoint.m_Path = path;
            pathpoint.index = i;
        }
    }



}
