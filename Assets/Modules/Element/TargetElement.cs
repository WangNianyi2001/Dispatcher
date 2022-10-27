using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Game {
	public class TargetElement : InstantElement {
		public List<HoldableElement> sources;

		HoldableElement selected;

		public void Select() {
			HoldableElement holding = Protagonist.instance.holding;
			if(!holding)
				return;

			if(!(
				holding.targets.Any(target => target == this)
				|| sources.Any(source => source == holding)
			))
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