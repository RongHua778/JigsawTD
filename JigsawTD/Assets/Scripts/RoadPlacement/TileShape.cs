using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ShapeType
{
    J,L,T,O,I
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
    public void InitializeRandomShpe(TileFactory tileFactory)
    {
        levelTileCount = Random.value > 0.9f ? 2 : 1;
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
        List<int> levelPos = StaticData.GetRandomSequence(tilePos.Length, levelTileCount).ToList();
        for (int i = 0; i < tilePos.Length; i++)
        {
            GameTile tile;
            if (levelPos.Contains(i))
            {
                tile = tileFactory.GetRandomTile();
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
        bgObj.SetActive(true);
    }

    public void SetPreviewPlace()
    {
        bgObj.SetActive(false);
        Vector2 pos = Camera.main.transform.position;
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), -1f);
        draggingShape.OnDraggingInUpdate();
        StaticData.holdingShape = draggingShape;
        foreach (GameTile tile in tiles)
        {
            tile.SetPreviewing(true);
        }
    }



}
