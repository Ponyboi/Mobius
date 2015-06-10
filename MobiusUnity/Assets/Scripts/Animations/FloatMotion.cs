using UnityEngine;
using System.Collections;

public class FloatMotion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static Vector3 floatMotion(Vector3 pos, float xFloatRange, float yFloatRange, float zFloatRange, float timeOffset) {
		float xSin =  xFloatRange * Mathf.Sin(Time.time + timeOffset);
		float ySin =  yFloatRange * Mathf.Sin(Time.time + timeOffset);
		float zSin =  zFloatRange * Mathf.Sin(Time.time + timeOffset);
		
		return new Vector3(pos.x + xSin, pos.y + ySin, pos.z + zSin);
	}
}
