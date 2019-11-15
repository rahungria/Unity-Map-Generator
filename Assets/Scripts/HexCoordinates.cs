using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;
    public int X {get => x;}
    public int Y { get => -X-Z;}
    public int Z {get => z;}

    public HexCoordinates (int x, int z){
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoords(int x, int z){
        return new HexCoordinates(x - z/2, z);
    }
    public static HexCoordinates FromPosition (Vector3 position){
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x-y);

        if (iX + iY + iZ != 0){
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(x - iY);
            float dZ = Mathf.Abs(-x -y - iZ);

            if (dX > dY && dX > dZ){
                iX = -iY - iZ;
            }
            else if (dZ > dY){
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX,iZ);
    }

    public override string ToString(){
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }
    public string ToStringOnSeparateLines(){
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}
