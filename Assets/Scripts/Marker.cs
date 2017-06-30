using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public string text = "";
	private Vector3 originalLossyScale;

	void Start() {
		originalLossyScale = transform.lossyScale;
	}

	void Update() {
		//Marker stays always the same size
		transform.localScale = originalLossyScale / transform.parent.lossyScale.x;
	}
}
