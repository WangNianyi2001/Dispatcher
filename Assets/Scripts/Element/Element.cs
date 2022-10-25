using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Game {
	[RequireComponent(typeof(Interactable))]
	public class Element : MonoBehaviour {
		Interactable interactable;
		Canvas canvas;
		TMP_Text label;

		[SerializeField, Label("Label Text")]
		protected string initLabelText = "Element";
		public virtual string LabelText {
			set {
				if(label)
					label.text = value;
			}
		}

		public bool active = true;
		public bool Active {
			get {
				return GetComponent<Interactable>().enabled;
			}
			set {
				active = value;
				interactable.enabled = active;
				canvas.enabled = active;
			}
		}

		protected virtual void Start() {
			interactable = GetComponent<Interactable>();
			canvas = GetComponentInChildren<Canvas>(true);
			label = canvas.GetComponentInChildren<TMP_Text>();

			LabelText = initLabelText;
			Active = active;
		}
	}
}