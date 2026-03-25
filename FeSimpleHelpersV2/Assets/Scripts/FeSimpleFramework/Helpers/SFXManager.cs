using FeSimpleHelpers.Core;
using UnityEngine;

namespace FeSimpleHelpers.General
{
	/// <summary>
	/// Singleton that manages all the SFX of the game, including music and ambient sounds.
	/// Remove if using something like FMOD or Wwise
	/// </summary>
	public class SFXManager : MonoBehaviourSingleton<SFXManager>
	{
		public AudioSource sound2DPrefab;
		public AudioSource sound3DPrefab;
		public AudioSource musicSource;
		public AudioSource ambientSource;

		public AudioClip buttonSound;

		public static void S_PlayButtonSound()
		{
			if (!Exists)
				return;
			Get().PlaySound2D(Get().buttonSound);
		}

		public static void S_PlaySound2D(AudioClip ac)
		{
			if (!Exists)
				return;
			Get().PlaySound2D(ac);
		}

		public static void S_PlaySound(AudioClip ac, Vector3 pos)
		{
			if (!Exists)
				return;
			Get().PlaySound(ac, pos);
		}

		public void PlaySound2D(AudioClip ac)
		{
			AudioSource aSource = Instantiate(sound2DPrefab);
			aSource.clip = ac;
			aSource.Play();
		}

		public void PlaySound(AudioClip ac, Vector3 pos)
		{
			AudioSource.PlayClipAtPoint(ac, pos);
		}

		// //////////////////////// MUSIC //////////////////////////////////////////////////////

		public static void S_PlayMusic(AudioClip ac)
		{
			if (!Exists)
				return;
			Get().PlayMusic(ac);
		}

		public static void S_StopMusic()
		{
			if (!Exists)
				return;
			Get().StopMusic();
		}

		public void PlayMusic(AudioClip ac)
		{
			musicSource.Stop();
			musicSource.clip = ac;
			musicSource.Play();
		}

		public void StopMusic()
		{
			musicSource.Stop();
		}


		// //////////////////////// AMBIENT //////////////////////////////////////////////////////

		public static void S_PlayAmbient(AudioClip ac)
		{
			if (!Exists)
				return;
			Get().PlayAmbient(ac);
		}

		public static void S_StopAmbient()
		{
			if (!Exists)
				return;
			Get().StopAmbient();
		}

		public void PlayAmbient(AudioClip ac)
		{
			ambientSource.Stop();
			ambientSource.clip = ac;
			ambientSource.Play();
		}

		public void StopAmbient()
		{
			ambientSource.Stop();
		}
	}
}