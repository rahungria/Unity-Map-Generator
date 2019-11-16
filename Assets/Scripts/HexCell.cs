using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;

	public RectTransform uiRect;

	public HexGridChunk chunk;
	public Color Color {
		get {
			return color;
		}
		set {
			if (color == value) {
				return;
			}
			color = value;
			Refresh();
		}
	}

	public int Elevation {
		get {
			return elevation;
		}
		set {
			if (elevation == value) {
				return;
			}
			elevation = value;
			Vector3 position = transform.localPosition;
			position.y = value * HexMetrics.elevationStep;
			position.y +=
				(HexMetrics.SampleNoise(position).y * 2f - 1f) *
				HexMetrics.elevationPerturbStrength;
			transform.localPosition = position;

			Vector3 uiPosition = uiRect.localPosition;
			uiPosition.z = -position.y;
			uiRect.localPosition = uiPosition;

			if (hasOutgoingRiver && elevation < GetNeighbor(outgoingRiver).elevation){
				RemoveOutgoingRiver();
			}
			if (hasIncomingRiver && elevation > GetNeighbor(incomingRiver).elevation){
				RemoveIncomingRiver();
			}

			Refresh();
		}
	}

	public Vector3 Position {
		get {
			return transform.localPosition;
		}
	}

	public float StreamBedY{
		get {
			return 
				(elevation + HexMetrics.streamBedElevationOffset) *
				HexMetrics.elevationStep;
		}
	}

    public bool HasIncomingRiver {
        get {
            return hasIncomingRiver;
        }
    }

    public bool HasOutgoingRiver {
        get {
            return hasOutgoingRiver;
        }
    }

    public HexDirection IncomingRiver {
        get {
            return incomingRiver;
        }
    }

    public HexDirection OutgoingRiver {
        get {
            return outgoingRiver;
        }
    }

	public bool HasRiver {
		get {
			return hasIncomingRiver || hasOutgoingRiver;
		}
	}

	public bool HasRiverBeginOrEnd {
		get {
			return hasIncomingRiver != hasOutgoingRiver;
		}
	}

	public bool HasRiverThroughEdge(HexDirection direction){
		return 
			HasIncomingRiver && incomingRiver == direction ||
			hasOutgoingRiver && outgoingRiver == direction;
	}

	Color color;

	int elevation = int.MinValue;
    bool hasIncomingRiver, hasOutgoingRiver;
    HexDirection incomingRiver, outgoingRiver;

	[SerializeField]
	HexCell[] neighbors;

	void Awake()
	{
		neighbors = new HexCell[6];
	}

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	public HexEdgeType GetEdgeType (HexDirection direction) {
		return HexMetrics.GetEdgeType(
			elevation, neighbors[(int)direction].elevation
		);
	}

	public HexEdgeType GetEdgeType (HexCell otherCell) {
		return HexMetrics.GetEdgeType(
			elevation, otherCell.elevation
		);
	}

	void Refresh () {
		if (chunk) {
			chunk.Refresh();
			for (int i = 0; i < neighbors.Length; i++) {
				HexCell neighbor = neighbors[i];
				if (neighbor != null && neighbor.chunk != chunk) {
					neighbor.chunk.Refresh();
				}
			}
		}
	}

	public void RemoveOutgoingRiver (){
		if (hasOutgoingRiver){
			hasOutgoingRiver = false;
			Refresh();

			HexCell neighbour = GetNeighbor(outgoingRiver);
			neighbour.hasIncomingRiver = false;
			neighbour.RefreshSelfOnly();
		}
		else {

		}
	}

	public void RemoveIncomingRiver (){
		if (hasIncomingRiver){
			hasIncomingRiver = false;
			Refresh();

			HexCell neighbour = GetNeighbor(incomingRiver);
			neighbour.hasOutgoingRiver = false;
			neighbour.RefreshSelfOnly();
		}
		else {

		}
	}

	public void RemoveRiver(){
		RemoveIncomingRiver();
		RemoveOutgoingRiver();
	}

	public void SetOutgoingRiver (HexDirection direction){
		if (!hasOutgoingRiver || outgoingRiver != direction){
			HexCell neighbour = GetNeighbor(direction);
			if (neighbour && neighbour.Elevation <= elevation){
				RemoveOutgoingRiver();
				if (hasIncomingRiver && incomingRiver == direction){
					RemoveIncomingRiver();
				}
				hasOutgoingRiver = true;
				outgoingRiver = direction;
				RefreshSelfOnly();

				neighbour.RemoveIncomingRiver();
				neighbour.hasIncomingRiver = true;
				neighbour.incomingRiver = direction.Opposite();
				neighbour.RefreshSelfOnly();
			}
		}
	}

	void RefreshSelfOnly(){
		chunk.Refresh();
	}
}