using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class Envelope : MonoBehaviour {
		public GameObject openModel, closeModel;
		public GameObject wax;

		[SerializeField] bool open;
		[SerializeField] SealingType sealing;
		public static Dictionary<SealingType, string> sealingPrompt = new Dictionary<SealingType, string> {
			{ SealingType.None, "阅读信件" },
			{ SealingType.Glue, "已胶封" },
			{ SealingType.Wax, "已蜡封" },
		};

		public InstantElement instant;
		public TargetElement target;

		void UpdateAppearance() {
			openModel.SetActive(open);
			closeModel.SetActive(!open);
			wax.SetActive(!open && sealing == SealingType.Wax);
			instant.LabelText = sealingPrompt[sealing];
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
				Open = sealing == SealingType.None;
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