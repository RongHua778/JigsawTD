using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbidRange : MonoBehaviour
{
    RangeHolder detector;

    private void Awake()
    {
        detector = transform.parent.GetComponent<RangeHolder>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetPoint target = collision.GetComponent<TargetPoint>();
        if (target) detector.ForbidTarget(target);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TargetPoint target = collision.GetComponent<TargetPoint>();
        detector.UnForbidTarget(target);
    }
}
