using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameTile : TileBase
{

    public DraggingShape m_DraggingShape;

    public int TileID;
    public abstract int TileLevel { get; }

    bool showingPath = false;
    float pathSpeed = 0.5f;
    LineRenderer lineSR;
    SpriteRenderer tileTypeSr;

    Direction pathDirection;
    public Direction PathDirection { get => pathDirection; set => pathDirection = value; }

    GameTile nextOnPath;
    public GameTile NextTileOnPath { get => nextOnPath; set => nextOnPath = value; }
    public Vector3 ExitPoint { get; set; }

    GameTileContent content;
    public GameTileContent Content
    {
        get => content;
        set
        {
            Debug.Assert(value != null, "Null Content Assigned");
            if (content != null)
                content.Recycle();
            content = value;
            content.transform.localPosition = transform.localPosition;
        }
    }

    private void Awake()
    {
        lineSR = this.GetComponent<LineRenderer>();
        tileTypeSr = transform.Find("TileType").GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (showingPath)
        {
            lineSR.material.SetTextureOffset("_MainTex", new Vector2(-Time.time * pathSpeed, 0));
        }
    }

    public void SetPreviewing(bool isPreviewing)
    {
        //if (isPreviewing)
        //{
        //    tileTypeSr.sortingOrder = 5;
        //    GetComponent<SpriteRenderer>().sortingOrder = 4;
        //    //GetComponent<Collider2D>().enabled = false;

        //}
        //else
        //{
        //    tileTypeSr.sortingOrder = 2;
        //    GetComponent<SpriteRenderer>().sortingOrder = 1;
        //    //GetComponent<Collider2D>().enabled = true;
        //}
    }

    public virtual void OnTilePass()
    {

    }

    public void ShowPath()
    {
        if (NextTileOnPath == null)
            return;
        showingPath = true;
        lineSR.enabled = true;
        Vector3[] pathPoss = new Vector3[2];
        pathPoss[0] = transform.position + new Vector3(0, 0, -0.1f);
        pathPoss[1] = nextOnPath.transform.position + new Vector3(0, 0, -0.1f);
        lineSR.positionCount = 2;
        lineSR.SetPositions(pathPoss);
    }

    public void HidePath()
    {
        lineSR.enabled = false;
        showingPath = false;
    }

    private void OnMouseDown()
    {
        if (m_DraggingShape != null)
        {
            m_DraggingShape.StartDragging();
        }
    }

    private void OnMouseUp()
    {
        if (m_DraggingShape != null)
        {
            m_DraggingShape.EndDragging();
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        m_DraggingShape = null;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.name);
    //    if (collision.CompareTag("Tile"))
    //    {
    //        collision.gameObject.layer = 0;
    //    }
    //}



}
