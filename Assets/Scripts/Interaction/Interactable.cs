using UnityEngine;

namespace Game {
	public class Interactable : MonoBehaviour {
		public Usable user;

		public Trigger trigger;

		public ComponentEvent onInteract;

		public void Start() {
			if(user == null)
				user = GetComponent<Usable>();
			if(user == null)
				Debug.LogWarning("User of interactable is not set", this);
			else
				user.onUse.AddListener(onInteract.Invoke);
		}

		public void OnInteract(Component source) {
			if(!isActiveAndEnabled)
				return;
			onInteract.Invoke(source);
		}
	}
}