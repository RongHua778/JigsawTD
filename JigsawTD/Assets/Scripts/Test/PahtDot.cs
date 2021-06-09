using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PahtDot : MonoBehaviour
{
    public List<Vector2> m_Path;

    public float Speed = 0.5f;
    public int index;
    private void Update()
    {
        MoveToNext();
    }


    private void MoveToNext()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_Path[index], Speed * Time.deltaTime);
        if (((Vector2)transform.position - m_Path[index]).sqrMagnitude < 0.001f)
        {
            index++;
            if (index >= m_Path.Count)
            {
                transform.position = m_Path[0];
                index = 1;
            }
            RotateToward();
        }
    }

    private void RotateToward()
    {
        //float singleStep = Speed * Time.deltaTime;
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, (Vector2)transform.position - m_Path[index], singleStep,  0.0f);
        transform.rotation = Quaternion.LookRotation(m_Path[index] - (Vector2)transform.position);
    }



}
