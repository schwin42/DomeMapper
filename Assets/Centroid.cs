using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Centroid
{
	public Vector2 pLimit;

	public Vector2 location;

	public Centroid (Vector2 pLimit, Vector2 locations) {
		this.pLimit = pLimit;
		this.location = location;
	}
}

