using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileSelect : MonoBehaviour
{

    [SerializeField] RenderTexture renderTexture;
    public TileShape Shape { get; set; }
    [SerializeField] LevelDownSelect m_LevelDownSelect = default;
    [SerializeField] ElementSelectPreview m_ElementPreview = default;
    [SerializeField] ElementInfoBtn m_InfoBtn = default;


    public void InitializeDisplay(int displayID, TileShape shape)
    {
        Shape = shape;
        shape.SetUIDisplay(displayID, renderTexture);
        StrategyBase strategy = Shape.m_ElementTurret.Strategy;
        if (strategy.Quality > 1)
        {
            m_LevelDownSelect.SetStrategy(strategy);
            m_LevelDownSelect.gameObject.SetActive(true);
        }
        else
        {
            m_LevelDownSelect.gameObject.SetActive(false);
        }
        m_InfoBtn.SetStrategy(strategy);
        m_ElementPreview.SetStrategy(strategy);
    }

    public void OnShapeClick(bool levelDown = false)
    {
        if (levelDown)//是否为降级选择
        {
            Shape.m_ElementTurret.Strategy.Quality -= 1;
            Shape.m_ElementTurret.Strategy.SetQualityValue();
            Shape.m_ElementTurret.SetGraphic();
        }
        Shape.SetPreviewPlace();
        Shape = null;
        GameManager.Instance.PreviewComposition(false);
        GameManager.Instance.SelectShape();
        GameManager.Instance.HideTips();
        GameEvents.Instance.TutorialTrigger(TutorialType.SelectShape);

    }

    public void ClearShape()
    {
        if (Shape == null)
            return;
        Shape.ReclaimTiles();
        Destroy(Shape.gameObject);
        Shape = null;
    }


}
