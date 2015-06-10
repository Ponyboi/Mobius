using UnityEngine;
using System.Collections;

public class Step : MonoBehaviour {
	private Vector3 pos;
	private float yFloatRange;
	private float xFloatRange;
	private float zFloatRange;
	private float yRand = 0.016f;
	private float xRand = 0.02f;
	private float zRand = 0.01f;

	private float timeOffset;
	private float minRange = 0.01f;

	// Use this for initialization
	void Start () {
		pos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		yFloatRange = (Random.value * yRand) + minRange;
		xFloatRange = (Random.value * xRand) + minRange;
		zFloatRange = (Random.value * zRand) + minRange;
		timeOffset = Random.value * 360 * Mathf.Deg2Rad;
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = FloatMotion.floatMotion(pos, xFloatRange, yFloatRange, zFloatRange, timeOffset);
	}
}
