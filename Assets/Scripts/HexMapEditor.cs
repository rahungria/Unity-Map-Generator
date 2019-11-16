﻿using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;
	private int activeElevation;

	void Awake () {
		SelectColor(0);
	}

	void Update () {
		if (
			Input.GetKey(KeyCode.Mouse0) &&
			!EventSystem.current.IsPointerOverGameObject()
		) {
			HandleInput();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			EditCell(hexGrid.GetCell(hit.point));
		}
	}

	void EditCell(HexCell cell){
		cell.color = activeColor;
		cell.Elevation = activeElevation;
		print("Active Elevation: " + activeElevation);
		hexGrid.Refresh();
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}
	public void SetElevation (UnityEngine.UI.Slider slider){
		print("elevation: " + slider.value);
		activeElevation = (int)slider.value;
	}
}