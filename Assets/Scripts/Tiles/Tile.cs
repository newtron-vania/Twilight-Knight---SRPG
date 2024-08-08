using System.Numerics;
using UnityEngine;

public class Tile
{
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; } // 높이 또는 깊이를 나타냅니다.
    public float gCost { get; set; }
    public float hCost { get; set; }
    public GameObject tileObject { get; set; }

    public Tile(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.gCost = 0f;
        this.hCost = 0f;
        this.tileObject = null;
    }

    public float fCost
    {
        get { return gCost + hCost; }
    }

    // 동등성 비교를 위한 == 연산자 오버로드
    public static bool operator ==(Tile a, Tile b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    // 동등성 비교를 위한 != 연산자 오버로드
    public static bool operator !=(Tile a, Tile b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return false;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return true;
        return !(a.x == b.x && a.y == b.y && a.z == b.z);
    }

    // Equals 메서드 오버라이드
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        Tile other = (Tile)obj;
        return this == other;
    }

    // GetHashCode 메서드 오버라이드
    public override int GetHashCode()
    {
        return (x, y, z).GetHashCode();
    }
}