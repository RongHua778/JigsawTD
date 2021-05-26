[System.Serializable]
public class Composition
{
    public int levelRequirement = 0;
    public int elementRequirement = 0;
    public bool obtained = false;
    public Composition(int levelRequirement, int elementRequirement)
    {
        this.levelRequirement = levelRequirement;
        this.elementRequirement = elementRequirement;
        obtained = false;
    }
}