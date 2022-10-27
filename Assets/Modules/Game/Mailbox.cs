using UnityEngine;

namespace Game {
	public class Mailbox : MonoBehaviour {
		public Force force;
		public InstantElement element;

		void Start() {
			if(force == Force.DontCare)
				Debug.LogWarning("Mailbox must have a force", this);
			element.LabelText = $"Post to {force}";
			Debug.Log(element.LabelText, element);
			element.interactable.onInteract.AddListener(_ => GameManager.instance.PostMail(force));
		}
	}
}