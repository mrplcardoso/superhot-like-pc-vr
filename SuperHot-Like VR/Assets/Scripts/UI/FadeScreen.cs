using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.EasingEquations;
using Utility.EventHub;

public class FadeScreen : MonoBehaviour
{
	Image image;
	float alpha { get { return image.color.a; } 
		set { Color c = image.color; c.a = value; image.color = c; } }
	public static FadeScreen instance { get; private set; }
	void Awake()
	{
		FadeScreen[] f = FindObjectsOfType<FadeScreen>();
		for(int i = 0; i < f.Length; i++)
		{
			if (f[i] != this)
			{ Destroy(gameObject); return; }
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
		image = GetComponentInChildren<Image>();
	}

	void Start()
	{
		EventHub.instance.ObserveEvent(EventList.BlackScreenOn, BlackScreen);
		EventHub.instance.ObserveEvent(EventList.BlackScreenOff, TransparentScreen);
	}

	public void BlackScreen(EventData e)
	{
		StartCoroutine(Fade(1f));
	}

	public void TransparentScreen(EventData e)
	{
		StartCoroutine(Fade(0f));
	}

	IEnumerator Fade(float end)
	{
		float start = alpha;
		float t = 0;
		while(t < 1.01f)
		{
			alpha = EasingFloatEquations.Linear(start, end, t);
			t += Time.unscaledDeltaTime * 0.5f;
			yield return null;
		}
		alpha = end;
		yield return null;
		EventHub.instance.PostEvent(EventList.BlackScreenEnd);
	}

	private void OnDestroy()
	{
		EventHub.instance.RemoveObserver(EventList.BlackScreenOn, BlackScreen);
		EventHub.instance.RemoveObserver(EventList.BlackScreenOff, TransparentScreen);
	}
}
