using TMPro;
using UnityEngine;

namespace Game {
	public class ContentUI : MonoBehaviour {
		public TMP_Text text;

		public string Text {
			set => text.text = value;
		}

		public void Show() {
			gameObject.SetActive(true);
			GameManager.instance.EnterUI();
		}

		public void Show(string text) {
			Text = text;
			Show();
		}

		public void Quit() {
			gameObject.SetActive(false);
			GameManager.instance.QuitUI();
		}
	}
}