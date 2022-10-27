using UnityEngine;

namespace Game {
	public class Mailbox : MonoBehaviour {
		public Force force;
		public InstantElement element;

		void Start() {
			element.LabelText = $"投递到{Constant.forceNames[force]}";
			element.interactable.onInteract.AddListener(_ => GameManager.instance.PostMail(force));
		}
	}
}