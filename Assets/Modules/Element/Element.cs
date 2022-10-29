using TMPro;
using UnityEngine;

namespace Game {
	[RequireComponent(typeof(Interactable))]
	public class Element : MonoBehaviour {
		public Interactable interactable;
		public Canvas canvas;
		public TMP_Text label;

		public string labelText = "Element";
		public string LabelText {
			get => label.text;
			set => label.text = labelText = value;
		}

		public bool active = true;
		public bool Active {
			get => active;
			set => interactable.enabled = active = value;
		}

		protected virtual void Start() {
			LabelText = labelText;
			Active = active;
		}
	}
}