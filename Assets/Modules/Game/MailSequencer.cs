using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using System;
using Random = UnityEngine.Random;

namespace Game {
	[CreateAssetMenu]
	public class MailSequencer : ScriptableObject {
		[Serializable]
		public struct ReputationRecord {
			public Force force;
			public float reputation;
		}
		[SerializeField, ReorderableList] public List<ReputationRecord> reputations;

		[ReorderableList] public List<GeneralMail> generalMails;
		[ReorderableList] public List<PlotMail> plotEntries;

		[ReorderableList] public List<PlotMail.Successor> goodEndings;

		[NonSerialized] public Force choice;

		IEnumerator<Mail> MakeMailSequence() {
			for(int i = 0; i < plotEntries.Count; ++i) {
				for(var currentPlot = plotEntries[i]; ;) {
					if(currentPlot == null)
						break;
					for(int generalCount = Random.Range(1, 3); generalCount > 0; --generalCount)
						yield return generalMails[Random.Range(0, generalMails.Count - 1)];
					yield return currentPlot;
					if(currentPlot.end)
						break;
					currentPlot = currentPlot.successors.Find(s => s.force == choice)?.mail;
				}
			}
			while(true)
				yield return null;
		}
		IEnumerator<Mail> enumerator;

		public MailSequencer() {
			Reset();
		}

		#region Pubic interface
		public void Reset() {
			enumerator = MakeMailSequence();
		}

		public Mail Next() {
			enumerator.MoveNext();
			Mail mail = enumerator.Current;
			if(!mail)
				return null;
			return mail;
		}
		#endregion
	}
}