using System.Collections.Generic;
using UnityEngine;
using Utility.EventHub;

namespace Utility
{
	namespace Audio
	{
		public class AudioHub : MonoBehaviour
		{
			public static AudioHub instance { get; private set; }
			[SerializeField] AudioDataBase dataBase;
			Dictionary<string, CustomClip> clipMap;

			List<AudioSource> sfxSource;
			List<AudioSource> bgmSource;
			GameObject sourceContainer;

			const int audioPerSource = 32;

			[SerializeField] bool isMute = false;
			public bool mute
			{
				get { return isMute; }
				set
				{
					isMute = value;
					for (int i = 0; i < sfxSource.Count; ++i)
					{ sfxSource[i].enabled = !value; }
					for (int i = 0; i < bgmSource.Count; ++i)
					{ bgmSource[i].enabled = !value; }
				}
			}

			private void Awake()
			{
				AudioHub[] g = GameObject.FindObjectsOfType<AudioHub>();
				if (g.Length > 0)
				{
					for (int i = 0; i < g.Length; ++i)
					{ if (g[i].gameObject != gameObject) { Destroy(gameObject); return; } }
				}
				DontDestroyOnLoad(gameObject);
				instance = this;

				LoadMap();

				this.ObserveEvent(EventList.AudioPlayOneTime, PlayOneTimeEvent);
				this.ObserveEvent(EventList.AudioPlayLoop, PlayLoopEvent);
				this.ObserveEvent(EventList.AudioPlayIntroLoop, PlayIntroLoopEvent);
				this.ObserveEvent(EventList.DestroyAudioHub, AutoDestroy);
				this.ObserveEvent(EventList.AudioStop, StopEvent);
				this.ObserveEvent(EventList.AudioMute, Mute);

				mute = false;
			}

			void AutoDestroy(EventData data)
			{
				Destroy(this.gameObject);
			}

			void Mute(EventData data)
			{
				if (mute)
				{ mute = false; }
				else
				{ mute = true; }
			}

			private void OnDestroy()
			{
				this.RemoveObserver(EventList.AudioPlayOneTime, PlayOneTimeEvent);
				this.RemoveObserver(EventList.AudioPlayLoop, PlayLoopEvent);
				this.RemoveObserver(EventList.AudioPlayIntroLoop, PlayIntroLoopEvent);
				this.RemoveObserver(EventList.DestroyAudioHub, AutoDestroy);
				this.RemoveObserver(EventList.AudioStop, StopEvent);
				this.RemoveObserver(EventList.AudioMute, Mute);
			}

			void LoadMap()
			{
				sourceContainer = new GameObject("Source Container");
				sourceContainer.transform.parent = transform;

				sfxSource = new List<AudioSource>();
				bgmSource = new List<AudioSource>();
				clipMap = dataBase.LoadTable();

				int sfxIndex = 0, sfxCount = 0;
				CustomClip[] sfx = dataBase.LoadArray(ClipType.SFX);
				for (int i = 0; i < sfx.Length; ++i)
				{
					if (sfxCount >= audioPerSource || sfxSource.Count == 0)
					{
						sfxSource.Add(sourceContainer.AddComponent<AudioSource>());
						sfxIndex++;
						sfxCount = 0;
					}
					clipMap[sfx[i].audioName].source = sfxSource[sfxIndex - 1];
					sfxCount++;
				}

				int bgmIndex = 0, bgmCount = 0;
				CustomClip[] bgm = dataBase.LoadArray(ClipType.BGM);
				for (int i = 0; i < bgm.Length; ++i)
				{
					if (bgmCount >= audioPerSource || bgmSource.Count == 0)
					{
						bgmSource.Add(sourceContainer.AddComponent<AudioSource>());
						bgmIndex++;
						bgmCount = 0;
					}
					clipMap[bgm[i].audioName].source = bgmSource[bgmIndex - 1];
					bgmCount++;
				}
			}

			public void PlayOneTime(string audioName)
			{
				if (!clipMap.ContainsKey(audioName))
				{ PrintConsole.Error("No '" + audioName + "' audio found"); return; }

				clipMap[audioName].source.PlayOneShot(clipMap[audioName].audioClip,
					clipMap[audioName].volume);
			}

			public void PlayOneTimeEvent(EventData data)
			{
				string audioName = (string)data.eventInformation;
				PlayOneTime(audioName);
			}

			public void PlayLoop(string audioName, bool forceSet = true)
			{
				if (!clipMap.ContainsKey(audioName))
				{ PrintConsole.Error("No '" + audioName + "' audio found"); return; }

				AudioSource source = clipMap[audioName].source;

				if (source.clip != null)
				{
					PrintConsole.Warning("Already playing '" + audioName + "' loop");
					if (!forceSet)
					{ return; }
				}

				source.volume = clipMap[audioName].volume;
				source.loop = true;
				source.clip = clipMap[audioName].audioClip;
				source.Play();
			}

			public void PlayLoopEvent(EventData data)
			{
				string audioName = (string)data.eventInformation;
				PlayLoop(audioName);
			}

			public void PlayLoop(string audioName, string introName, bool forceSet = true)
			{
				if (!clipMap.ContainsKey(audioName))
				{ PrintConsole.Error("No '" + audioName + "' audio found"); return; }

				AudioSource source = clipMap[audioName].source;

				if (source.clip != null)
				{
					PrintConsole.Warning("Already playing '" + audioName + "' loop");
					if (!forceSet)
					{ return; }
				}

				source.volume = clipMap[audioName].volume;
				source.loop = true;
				source.clip = clipMap[audioName].audioClip;
				source.PlayDelayed(clipMap[introName].audioClip.length);
				PlayOneTime(introName);
			}

			public void PlayIntroLoopEvent(EventData data)
			{
				string[] audioName = (string[])data.eventInformation;
				PlayLoop(audioName[1], audioName[0]);
			}

			public void Stop(string audioName)
			{
				clipMap[audioName].source.Stop();
			}

			public void StopEvent(EventData data)
			{
				string audioName = (string)data.eventInformation;
				Stop(audioName);
			}
		}
	}
}