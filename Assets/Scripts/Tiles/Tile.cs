using System.Numerics;

public struct Tile
{
    public int x, y;

    public int height;

    public float Gcost, Hcost;
    
    
    public Tile(int x, int y, int height)
    {
        this.x = x;
        this.y = y;
        this.height = height;
        
        Gcost = 0f;
        Hcost = 0f;
    }


    public float Fcost
    {
        get { return Gcost + Hcost; }
    }
    
    public static bool operator ==(Tile a, Tile b)
    {
        if (a.x == b.x && a.y == b.y)
        {
            return true;
        }

        return false;
    }
    

    public static bool operator !=(Tile a, Tile b)
    {
        if (a.x != b.x || a.y != b.y)
        {
            return true;
        }

        return false;
    }

}
