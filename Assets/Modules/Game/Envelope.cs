using UnityEngine;

namespace Game {
	public class Envelope : MonoBehaviour {
		public GameObject openModel, closeModel;
		public GameObject wax;

		[SerializeField] bool open;
		[SerializeField] SealingType sealing;

		public InstantElement instant;
		public TargetElement target;

		void UpdateAppearance() {
			openModel.SetActive(open);
			closeModel.SetActive(!open);
			wax.SetActive(!open && sealing == SealingType.Wax);
		}
		public bool Open {
			get => open;
			set {
				open = value;
				UpdateAppearance();
			}
		}
		public SealingType Sealing {
			get => sealing;
			set {
				sealing = value;
				UpdateAppearance();
			}
		}

		public void Inspect() {
			GameManager.instance.Inspect();
		}

		void Start() {
			Open = open;
			Sealing = sealing;
		}
	}
}