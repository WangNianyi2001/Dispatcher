using UnityEngine;

namespace Game {
	public class HoldableElement : Element {
		public void Hold() {
			Protagonist.instance.Interact(this);
		}
	}
}