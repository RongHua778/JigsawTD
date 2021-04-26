using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ShapeType
{
    T,
    L,
    I,
    O
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



    public void InitializeRandomShpe(TileFactory tileFactory)
    {
        for (int i = 0; i < tilePos.Length; i++)
        {
            GameTile tile = tileFactory.GetRandomTile();
            tile.transform.position = tilePos[i].position;
            tile.transform.rotation = DirectionExtensions.GetRotation(Random.Range(0, 4));
            tile.transform.SetParent(this.transform);
            tile.SetPreviewing(true);
            tiles.Add(tile);
        }

    }

    public int GetSlotCount()
    {
        return tilePos.Length;
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
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), -0.1f);
        this.GetComponent<DraggingShape>().OnDraggingInUpdate();
        GameManager.holdingShape = this.GetComponent<DraggingShape>();
    }



}
