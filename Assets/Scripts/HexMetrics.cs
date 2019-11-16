using UnityEngine;

public static class HexMetrics {
	public const float outerRadius = 10f;
	public const float innerRadius = outerRadius * 0.866025404f;
	public const float solidFactor = 0.8f;
	public const float blendFactor = 1f - solidFactor;
    public const float elevationStep = 3f;
    public const int terracesPerSlope = 2;
    public const int terraceSteps = terracesPerSlope * 2 + 1;
    public const float horizontalTerraceStepSize = 1f/terraceSteps;
    public const float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);
    public const float perturbStrenght = 4f;
    public const float elevationPerturbStrenght = 1.5f;
    public const float noiseScale = .003f;
    public const int chunkSizeX = 5, chunkSizeZ = 5;

    public static Texture2D noiseSource;

	static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(0f, 0f, outerRadius)
	};

    //Getting Corners of neighbours
	public static Vector3 GetFirstCorner (HexDirection direction) {
		return corners[(int)direction];
	}

	public static Vector3 GetSecondCorner (HexDirection direction) {
		return corners[(int)direction + 1];
	}

    //Getting corner vertex minus quad offset
	public static Vector3 GetFirstSolidCorner (HexDirection direction) {
		return corners[(int)direction] * solidFactor;
	}

	public static Vector3 GetSecondSolidCorner (HexDirection direction) {
		return corners[(int)direction + 1] * solidFactor;
	}


	public static Vector3 GetBridge (HexDirection direction) {
		return (corners[(int)direction] + corners[(int)direction + 1]) *
			blendFactor;
	}

    //Logic for getting the Terraces Slopes
    public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step){
        float h = step * HexMetrics.horizontalTerraceStepSize;
        a.x += (b.x - a.x) * h;
        a.z += (b.z - a.z) * h;

        float v = ((step + 1) / 2) * HexMetrics.verticalTerraceStepSize;
        a.y += (b.y - a.y) * v;

        return a;
    }

    public static Color TerraceLerp(Color a, Color b, int step){
        float h = step * HexMetrics.horizontalTerraceStepSize;
        return Color.Lerp(a,b,h);
    }

    //Return the edge connection type depending on both elevations
    public static HexEdgeType GetEdgeType (int elevation1, int elevation2){
        if (elevation1 == elevation2){
            return HexEdgeType.FLAT;
        }

        int delta = Mathf.Abs(elevation2 - elevation1);

        if (delta == 1){
            return HexEdgeType.SLOPE;
        }
        return HexEdgeType.CLIFF;
    }

    //Noise helper functions
    public static Vector4 SampleNoise(Vector3 position){
        return noiseSource.GetPixelBilinear(position.x * noiseScale, position.z*noiseScale);
    }
}