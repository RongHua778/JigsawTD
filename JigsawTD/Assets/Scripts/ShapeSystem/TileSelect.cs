using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect : MonoBehaviour
{

    [SerializeField] RenderTexture renderTexture;
    public TileShape Shape { get; set; }
    [SerializeField] LevelDownSelect m_LevelDownSelect = default;



    public void InitializeDisplay(int displayID, TileShape shape)
    {
        Shape = shape;
        shape.SetUIDisplay(displayID, renderTexture);
        StrategyElement strategy = Shape.m_ElementTurret.Strategy as StrategyElement;
        if (strategy.Quality > 1)
        {
            m_LevelDownSelect.SetStrategy((StrategyElement)Shape.m_ElementTurret.Strategy);
            m_LevelDownSelect.gameObject.SetActive(true);
        }
        else
        {
            m_LevelDownSelect.gameObject.SetActive(false);
        }
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
