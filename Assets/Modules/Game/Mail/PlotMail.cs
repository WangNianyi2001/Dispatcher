using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Game {
	[CreateAssetMenu(menuName = "Mail/Plot")]
	public class PlotMail : Mail {
		public bool end;

		[ShowIf("end"), ResizableTextArea] public string endingRemarks;

		[Serializable]
		public struct Successor {
			public Force force;
			public Mail mail;
		}
		[HideIf("end"), ReorderableList] public List<Successor> successors;
	}
}