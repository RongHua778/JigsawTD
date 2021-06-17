[System.Serializable]
public class Composition
{
    public int qualityRequeirement = 0;
    public int elementRequirement = 0;
    public bool obtained = false;
    public GameTile turretTile;
    public Composition(int levelRequirement, int elementRequirement)
    {
        this.qualityRequeirement = levelRequirement;
        this.elementRequirement = elementRequirement;
        obtained = false;
    }
}