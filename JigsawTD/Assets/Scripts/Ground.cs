using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ground : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer _spriteRenderer = default;

    public void SetSize(Vector2Int size)
    {
        _spriteRenderer.size = size;
    }

    public void Extend(Direction direction, int distance)
    {
        switch (direction)
        {
            case Direction.up:
                _spriteRenderer.size += new Vector2Int(0, distance);
                transform.localPosition += new Vector3Int(0, distance / 2, 0);
                break;
            case Direction.down:
                _spriteRenderer.size += new Vector2Int(0, distance);
                transform.localPosition += new Vector3Int(0, -distance / 2, 0);
                break;
            case Direction.left:
                _spriteRenderer.size += new Vector2Int(distance, 0);
                transform.localPosition += new Vector3Int(-distance / 2, 0, 0);
                break;
            case Direction.right:
                _spriteRenderer.size += new Vector2Int(distance, 0);
                transform.localPosition += new Vector3Int(distance / 2, 0, 0);
                break;
        }
    }
}
