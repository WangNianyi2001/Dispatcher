using UnityEngine;

public class Billboard : MonoBehaviour {
	void FixedUpdate() {
		if(Camera.main == null)
			return;
		Transform camT = Camera.main.transform;
		transform.rotation = Quaternion.LookRotation(camT.forward, camT.up);
	}
}
