using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameBoard _board = default;

    [SerializeField]
    Vector2Int _startSize = default;

    [SerializeField]
    GameTileContentFactory _contentFactory=default;

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    // Start is called before the first frame update
    void Start()
    {
        _board.Initialize(_startSize, _contentFactory);
        _board.ShowTempTile = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameTile tile = _board.GetTile();
            if (tile != null)
            {
                _board.ToggleTurret(tile);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameTile tile = _board.GetTile();
            if (tile != null)
            {
                _board.ToggleRock(tile);
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            _board.ShowPaths = !_board.ShowPaths;
            _board.ShowTempTile = !_board.ShowTempTile;
        }
    }
}
