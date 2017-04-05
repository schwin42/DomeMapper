using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Replicator : MonoBehaviour
{
	
	public Color color;

	public GameObject markerPrefab;


	public Transform poolTransform;
	public Transform activeMarkersContainer;

	private List<GameObject> markerPool;

	//Configurable

	public int max_m = 1000;
	public int m = 100;
	public float radius = 3;
	public int centroidCount = 4;
	public float alpha = 0.5f;

	//State
	public List<Centroid> centroids;

	//Bookkeeping
	private float lastM;
	private float lastRadius;
	private float lastAlpha;

	private List<Transform> activeMarkers = new List<Transform> ();
	List<Vector2> randomSeeds = new List<Vector2> ();

		void Update () {
	
			if(lastRadius != radius) {
				UpdatePositions();
			}
			lastRadius = radius;
	
		if(lastAlpha != alpha) {
				UpdatePositions();
			}
		lastAlpha = alpha;
	
			if(lastM != m) {
				GenerateMarkers();
				UpdatePositions();
			}
			lastM = m;
		}

	public void Initialize ()
	{
		
		//Create module pool
		poolTransform = transform.Find ("MarkerPool");

		//Create active objects container
		activeMarkersContainer = transform.Find ("ActiveMarkers");

		//Create marker gameobjects
		print ("creating pool objects: " + max_m);
		markerPool = new List<GameObject> ();
		for (int i = 0; i < max_m; i++) {
			markerPool.Add (Instantiate (markerPrefab) as GameObject);
			ReturnMarkerToPool (markerPool [i].transform);
		}

		//Generate random seeds
		for (int i = 0; i < max_m; i++) {
			randomSeeds.Add (new Vector2 (Random.value, Random.value));
		}//m-dimensional by n-dimensional array with random marker positions against 2d location 


		GenerateMarkers ();
		GenerateCentroids();
		UpdatePositions ();


	}

	// Update is called once per frame
	void UpdatePositions ()
	{
//		print("update positions");
		for (int i = 0; i < activeMarkers.Count; i++) {
//				print("setting position to: " + randomSeeds[i].ToString());
			activeMarkers [i].position = GetSmartMarkerPosition (randomSeeds [i].x, randomSeeds [i].y);
		}
	}

	void GenerateMarkers ()
	{
		print ("GENERATION");
		for (int i = 0; i < activeMarkers.Count; i++) { //Release into pool
			ReturnMarkerToPool (activeMarkers [i]);
		}
		activeMarkers.Clear ();
			
		for (int i = 0; i < m; i++) {
			activeMarkers.Add (AcquireMarkerFromPool ());
		}

		HeatMap hm = GetComponent<HeatMap> ();
		if (hm != null)
			hm.Initialize (activeMarkersContainer);

	}

	void GenerateCentroids() {
//		centroids = new List<Centroid> () { new Centroid(new Vector2(0.0f, ) };

		centroids = new List<Centroid>();
		float lastLowLimit = 0f;
		for(int i = 0; i < centroidCount; i++) {
			float highLowLimit = lastLowLimit + 1f / (float)(i + 1f);
			centroids.Add(new Centroid(new Vector2(lastLowLimit, highLowLimit), GetRandomPointInCircle(Random.value, Random.value)));
			lastLowLimit = highLowLimit;
		}
	}

	Vector2 GetRandomPointInCircle(float random1, float random2) {
		var angle = random1 * Mathf.PI * 2;
		var distance = Mathf.Sqrt (-2 * Mathf.Log (random2)) * radius;
		var x = distance * Mathf.Cos (angle);
		var y = distance * Mathf.Sin (angle);
		return new Vector2(x, y);
	}

	Vector2 GetSmartMarkerPosition (float random1, float random2)
	{
		var rawCoordinates = GetRandomPointInCircle(random1, random2);

		//Find parent centroid
		float random = Random.value;
		var matchingCentroids = centroids.Where(c => c.pLimit.x <= random && c.pLimit.y > random ).ToList();

		//Find matching centroid
		Vector2 targetCentroidLocation = Vector2.zero;
		if(matchingCentroids.Count > 1) {
			Debug.LogError("Something went wrong.");
		} else if (matchingCentroids.Count() == 1) {
			targetCentroidLocation = matchingCentroids[0].pLimit;
		} else { 
			//Set it to rawCoordinates
			targetCentroidLocation = rawCoordinates;
		}

		//Adjust raw location to be around centroid
		Vector2 adjustedLocation = alpha * rawCoordinates + (1 - alpha) * targetCentroidLocation;
		return adjustedLocation;
	}

	Transform AcquireMarkerFromPool ()
	{
		Transform marker = poolTransform.GetChild (0);
		marker.SetParent (activeMarkersContainer);
		return marker;
	}

	void ReturnMarkerToPool (Transform module)
	{
		module.SetParent (poolTransform);
	}


}
