using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game {
	public class ReputationUI : MonoBehaviour {
		[Serializable] public class Pair {
			public Force force;
			public TMP_Text text;
		}
		[ReorderableList] public List<Pair> values;

		public void Set(Force force, float value) {
			var pair = values.Find(p => p.force == force);
			if(pair == null)
				return;
			pair.text.text = $"{Constant.forceNames[force]}：{value}";
		}
	}
}