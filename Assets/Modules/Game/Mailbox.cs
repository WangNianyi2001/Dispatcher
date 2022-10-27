using UnityEngine;

namespace Game {
	public class Mailbox : MonoBehaviour {
		public Force force;
		public TargetElement element;

		void Start() {
			if(force == Force.DontCare)
				Debug.LogWarning("Mailbox must have a force", this);
			element.LabelText = $"Post to {force}";
			element.interactable.onInteract.AddListener(_ => GameManager.instance.PostMail(force));
		}
	}
}