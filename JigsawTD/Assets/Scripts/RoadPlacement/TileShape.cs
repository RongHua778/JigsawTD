using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum ShapeType
{
    T,L,I,O
}

public struct TileInfo
{
    public Quaternion rotation;
    public int tileID;
    public TileInfo(Quaternion rot, int id)
    {
        rotation = rot;
        tileID = id;
    }
}
public class TileShape : MonoBehaviour
{
    public ShapeType shapeType;
    [SerializeField] Transform[] tilePos = default;
    [SerializeField] Camera renderCam = default;
    [SerializeField] GameObject bgObj = default;
    public List<GameTile> tiles = new List<GameTile>();


    int levelTileCount = 0;

    public void InitializeRandomShpe(TileFactory tileFactory)
    {
        switch (shapeType)
        {
            case ShapeType.T:
                levelTileCount = Random.value > 0.3f ? 2 : 1;
                break;
            case ShapeType.L:
            case ShapeType.I:
                levelTileCount = Random.value > 0.9f ? 2 : 1;
                break;
            case ShapeType.O:
                levelTileCount = Random.value > 0.5f ? 1 : 0;
                break;
        }
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
                tile = tileFactory.GetTile(0);
            }
            tile.transform.position = tilePos[i].position; 
            tile.transform.rotation = DirectionExtensions.GetRandomRotation();
            tile.transform.SetParent(this.transform);
            tile.SetPreviewing(true);
            tile.m_DraggingShape = this.GetComponent<DraggingShape>();
            tiles.Add(tile);
        }

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
        this.GetComponent<DraggingShape>().OnDraggingInUpdate();
        GameManager.holdingShape = this.GetComponent<DraggingShape>();
    }



}
