using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlyTes : MonoBehaviour
{
    Vector2 pos = new Vector2(100, 100);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
           pos, 10f * Time.deltaTime);
    }
}
