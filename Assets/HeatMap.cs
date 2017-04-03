using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeatMap : MonoBehaviour {

	private float step = 0.6f;

	public Vector2 offset;

	public int[,] cellDensities;

	public int squareLength = 100;

	private Transform pixelContainer;
	public GameObject pixelPrefab;

	private List<GameObject> pixels = new List<GameObject>();

	// Use this for initialization
	public void Initialize (Transform activeMarkers) {
		print("heat map init: ");

		foreach(HeatMap hm in GameObject.FindObjectsOfType<HeatMap>()) {

			hm.pixelContainer = new GameObject("Pixels").transform;
			hm.pixelContainer.transform.SetParent(hm.transform);
			hm.pixelContainer.transform.position = hm.offset;

			//Get cell densities
			//2d array of real number marker hits
			hm.cellDensities = new int[hm.squareLength, hm.squareLength];
			foreach(Transform marker in activeMarkers) {

				//Determine containing cell
				//Convert Vector2 of 4.2 >  R > -4.2 into row and column for n-lengthed table [0, n - 1] [0, n - 1]
//				print("input position: " + marker.position.ToString());
				int xCell = Mathf.FloorToInt(((marker.position.x - -4.6f) / 9.2f) * 15);
				int yCell = Mathf.FloorToInt(((marker.position.y - -4.6f) / 9.2f) * 15);
				//				int yCell = (int)Mathf.Floor(marker.transform.position.y) * hm.squareLength;
				print("marker x, marker y, x cell, y cell: " + marker.position.x + ", " + marker.position.y + ", " + xCell + ", " + yCell);
				try {
				hm.cellDensities[xCell, yCell] = hm.cellDensities[xCell, yCell] + 1;
				} catch(Exception e) {
					Debug.LogWarning("Marker fell outside of grid bounds.");
				}
			}
			print("Center sample density: " + hm.cellDensities[7, 7]);

			//Create pixels
			for(int i = 0; i < hm.squareLength; i++) {
				for(int j = 0; j < hm.squareLength; j++) {
					GameObject pixel = Instantiate(hm.pixelPrefab) as GameObject;
					SpriteRenderer sr = pixel.GetComponent<SpriteRenderer>();


					float rawHits = hm.cellDensities[i, j];
//					print("hits: " + rawHits);
					float heat = 1 - (hm.cellDensities[i, j] / 100f);
//					print("sample heat: " + heat);
					sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, heat);
					hm.pixels.Add(pixel);

					pixel.transform.position = new Vector2(i * hm.step, j * hm.step) + hm.offset;
					pixel.transform.SetParent(hm.pixelContainer);
				}
			}
		}



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
