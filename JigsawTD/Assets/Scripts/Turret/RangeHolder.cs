using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHolder : MonoBehaviour
{
    bool ShowingRange = false;

    //圆型
    [SerializeField] DetectRange circleDetectRange = default;
    [SerializeField] ForbidRange circleForbidRange = default;
    //半圆型
    [SerializeField] DetectRange halfCircleDetectRange = default;
    [SerializeField] ForbidRange halfCircleForbidRange = default;
    //直线
    [SerializeField] DetectRange lineDetectRange = default;

    TurretContent Turret;
    List<TargetPoint> forbidList = new List<TargetPoint>();

    RangeIndicator rangeIndicatorPrefab;
    List<RangeIndicator> rangeIndicators = new List<RangeIndicator>();
    private void Awake()
    {
        Turret = this.transform.root.GetComponentInChildren<TurretContent>();
        rangeIndicatorPrefab = StaticData.Instance.RangeIndicatorPrefab;
    }

    public void SetRange(int range, int fRange, RangeType rangeType)
    {
        RecycleRanges();
        List<Vector2Int> points = null;
        switch (rangeType)
        {
            case RangeType.Circle:
                circleDetectRange.gameObject.SetActive(true);
                circleForbidRange.gameObject.SetActive(true);
                halfCircleDetectRange.gameObject.SetActive(false);
                halfCircleForbidRange.gameObject.SetActive(false);
                lineDetectRange.gameObject.SetActive(false);

                circleDetectRange.GetComponent<BoxCollider2D>().size = Vector2.one * (2 * range + 1) * Mathf.Cos(45 * Mathf.Deg2Rad);
                circleForbidRange.GetComponent<BoxCollider2D>().size = Vector2.one * 2 * fRange * Mathf.Cos(45 * Mathf.Deg2Rad);
                points = StaticData.GetCirclePoints(range, fRange);
                break;
            case RangeType.HalfCircle:
                circleDetectRange.gameObject.SetActive(false);
                circleForbidRange.gameObject.SetActive(false);
                halfCircleDetectRange.gameObject.SetActive(true);
                halfCircleForbidRange.gameObject.SetActive(true);
                lineDetectRange.gameObject.SetActive(false);

                halfCircleDetectRange.transform.localScale = Vector2.one * (range + 0.5f);
                halfCircleForbidRange.transform.localScale = Vector2.one * fRange;
                points = StaticData.GetHalfCirclePoints(range, fRange);
                break;
            case RangeType.Line:
                circleDetectRange.gameObject.SetActive(false);
                circleForbidRange.gameObject.SetActive(false);
                halfCircleDetectRange.gameObject.SetActive(false);
                halfCircleForbidRange.gameObject.SetActive(false);
                lineDetectRange.gameObject.SetActive(true);
                points = StaticData.GetLinePoints(range, fRange);
                lineDetectRange.GetComponent<BoxCollider2D>().size = new Vector2(1, range - fRange);
                lineDetectRange.GetComponent<BoxCollider2D>().offset = new Vector2(0, fRange + 1 + 0.5f * (range - fRange - 1));

                break;
        }

        for (int i = 0; i < points.Count; i++)
        {
            RangeIndicator rangeIndecator = Instantiate(rangeIndicatorPrefab, transform);
            rangeIndecator.transform.localPosition = (Vector3Int)points[i];
            rangeIndicators.Add(rangeIndecator);
        }
        ShowRange(ShowingRange);
    }

    public void ShowRange(bool show)
    {
        ShowingRange = show;
        var ranges = rangeIndicators.GetEnumerator();
        while (ranges.MoveNext())
        {
            ranges.Current.ShowSprite(show);
        }
    }

    private void RecycleRanges()
    {
        var ranges = rangeIndicators.GetEnumerator();
        while (ranges.MoveNext())
        {
            ranges.Current.ShowSprite(false);
        }
        rangeIndicators.Clear();
    }

    public void AddTarget(TargetPoint target)
    {
        if (forbidList.Contains(target))
            return;
        Turret.AddTarget(target);
    }

    public void RemoveTarget(TargetPoint target)
    {
        Turret.RemoveTarget(target);
    }

    public void ForbidTarget(TargetPoint target)
    {
        forbidList.Add(target);
        Turret.RemoveTarget(target);
    }

    public void UnForbidTarget(TargetPoint target)
    {
        forbidList.Remove(target);
        Turret.AddTarget(target);
    }



}
