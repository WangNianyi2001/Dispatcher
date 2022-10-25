using UnityEngine;
using System.Linq;

namespace Game {
	public class TargetElement : InstantElement {
		HoldableElement selected;

		public void Select() {
			HoldableElement holding = Protagonist.instance.holding;
			if(!holding || !holding.targets.Any(target => target == this))
				return;

			selected = holding;
			canvas.gameObject.SetActive(true);
		}

		public void Deselect() {
			selected = null;
			canvas.gameObject.SetActive(false);

		}

		public void Interact() {
			if(!selected)
				return;
			selected.OnPlace(this);
		}

		protected override void Start() {
			base.Start();

			interactable.user.onSelect.AddListener((Component _) => Select());
			interactable.user.onDeselect.AddListener((Component _) => Deselect());
			interactable.onInteract.AddListener((Component _) => Interact());
		}
	}
}