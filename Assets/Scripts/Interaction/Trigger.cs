using UnityEngine;
using NaughtyAttributes;

namespace Game {
	public class Trigger : MonoBehaviour {
		[Tag] public string tagMask;
		public bool oneTime = false;
		public ColliderEvent onEnter;
		public ColliderEvent onExit;

		bool Validate(Collider other) => string.IsNullOrEmpty(tagMask) || other.tag == tagMask;

		public void OnTriggerEnter(Collider other) {
			if(!Validate(other))
				return;
			onEnter.Invoke(other);
			if(oneTime)
				Destroy(this);
		}

		public void OnTriggerExit(Collider other) {
			if(!Validate(other))
				return;
			onExit.Invoke(other);
		}
	}
}