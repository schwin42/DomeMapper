using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

//	static bool hasInitialized = false;

	// Use this for initialization
	void Start () {
		Replicator.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void HandleMarkersReady() {
//		if(!hasInitialized) {
			HeatMap.Initialize();
//			hasInitialized = true;
//		}
//		float x = -4.2f;
//		float y = -4.2f;
//
//		int xCell = Mathf.FloorToInt(((x - -4.2f) / 8.4f) * 15);
//		int yCell = Mathf.FloorToInt(((y - -4.2f) / 8.4f) * 15);
//		print("x y cell: " + xCell + ", " + yCell);
	}
}
