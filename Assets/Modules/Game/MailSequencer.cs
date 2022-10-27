using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Game {
	[CreateAssetMenu]
	public class MailSequencer : ScriptableObject {
		[ReorderableList] public List<GeneralMail> generalMails;
		[ReorderableList] public List<PlotMail> plotEntries;

		IEnumerator<Mail> MakeMailSequence() {
			foreach(Mail general in generalMails)
				yield return general;
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