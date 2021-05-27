    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/ShapeFactory", fileName = "ShapeFactory")]
public class TileShapeFactory : ScriptableObject
{
    [SerializeField] TileShape TShapePrefab = default;
    [SerializeField] TileShape LShapePrefab = default;
    [SerializeField] TileShape IShapePrefab = default;
    [SerializeField] TileShape OShapePrefab = default;
    [SerializeField] TileShape JShapePrefab = default;
    [SerializeField] TileShape DShapePrefab = default;

    [SerializeField] float[] RandomShapeChance = new float[5];


    public TileShape GetBasicShape()
    {
        int shapeID = StaticData.RandomNumber(RandomShapeChance);
        return GetShape((ShapeType)shapeID);
    }
    public TileShape GetDShape()
    {
        return GetShape(ShapeType.D);
    }
    private TileShape GetShape(ShapeType type)
    {
        switch (type)
        {
            case ShapeType.T:
                return Get(TShapePrefab);
            case ShapeType.L:
                return Get(LShapePrefab);
            case ShapeType.I:
                return Get(IShapePrefab);
            case ShapeType.O:
                return Get(OShapePrefab);
            case ShapeType.J:
                return Get(JShapePrefab);
            case ShapeType.D:
                return Get(DShapePrefab);
        }
        Debug.Assert(false, "Î´Ö¸¶¨µÄShapetype");
        return null;
    }

    private TileShape Get(TileShape prefab)
    {
        return Instantiate(prefab);
    }

}
