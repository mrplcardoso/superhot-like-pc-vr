using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utility.EventHub;

public class StartGame : GameState
{
	[SerializeField] TextMeshProUGUI textMesh;
	public override void OnEnter()
	{
		EventHub.instance.PostEvent(EventList.EnterStartGame);
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText()
	{
		string message = "Break every crystal enemy and don't get shoot";
		int length = message.Length;
		for(int i = 0; i < length; i++)
		{
			textMesh.text += message[i];
			yield return new WaitForSeconds(0.15f);
		}
		yield return new WaitForSeconds(2f);
		textMesh.text = "GO!";
		yield return new WaitForSeconds(1f);
		textMesh.gameObject.SetActive(false);
		gameMachine.ChangeStateIntervaled<UpdateGame>();
	}
}
