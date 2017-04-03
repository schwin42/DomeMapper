using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

//	static bool hasInitialized = false;

	Replicator[] replicators;

	// Use this for initialization
	void Start () {
		replicators = GameObject.FindObjectsOfType<Replicator>();
		print("replicators: " + replicators.Length);
		foreach(Replicator r in replicators) {
			r.Initialize();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
