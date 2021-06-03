using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/ShapeFactory", fileName = "ShapeFactory")]
public class TileShapeFactory : ScriptableObject
{
    [SerializeField] TileShape[] ShapePrefabs = default;
    private Dictionary<ShapeType, TileShape> ShapeDIC;

    [SerializeField] float[] RandomShapeChance = new float[7];


    public void Initialize()
    {
        ShapeDIC = new Dictionary<ShapeType, TileShape>();
        foreach (TileShape shape in ShapePrefabs)
        {
            ShapeDIC.Add(shape.shapeType, shape);
        }
    }

    public TileShape GetRandomShape()
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
        if (ShapeDIC.ContainsKey(type))
        {
            TileShape returnShape = Instantiate(ShapeDIC[type]);
            return returnShape;
        }
        Debug.Assert(false, "Î´Ö¸¶¨µÄShapetype");
        return null;
    }


}
