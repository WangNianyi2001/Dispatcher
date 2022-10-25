using UnityEngine;
using NaughtyAttributes;

namespace Game {
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviour {
		#region Inspector fields
		[Header("Movement")]
		[Range(1, 500)] public float movementSpeed;

		[Header("Orientation")]
		[Range(1, 10)] public float orientingSpeed;
		[MinMaxSlider(-90, 90)] public Vector2 pitchRange;
		#endregion

		protected CharacterController controller;

		#region Movements
		public virtual void Move(Vector3 velocity) {
			controller.SimpleMove(velocity);
		}

		public virtual void Rotate(Vector2 rotation) {
			Vector3 body = transform.rotation.eulerAngles;
			body.y += rotation.x;
			transform.rotation = Quaternion.Euler(body);
		}
		#endregion

		public void Start() {
			controller = GetComponent<CharacterController>();
		}
	}
}