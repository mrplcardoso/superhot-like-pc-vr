using System.Collections;
using UnityEngine;
using TMPro;
using Utility.EasingEquations;
using Utility.EventHub;

public class Blink : MonoBehaviour
{
	TextMeshProUGUI textMesh;
	float alpha
	{
		get { return textMesh.color.a; }
		set { Color c = textMesh.color; c.a = value; textMesh.color = c; }
	}

	Coroutine routine;

	void Awake()
	{
		textMesh = GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		EventHub.instance.ObserveEvent(EventList.EnterStartTitle, PressFire);
		routine = StartCoroutine(Fade());
	}

	void PressFire(EventData e)
	{
		StartCoroutine(WaitInput());
	}

	IEnumerator WaitInput()
	{
		yield return new WaitWhile(() => !Input.GetButtonDown("Fire1"));
		StopCoroutine(routine); alpha = 0;
	}

	IEnumerator Fade()
	{
		float start = 0;
		float end = 1;
		float speed = 1f;
		float t = 0;
		while (true)
		{
			while (t < 1.01f)
			{
				alpha = EasingFloatEquations.Linear(start, end, t);
				t += Time.unscaledDeltaTime * speed;
				yield return null;
			}
			t = 0;
			while(t < 1.01f)
			{
				alpha = EasingFloatEquations.Linear(end, start, t);
				t += Time.unscaledDeltaTime * speed;
				yield return null;
			}
			t = 0;
			yield return null;
		}
	}

	private void OnDestroy()
	{
		EventHub.instance.RemoveObserver(EventList.EnterStartTitle, PressFire);
	}
}
