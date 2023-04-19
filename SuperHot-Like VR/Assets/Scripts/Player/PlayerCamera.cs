using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	PlayerObject playerObject;
	Camera sceneCamera => CameraController.instance.sceneCamera;
	[SerializeField] Transform headPosition;
	[SerializeField] Transform rightHandPosition;

	[SerializeField] float sensitivity = 10;
	float xRotation, yRotation;
	const float minAngle = -60;
	const float maxAngle = 90;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;

		playerObject = GetComponent<PlayerObject>();
	}

	private void Start()
	{
		rightHandPosition = CameraController.instance.transform.GetChild(0);

		PostUpdate();
		playerObject.PostAction += PostUpdate;
		if (!CameraController.vrModeOn)
		{
			playerObject.FrameAction += FrameUpdate;
		}
		else
		{ playerObject.FrameAction += RotationRead; }
	}

	private void FrameUpdate()
	{
		RotationInput();
	}

	private void PostUpdate()
	{
		sceneCamera.transform.position = headPosition.position;
	}

	void RotationRead()
	{
		Vector3 euler = transform.eulerAngles;
		euler.y = sceneCamera.transform.eulerAngles.y;
		transform.eulerAngles = euler;
	}

	void RotationInput()
	{
		xRotation += Input.GetAxisRaw("Mouse X") * sensitivity;
		yRotation += Input.GetAxisRaw("Mouse Y") * sensitivity;
		yRotation = Mathf.Clamp(yRotation, minAngle, maxAngle);

		sceneCamera.transform.eulerAngles =
			new Vector3(-yRotation, xRotation, 0);
		transform.eulerAngles = new Vector3(0, xRotation, 0);
	}

	private void OnDisable()
	{
		playerObject.FrameAction -= FrameUpdate;
		playerObject.PostAction -= PostUpdate;
	}
}