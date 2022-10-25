using UnityEngine;

namespace Game {
	public class Usable : MonoBehaviour {
		public ComponentEvent onSelect;
		public ComponentEvent onDeselect;
		public ComponentEvent onUse;

		public void OnSelect(Component source) {
			if(!isActiveAndEnabled)
				return;
			onSelect.Invoke(source);
		}
		public void OnDeselect(Component source) {
			if(!isActiveAndEnabled)
				return;
			onDeselect.Invoke(source);
		}
		public void OnUse(Component source) {
			if(!isActiveAndEnabled)
				return;
			onUse.Invoke(source);
		}
	}
}