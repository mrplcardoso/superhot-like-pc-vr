using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.SpatialTracking;

public class CameraController : MonoBehaviour
{
	public static CameraController instance { get; private set; }
	public TrackedPoseDriver driver { get; private set; }
	public CameraRaycaster raycaster {get; private set;}
	public Camera sceneCamera { get; private set; }
	public static bool vrModeOn => (Application.platform == RuntimePlatform.Android);

	private void Awake()
	{
		CameraController[] controllers = FindObjectsOfType<CameraController>();
		for (int i = 0; i < controllers.Length; ++i)
		{
			if (controllers[i] != this)
			{ Destroy(gameObject); return; }
		}
		instance = this;
		DontDestroyOnLoad(gameObject);

		driver = GetComponent<TrackedPoseDriver>();
		raycaster = GetComponent<CameraRaycaster>();
		sceneCamera = GetComponent<Camera>();

	}

	void Start()
	{
		if(!vrModeOn)
		{ GetComponent<CardboardStartup>().enabled = false; }
	}
}
