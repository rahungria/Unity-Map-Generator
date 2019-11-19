using UnityEngine;

public class NewMapMenu : MonoBehaviour {

	public HexGrid hexGrid;

	public void Open () {
		gameObject.SetActive(true);
		HexMapCamera.Locked = true;
	}

	public void Close () {
		gameObject.SetActive(false);
		HexMapCamera.Locked = false;
	}

	public void CreateSmallMap () {
		CreateMap(30, 20);
	}

	public void CreateMediumMap () {
		CreateMap(50, 40);
	}

	public void CreateLargeMap () {
		CreateMap(100, 70);
	}

	void CreateMap (int x, int z) {
		hexGrid.CreateMap(x, z);
		HexMapCamera.ValidatePosition();
		Close();
	}
}