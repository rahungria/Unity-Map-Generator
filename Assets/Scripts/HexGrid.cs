using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int xWidht = 6, zHeight = 6;
    public HexCell cellPrefab;
    public TMPro.TextMeshProUGUI cellLabelPrefab;

    public Color defaultColor = Color.white, touchedColor = Color.magenta;
    Canvas gridCanvas;
    HexMesh hexMesh;

    HexCell[] cells;

    void Awake(){
        hexMesh = GetComponentInChildren<HexMesh>();
        gridCanvas = GetComponentInChildren<Canvas>();

        cells = new HexCell[zHeight * xWidht];

        for (int z = 0, i = 0; z < zHeight; z++){
            for (int x = 0; x < xWidht; x++){
                CreateCell(x,z,i++);
            }
        }
    }

    void Start(){
        hexMesh.Triangulate(cells);
    }
    
    public void ColorCell(Vector3 position, Color color){
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);

        int index = coordinates.X + coordinates.Z * xWidht + coordinates.Z/2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);

        print("touched at " + coordinates.ToString());
    }

    void CreateCell(int x, int z, int i){
        Vector3 position;
        position.x = (x + z * 0.5f - z/2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoords(x,z);
        cell.color = defaultColor;

        TMPro.TextMeshProUGUI label = Instantiate<TMPro.TextMeshProUGUI>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = 
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        label.transform.position += Vector3.up * .1f;
    }
}
