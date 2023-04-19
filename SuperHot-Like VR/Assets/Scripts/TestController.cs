using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
	void UpdateF()
	{
		PrintAxis("Horizontal");
		PrintAxis("Vertical");

		PrintAxis("Fire1");
		PrintAxis("Fire2");
		PrintAxis("Fire3");

		PrintAxis("Jump");
		PrintAxis("Submit");
		PrintAxis("Cancel");
	}

	void PrintAxis(string axis)
	{
		float a = Input.GetAxis(axis);
		if (a != 0f)
		{ print(axis + ": " + a); }
	}
}
