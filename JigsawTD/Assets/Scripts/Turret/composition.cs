public class Composition
{
    public int levelRequirement;
    public int elementRequirement;
    public bool obtained;
    public Composition(int levelRequirement,int elementRequirement)
    {
        this.levelRequirement = levelRequirement;
        this.elementRequirement = elementRequirement;
        obtained = false;
    }
}