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
    TileSlot[] tilePos;
    Camera renderCam;
    GameObject bgObj;
    DraggingShape draggingShape;
    Text turretNameTxt;

    [HideInInspector]
    public List<GameTile> tiles = new List<GameTile>();

    public ShapeType shapeType = default;

    private void Awake()
    {
        turretNameTxt = transform.GetComponentInChildren<Text>();
        tilePos = transform.GetComponentsInChildren<TileSlot>();
        renderCam = transform.Find("RenderCam").GetComponent<Camera>();
        bgObj = transform.Find("BG").gameObject;
        draggingShape = this.GetComponent<DraggingShape>();
    }
    //在shape上面加上塔
    public void SetTile(GameTile specialTile)
    {
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
            int g = Random.Range(0, tilePos.Length);
            for (int i = 0; i < tilePos.Length; i++)
            {
                GameTile tile;
                if (i == g)
                {
                    tile = specialTile;
                    ElementTurret turret = tile.Content as ElementTurret;
                    turretNameTxt.text = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].TurretName;
                }
                else
                {
                    tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
                }
                tile.transform.position = tilePos[i].transform.position;
                tile.transform.rotation = DirectionExtensions.GetRandomRotation();
                tile.CorrectRotation();
                tile.GetTileDirection();
                tile.transform.SetParent(this.transform);
                tile.m_DraggingShape = draggingShape;
                tiles.Add(tile);
            }
            draggingShape.Initialized(this);
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



}
