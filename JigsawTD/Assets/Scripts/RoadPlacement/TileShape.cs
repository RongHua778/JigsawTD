using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum ShapeType
{
    J, L, T, O, I, D
}
public class TileShape : MonoBehaviour
{
    TileSlot[] tilePos;
    Camera renderCam;
    GameObject bgObj;
    DraggingShape draggingShape;
    Text turretName;

    [HideInInspector]
    public List<GameTile> tiles = new List<GameTile>();

    public ShapeType shapeType = default;

    private void Awake()
    {
        turretName = GetComponentInChildren<Text>();
        tilePos = transform.GetComponentsInChildren<TileSlot>();
        renderCam = transform.Find("RenderCam").GetComponent<Camera>();
        bgObj = transform.Find("BG").gameObject;
        draggingShape = this.GetComponent<DraggingShape>();
    }
    //在shape上面加上塔
    public void InitializeShape(GameTile specialTile, TileFactory tileFactory = null)
    {
        if (shapeType == ShapeType.D)
        {
            specialTile.transform.position = tilePos[0].transform.position;
            specialTile.transform.SetParent(this.transform);
            specialTile.m_DraggingShape = this.GetComponent<DraggingShape>();
            tiles.Add(specialTile);
            draggingShape.Initialized();
            SetPreviewPlace();
        }
        else
        {
            int g = Random.Range(0, tilePos.Length);
            for (int i = 0; i < tilePos.Length; i++)
            {
                GameTile tile;
                if (i == g)
                {
                    tile = specialTile;
                    Turret turret = ((TurretTile)specialTile).turret;
                    turretName.text = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].TurretName;
                }
                else
                {
                    tile = tileFactory.GetBasicTile();
                }
                tile.transform.position = tilePos[i].transform.position;
                tile.tileType.rotation = DirectionExtensions.GetRandomRotation();
                tile.transform.SetParent(this.transform);
                tile.m_DraggingShape = this.GetComponent<DraggingShape>();
                tiles.Add(tile);
            }
            draggingShape.Initialized();
        }

    }
    public int GetSlotCount()
    {
        return tilePos.Length;
    }

    public void ReclaimTiles()
    {
        foreach (GameTile tile in tiles)
        {
            ObjectPool.Instance.UnSpawn(tile.gameObject);
        }
    }


    public void SetUIDisplay(int displayID, RenderTexture texture)
    {
        transform.position = new Vector3(1000f + 10f * displayID, 0, -12f);
        renderCam.targetTexture = texture;
        renderCam.gameObject.SetActive(true);
        bgObj.SetActive(true);
    }

    public void SetPreviewPlace()
    {
        bgObj.SetActive(false);
        turretName.transform.parent.gameObject.SetActive(false);
        Vector2 pos = Camera.main.transform.position;
        renderCam.gameObject.SetActive(false);
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), -1f);
        draggingShape.OnDraggingInUpdate();
        StaticData.holdingShape = draggingShape;
        foreach (GameTile tile in tiles)
        {
            tile.SetPreviewing(true);
        }
    }



}
