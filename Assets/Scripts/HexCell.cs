﻿using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;

	public Color color;
        public RectTransform uiRect;
    public int Elevation {
        get {
            return elevation;
        }
        set {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y += (HexMetrics.SampleNoise(position).y * 2f -1f) * HexMetrics.elevationPerturbStrenght;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z -= position.y;
            uiRect.localPosition = uiPosition;
        }
    }

    public Vector3 Position{
        get {
            return transform.localPosition;
        }
    }
    int elevation;

	[SerializeField]
	HexCell[] neighbors;

    void Awake(){
        neighbors = new HexCell[6];
    }

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

    public HexEdgeType GetEdgeType(HexDirection direction){
        return HexMetrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
    }
    public HexEdgeType GetEdgeType(HexCell other){
        return HexMetrics.GetEdgeType(elevation, other.elevation);
    }
}