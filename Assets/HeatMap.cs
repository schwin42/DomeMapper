using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeatMap : MonoBehaviour {

	public float spacing = 0.1f;

	public int[,] cellDensities;

	public int lengthInSquares = 100;

	private Transform pixelContainer;
	public GameObject pixelPrefab;

	public Vector2 offset = new Vector2(-4.3f, -4.3f);

	public float domeRadius = 4.5f;

	public float heatConstant = 100;

	private List<GameObject> pixels = new List<GameObject>();

	// Use this for initialization
	public void Initialize (Transform activeMarkers) {
		print("heat map init: ");

		foreach(HeatMap hm in GameObject.FindObjectsOfType<HeatMap>()) {

			hm.pixelContainer = new GameObject("Pixels").transform;
			hm.pixelContainer.transform.SetParent(hm.transform);
			hm.pixelContainer.transform.localPosition = offset;

			//Get cell densities
			//2d array of real number marker hits
			hm.cellDensities = new int[hm.lengthInSquares, hm.lengthInSquares];
			foreach(Transform marker in activeMarkers) {

				//Determine containing cell
				//Convert Vector2 of 4.2 >  R > -4.2 into row and column for n-lengthed table [0, n - 1] [0, n - 1]
				print("input position: " + marker.position.ToString());

				//Map markers to cells
				int xCell = Mathf.FloorToInt(((marker.position.x + domeRadius) / (domeRadius * 2)) * hm.lengthInSquares);
				int yCell = Mathf.FloorToInt(((marker.position.y + domeRadius) / (domeRadius * 2)) * hm.lengthInSquares);
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
			float maxAlpha = 0f;
			for(int i = 0; i < hm.lengthInSquares; i++) {
				for(int j = 0; j < hm.lengthInSquares; j++) {
					GameObject pixel = Instantiate(hm.pixelPrefab) as GameObject;
					SpriteRenderer sr = pixel.GetComponent<SpriteRenderer>();


					float rawHits = hm.cellDensities[i, j];
//					print("hits: " + rawHits);
					float heat = (hm.cellDensities[i, j] / heatConstant);
//					print("sample heat: " + heat);
					if(heat > maxAlpha) maxAlpha = heat;

					sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, heat);
					hm.pixels.Add(pixel);

					pixel.transform.localScale = new Vector2((2 * domeRadius) / (lengthInSquares * 3) - spacing, (2 * domeRadius) / (lengthInSquares * 3) - spacing);
//					pixel.transform.position = new Vector2(i * hm.step, j * hm.step) + hm.offset;
					pixel.transform.SetParent(hm.pixelContainer);
					pixel.transform.localPosition = new Vector2(i * (((2 * domeRadius) / lengthInSquares)), j * (((2 * domeRadius) / lengthInSquares))) + new Vector2(-domeRadius, -domeRadius);

				}
			}
			print("max alpha = " + maxAlpha);
		}



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
