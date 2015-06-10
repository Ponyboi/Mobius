// Copyright (C) Stanislaw Adaszewski, 2013
// http://algoholic.eu

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleport : MonoBehaviour {

	public GameManager gm; 
	public Transform OtherEnd;
	public bool heightOffset; //up or down
	public int sign;
	HashSet<Collider> colliding = new HashSet<Collider>();

	// Use this for initialization
	void Start () {
		gm = GameObject.Find("_GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnTriggerEnter(Collider other) {
		if (!colliding.Contains(other)) {
			
			Quaternion q1 = Quaternion.FromToRotation(transform.up, OtherEnd.up);
			Quaternion q2 = Quaternion.FromToRotation(-transform.up, OtherEnd.up);
			
			Vector3 newPos = OtherEnd.position + q2 * (other.transform.position - transform.position) + (sign * Vector3.up * 2 * gm.staircaseYOffset);
				;// + OtherEnd.transform.up * 2;;
			
			if (other.GetComponent<Rigidbody>() != null) {
				GameObject o = (GameObject) GameObject.Instantiate(other.gameObject, newPos, other.transform.localRotation);
				o.GetComponent<Rigidbody>().velocity = q2 * other.GetComponent<Rigidbody>().velocity;
				o.GetComponent<Rigidbody>().angularVelocity = other.GetComponent<Rigidbody>().angularVelocity;
				other.gameObject.SetActive(false);
				Destroy(other.gameObject);				
				other = o.GetComponent<Collider>();
			}
			
			OtherEnd.GetComponent<Teleport>().colliding.Add(other);

			if (other.gameObject.GetComponent<vp_FPController>() != null) {
				var fpController = other.gameObject.GetComponent<vp_FPController>();
				Debug.Log (fpController.transform.position);
				Debug.Log (fpController.m_SmoothPosition);
				Debug.Log (fpController.m_FixedPosition);
				fpController.SetPosition(newPos);
				Debug.Log ("char pos");
				Debug.Log (fpController.transform.position);
				Debug.Log (fpController.m_SmoothPosition);
				Debug.Log (fpController.m_FixedPosition);
			} else {
				other.transform.position = newPos;
			}
			
			
			Vector3 fwd = other.transform.forward;
			
			if (other.GetComponent<Rigidbody>() == null) {
				other.transform.LookAt(other.transform.position + q2 * fwd, OtherEnd.transform.forward);
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		colliding.Remove(other);
	}
}
