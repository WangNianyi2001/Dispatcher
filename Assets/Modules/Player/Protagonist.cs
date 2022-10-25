using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	[RequireComponent(typeof(PlayerInput))]
	public class Protagonist : Player {
		public static Protagonist instance;
		public Protagonist() {
			instance = this;
		}

		new Camera camera;

		public new void Rotate(Vector2 rotation) {
			base.Rotate(rotation);

			Vector3 cam = camera.transform.rotation.eulerAngles;
			cam.x += rotation.y;
			if(cam.x >= 180)
				cam.x -= 360;
			cam.x = Mathf.Clamp(cam.x, pitchRange.x, pitchRange.y);
			camera.transform.rotation = Quaternion.Euler(cam);
		}

		#region Input Handling
		Vector3 inputVelocity = Vector3.zero;
		Vector2 inputRotation = Vector2.zero;

		public void OnMove(InputValue value) {
			inputVelocity = value.Get<Vector2>();
			inputVelocity.z = inputVelocity.y;
			inputVelocity.y = 0;
		}

		public void OnOrient(InputValue value) {
			inputRotation = value.Get<Vector2>();
		}

		public void OnInteract() {
			CameraSelector selector = GetComponentInChildren<CameraSelector>();
			if(selector == null)
				return;
			selector.Use();
		}
		#endregion

		public GameObject aimUI;
		PlayerInput input;
		public bool Input {
			set {
				if(value) {
					UI = false;
					Cursor.lockState = CursorLockMode.Locked;
					input.SwitchCurrentActionMap("Protagonist");
				}
				aimUI?.SetActive(value);
			}
		}

		public bool UI {
			set {
				if(value) {
					Input = false;
					Cursor.lockState = CursorLockMode.None;
					input.SwitchCurrentActionMap("UI");
				}
			}
		}

		public new void Start() {
			base.Start();

			camera = GetComponentInChildren<Camera>();
			camera.tag = "MainCamera";
			input = GetComponent<PlayerInput>();
			Input = true;
		}

		public void Update() {
			Vector3 velocity = inputVelocity * movementSpeed * Time.deltaTime;
			velocity = transform.localToWorldMatrix.MultiplyVector(velocity);
			Move(velocity);

			Vector2 rotation = inputRotation * orientingSpeed * Time.deltaTime;
			rotation.y = -rotation.y;
			Rotate(rotation);
		}
	}
}