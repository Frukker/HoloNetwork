using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerMouseEvents : MonoBehaviour {

	[Range(1.0f, 2.0f)]
	public float maxScaleFactor = 1.2f;

	private float heldTime = 0.0f;
	private float curScaleFactor = 1.0f;
	private Vector3 initScale;

	private Coroutine scaleUpRoutine;
	private Coroutine scaleDownRoutine;

	void Start() {
		initScale = transform.localScale;
	}

	void Update() {
		//if (Input.GetMouseButton(0)) 
			//heldTime += Time.deltaTime;
	}

	void OnMouseEnter() {
		if (scaleDownRoutine != null)
			StopCoroutine(scaleDownRoutine);

		scaleUpRoutine = StartCoroutine(ScaleUp());
	}

	void OnMouseExit() {
		if (scaleUpRoutine != null)
			StopCoroutine(scaleUpRoutine);

		scaleDownRoutine = StartCoroutine(ScaleDown());
	}

	void OnMouseUp() {
		//if (heldTime < 2.0f)
			MarkerManager.SetMarkerActive(transform.parent.gameObject);
		//else
			//MarkerManager.DestroyMarker(transform.parent.gameObject);
	}

	IEnumerator ScaleUp() {
		while (curScaleFactor < maxScaleFactor) {
			curScaleFactor += Time.deltaTime;

			transform.localScale = initScale * curScaleFactor;

			yield return null;
		}
	}

	IEnumerator ScaleDown() {
		while (curScaleFactor > 1.0f) {
			curScaleFactor -= Time.deltaTime;

			transform.localScale = initScale * curScaleFactor;

			yield return null;
		}
	}
}
