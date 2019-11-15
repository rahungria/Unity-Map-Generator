using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;
    public HexGrid hexGrid;
    private Color activeColor;

    void Awake(){
        SelectColor(0);
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            HandleInput();
        }
    }

    void HandleInput(){
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out RaycastHit hit) &&
        !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            hexGrid.ColorCell(hit.point, activeColor);
        }
    }

    public void SelectColor(int index){
        activeColor = colors[index];
    }
}
