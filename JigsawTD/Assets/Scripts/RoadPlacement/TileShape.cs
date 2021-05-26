using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ShapeType
{
    J,L,T,O,I,D
}
public class TileShape : MonoBehaviour
{
    TileSlot[] tilePos;
    Camera renderCam;
    GameObject bgObj;
    int levelTileCount = 0;
    DraggingShape draggingShape;

    [HideInInspector]
    public List<GameTile> tiles = new List<GameTile>();

    public ShapeType shapeType = default;

    private void Awake()
    {
        tilePos = transform.GetComponentsInChildren<TileSlot>();
        renderCam = transform.Find("RenderCam").GetComponent<Camera>();
        bgObj = transform.Find("BG").gameObject;
        draggingShape = this.GetComponent<DraggingShape>();
    }
    //在shape上面加上塔
    public void InitializeShape()
    {
        //判断shape上面生成几个塔
        //levelTileCount = Random.value > 0.9f ? 2 : 1;
        levelTileCount = 1;
        TileFactory tileFactory = GameManager.Instance._tileFactory;
        if (shapeType == ShapeType.D)
        {
            GameTile tile;
            tile = tileFactory.GetTurretTile();
            tile.transform.position = tilePos[0].transform.position;
            tile.transform.SetParent(this.transform);
            tile.m_DraggingShape = this.GetComponent<DraggingShape>();
            tiles.Add(tile);
            draggingShape.Initialized();
            SetPreviewPlace();
        }
        else
        {
            List<int> levelPos = StaticData.GetRandomSequence(tilePos.Length, levelTileCount).ToList();
            for (int i = 0; i < tilePos.Length; i++)
            {
                GameTile tile;
                if (levelPos.Contains(i))
                {
                    tile = tileFactory.GetTurretTile();
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
        //switch (shapeType)
        //{
        //    case ShapeType.T:
        //        levelTileCount = Random.value > 0.3f ? 2 : 1;
        //        break;
        //    case ShapeType.L:
        //    case ShapeType.I:
        //        levelTileCount = Random.value > 0.9f ? 2 : 1;
        //        break;
        //    case ShapeType.O:
        //        levelTileCount = Random.value > 0.5f ? 1 : 0;
        //        break;
        //}

    }
    public int GetSlotCount()
    {
        return tilePos.Length;
    }

    public void ReclaimTiles()
    {
        foreach(GameTile tile in tiles)
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
