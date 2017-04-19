using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Centroid
{
	public Vector2 pLimit;

	public Vector2 location;

	public GameObject marker;

	public Centroid (Vector2 pLimit, Vector2 location, GameObject marker) {
		this.pLimit = pLimit;
		this.location = location;
		this.marker = marker;
	}
}

