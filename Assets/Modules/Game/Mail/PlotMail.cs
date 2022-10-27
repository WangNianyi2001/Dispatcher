using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Game {
	[CreateAssetMenu(menuName = "Mail/Plot")]
	public class PlotMail : Mail {
		public bool hasConsequence;
		[ShowIf("hasConsequence"), ResizableTextArea] public string consequence;

		public bool end;

		[ShowIf("end"), ResizableTextArea] public string endingRemarks;

		[Serializable]
		public class Successor {
			public Force force;
			public PlotMail mail;
		}
		[HideIf("end"), ReorderableList] public List<Successor> successors;
	}
}