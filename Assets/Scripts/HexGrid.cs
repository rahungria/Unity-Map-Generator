using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HexGrid : MonoBehaviour {
    int cellCountX, cellCountZ;

    public int chunkCountX = 4, chunkCountZ = 3;

	public Color defaultColor = Color.white;

	public HexCell cellPrefab;
	public TextMeshProUGUI cellLabelPrefab;
    public Texture2D noiseSource;

	HexCell[] cells;

	Canvas gridCanvas;
	HexMesh hexMesh;

	void Awake () {
        HexMetrics.noiseSource = noiseSource;
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

        CreateCells();
	}

	void Start () {
		hexMesh.Triangulate(cells);
	}

    public HexCell GetCell (Vector3 position){
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountZ + coordinates.Z/2;
        return cells[index];
    }

    void CreateCells(){

        cellCountX = chunkCountX * HexMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;


		cells = new HexCell[cellCountX * cellCountZ];

		for (int z = 0, i = 0; z < cellCountZ; z++) {
			for (int x = 0; x < cellCountX; x++) {
				CreateCell(x, z, i++);
			}
		}
    }

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountZ]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountZ - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountZ]);
				if (x < cellCountZ - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountZ + 1]);
				}
			}
		}

		TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
        label.transform.position += Vector3.up * .1f;
		label.text = cell.coordinates.ToStringOnSeparateLines();

        cell.uiRect = label.rectTransform;
        cell.Elevation = 0;
	}

    public void Refresh(){
        hexMesh.Triangulate(cells);
    }
}