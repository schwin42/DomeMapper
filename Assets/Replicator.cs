using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replicator : MonoBehaviour {

	public int max_m = 1000;

	public Color color;

	public GameObject markerPrefab;


	public Transform poolTransform;
	public Transform activeMarkersContainer;

	private List<GameObject> markerPool;

	public int m = 100;
	public float radius = 3;
	public float variance = 1;

	private float lastM;
	private float lastRadius;
	private float lastVariance;

	private List<Transform> activeMarkers = new List<Transform>();
	List<Vector2> randomSeeds = new List<Vector2>();

	// Use this for initialization


	void Update () {

		if(lastRadius != radius) {
			UpdatePositions();
		}
		lastRadius = radius;

		if(lastVariance != variance) {
			UpdatePositions();
		}
		lastVariance = variance;

		if(lastM != m) {
			GenerateMarkers();
			UpdatePositions();
		}
		lastM = m;
	}

	public static void Initialize() {

		foreach(Replicator r in GameObject.FindObjectsOfType<Replicator>()) {
			//Create module pool
			r.poolTransform = r.transform.Find("MarkerPool");

			//Create active objects container
			r.activeMarkersContainer = r.transform.Find("ActiveMarkers");

			//Create marker gameobjects
			r.markerPool = new List<GameObject>();
			for (int i = 0; i < r.max_m; i++) {
				r.markerPool.Add(Instantiate(r.markerPrefab) as GameObject);
				r.ReturnMarkerToPool(r.markerPool[i].transform);
			}

			//Generate random seeds
			for(int i = 0; i < r.max_m; i++) {
				r.randomSeeds.Add(new Vector2(Random.value, Random.value));
			}//m-dimensional by n-dimensional array with random marker positions against 2d location 
			r.UpdatePositions();
		}


	}

	// Update is called once per frame
	void UpdatePositions () {
		print("update positions");
		for(int i = 0; i < activeMarkers.Count; i++) {
//				print("setting position to: " + randomSeeds[i].ToString());
			activeMarkers[i].position = GetRandomPointFromSeed(randomSeeds[i].x, randomSeeds[i].y);
			}
	}

	void GenerateMarkers() {
		
		for(int i = 0; i < activeMarkers.Count; i++) { //Release into pool
			ReturnMarkerToPool(activeMarkers[i]);
				}
		activeMarkers.Clear();
			
		for(int i = 0; i < m; i++) {
			activeMarkers.Add(AcquireMarkerFromPool());
		}
		UpdatePositions();
		MainController.HandleMarkersReady();
	}

		Vector2 GetRandomPointFromSeed(float random1, float random2) {
		{
			
				var angle = random1 * Mathf.PI * 2;
//			var distance = random2 * radius * variance;
			var distance = Mathf.Sqrt(-2 * Mathf.Log(random2) * variance) * radius;
			var x =  distance * Mathf.Cos(angle);
			var y = distance * Mathf.Sin(angle);
			return new Vector2(x, y);
		}
	}

	Transform AcquireMarkerFromPool() {
		Transform marker = poolTransform.GetChild(0);
		marker.SetParent(activeMarkersContainer);
		return marker;
	}

	void ReturnMarkerToPool(Transform module) {
		module.SetParent( poolTransform);
	}


}
