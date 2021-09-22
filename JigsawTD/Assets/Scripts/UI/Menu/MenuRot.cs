using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRot : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float rotSpeed = default;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime, Space.Self);
    }
}
