using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum ShapeType
{
    J, L, T, O, I, Z, S, D
}
public class TileShape : MonoBehaviour
{
    public bool IsPreviewing = false;
    List<TileSlot> tilePos = new List<TileSlot>();
    List<TileSlot> turretPos = new List<TileSlot>();
    Camera renderCam;
    GameObject bgObj;
    DraggingShape draggingShape;
    Text turretNameTxt;
    public ElementTurret m_ElementTurret;

    [HideInInspector]
    public ShapeSelectUI m_ShapeSelectUI;
    public List<GameTile> tiles = new List<GameTile>();
    public ShapeType shapeType = default;

    //教程强制落位
    private bool needForcePlace;
    private Vector2 forceDir;
    private Vector2 forcePos;

    private void Awake()
    {
        turretNameTxt = transform.GetComponentInChildren<Text>();
        tilePos = transform.GetComponentsInChildren<TileSlot>().ToList();
        foreach (var slot in tilePos)
        {
            if (slot.IsTurretPos)
            {
                turretPos.Add(slot);
            }
        }
        renderCam = transform.Find("RenderCam").GetComponent<Camera>();
        bgObj = transform.Find("BG").gameObject;
        draggingShape = this.GetComponent<DraggingShape>();
    }


    public void SetForcePlace(Vector2 dir, Vector2 pos)//设置强制落位
    {
        needForcePlace = true;
        forceDir = dir;
        forcePos = pos;
    }



    //在shape上面加上塔
    public void SetTile(GameTile specialTile, int posID = -1)
    {
        if (specialTile.Content.ContentType == GameTileContentType.ElementTurret)
        {
            m_ElementTurret = specialTile.Content as ElementTurret;//预览配方功能
        }
        if (shapeType == ShapeType.D)
        {
            specialTile.transform.position = tilePos[0].transform.position;
            specialTile.transform.SetParent(this.transform);
            specialTile.m_DraggingShape = draggingShape;
            tiles.Add(specialTile);
            draggingShape.Initialized(this);
            SetPreviewPlace();
        }
        else
        {
            GameTile tile;
            TileSlot turretSlot = tilePos[posID == -1 ? Random.Range(0, tilePos.Count) : posID];
            tile = specialTile;
            ElementTurret turret = tile.Content as ElementTurret;
            turretNameTxt.text = turret.Strategy.m_Att.TurretLevels[turret.Strategy.Quality - 1].TurretName.Substring(0, 2);
            SetTilePos(tile, turretSlot.transform.position);
            tilePos.Remove(turretSlot);

            for (int i = 0; i < tilePos.Count; i++)
            {
                tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
                SetTilePos(tile, tilePos[i].transform.position);
            }
            draggingShape.Initialized(this);
        }

    }

    private void SetTilePos(GameTile tile, Vector3 pos)
    {
        tile.transform.position = pos;
        tile.SetRandomRotation();
        tile.transform.SetParent(this.transform);
        tile.m_DraggingShape = draggingShape;
        tiles.Add(tile);
    }

    public void ReclaimTiles()
    {
        foreach (GameTile tile in tiles)
        {
            ObjectPool.Instance.UnSpawn(tile);
        }
    }


    public void SetUIDisplay(int displayID, RenderTexture texture)
    {
        IsPreviewing = false;
        transform.position = new Vector3(1000f + 10f * displayID, 0, -12f);
        renderCam.targetTexture = texture;
        renderCam.gameObject.SetActive(true);
        bgObj.SetActive(true);
    }

    public void SetPreviewPlace()
    {
        IsPreviewing = true;

        bgObj.SetActive(false);
        turretNameTxt.transform.parent.gameObject.SetActive(false);
        Vector2 pos = Camera.main.transform.position;
        renderCam.gameObject.SetActive(false);
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), -1f);
        draggingShape.ShapeSpawned();
        foreach (GameTile tile in tiles)
        {
            tile.Previewing = true;
        }
    }

    public bool CheckForcePlacement()
    {
        if (!needForcePlace)
            return true;
        if (Vector2.SqrMagnitude((Vector2)transform.position - forcePos) > 0.1f
            || Vector2.Dot(transform.up, forceDir) < 0.99f)
            return false;
        else
        {
            m_ShapeSelectUI.ClearTutorialPrefabs();
            return true;
        }
    }


}
