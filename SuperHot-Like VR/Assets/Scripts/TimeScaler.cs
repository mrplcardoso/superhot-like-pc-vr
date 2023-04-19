using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeScaler
{
	const float normalTime = 1f;
	const float delayedTime = 0.2f;
	public static void SetTimeScale(float time)
	{
		Time.timeScale = time;
	}
}
