using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	private GameAI gameAI;
	public GameObject handle;
	public bool state = false;
	public bool oldState = false;
	private bool keyState = true;
	private bool inTrigger = false;
	public RoomLayer roomLayerTarget = RoomLayer.Future;

	// Use this for initialization
	void Start () {
		handle = transform.Find("HandlePivot").gameObject;
		gameAI = GameObject.Find("GameAI").GetComponent<GameAI>();
	}
	
	// Update is called once per frame
	void Update () {
		if (inTrigger) {
			if (Input.GetKey(KeyCode.E) && keyState) {
				state = !state;
				keyState = false;
			}
			if (Input.GetKeyUp(KeyCode.E))
				keyState = true;
		}

		if (state != oldState) {
			if (state) {
				handle.transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z -110);
			} else {
				handle.transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
			}
			oldState = state;
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player") {
			inTrigger = true;
		}
	}
	void OnTriggerLeave (Collider col) {
		if (col.gameObject.tag == "Player") {
			inTrigger = false;
		}
	}
}
