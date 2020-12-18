using UnityEngine;
using System.Collections;
using DG.Tweening;

public sealed class AudioUtils : MonoBehaviour
{
	private static AudioSource _audioSource;

	private static AudioUtils _instance;
	private static AudioUtils instance
	{
		get
		{
			AudioUtils script = _instance;

			if (script == null) 
			{
				GameObject gameObject = new GameObject();
				gameObject.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
				gameObject.name = "AudioUtils";

				script = gameObject.AddComponent<AudioUtils>();

				_audioSource = gameObject.AddComponent<AudioSource>();
			}

			return script;
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			_instance.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;

			if (_audioSource == null) {
				_audioSource = this.gameObject.AddComponent<AudioSource>();
			}
		}
		else 
		{
			DestroyImmediate(this);
		}
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = null;
			_audioSource = null;
		}
	}

	private IEnumerator PlayClipDelayed(AudioClip clip, Vector3 position, float delay, float volume)
	{
		yield return new WaitForSeconds(delay);
		AudioSource.PlayClipAtPoint(clip, position, volume);
	}

	public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
	{
		PlayClipAtPoint(clip, position, 0.0f, 1.0f);
	}

	public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float delay)
	{
		PlayClipAtPoint(clip, position, delay, 1.0f);
	}

	public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float delay, float volume)
	{
		if (clip == null) {
			return;
		}

		if (delay <= 0.0f) {
			AudioSource.PlayClipAtPoint(clip, position, volume);
		} else {
			AudioUtils.instance.StartCoroutine(AudioUtils.instance.PlayClipDelayed(clip, position, delay, volume));
		}
	}

	public static void PlayClipAtPoint(AudioClipExtended clip, Vector3 position)
	{
		PlayClipAtPoint(clip.audioClip, position, 0.0f, clip.volume);
	}

	public static void PlayClipAtPoint(AudioClipExtended clip, Vector3 position, float delay)
	{
		PlayClipAtPoint(clip.audioClip, position, delay, clip.volume);
	}

	public static void PlayClipAtPoint(AudioClipExtended clip, Vector3 position, float delay, float volume)
	{
		PlayClipAtPoint(clip.audioClip, position, delay, volume);
	}

	public static void PlayClipsAtPoint(AudioClip[] clips, Vector3 position)
	{
		if (clips == null) {
			return;
		}

		for (int i = 0; i < clips.Length; i++) {
			PlayClipAtPoint(clips[i], position, 0.0f, 1.0f);
		}
	}

	public static void PlayClipsAtPoint(AudioClip[] clips, Vector3 position, float delay)
	{
		if (clips == null) {
			return;
		}

		for (int i = 0; i < clips.Length; i++) {
			PlayClipAtPoint(clips[i], position, delay, 1.0f);
		}
	}

	public static void PlayClipsAtPoint(AudioClip[] clips, Vector3 position, float delay, float volume)
	{
		if (clips == null) {
			return;
		}

		for (int i = 0; i < clips.Length; i++) {
			PlayClipAtPoint(clips[i], position, delay, volume);
		}
	}

	public static void PlayClipsAtPoint(AudioClipExtended[] clips, Vector3 position)
	{
		if (clips == null) {
			return;
		}

		for (int i = 0; i < clips.Length; i++) {
			PlayClipAtPoint(clips[i], position, 0.0f, clips[i].volume);
		}
	}

	public static void PlayClipsAtPoint(AudioClipExtended[] clips, Vector3 position, float delay)
	{
		if (clips == null) {
			return;
		}

		for (int i = 0; i < clips.Length; i++) {
			PlayClipAtPoint(clips[i], position, delay, clips[i].volume);
		}
	}

	public static void PlayClipsAtPoint(AudioClipExtended[] clips, Vector3 position, float delay, float volume)
	{
		if (clips == null) {
			return;
		}

		for (int i = 0; i < clips.Length; i++) {
			PlayClipAtPoint(clips[i], position, delay, volume);
		}
	}

	public static void FadeOutAudio(AudioSource audio, float duration)
	{
		if (audio != null) {
			audio.DOFade(0.0f, duration);
		}
	}

	public static void FadeOutAudio(AudioSource[] audio, float duration)
	{
		if (audio == null) {
			return;
		}

		for (int i = 0; i < audio.Length; i++) {
			audio[i].DOFade(0.0f, duration);
		}
	}

	public static void FadeOutAudio(AudioSourceExtended[] audio, float duration)
	{
		if (audio == null) {
			return;
		}

		for (int i = 0; i < audio.Length; i++) {
			audio[i].FadeOut(duration);
		}
	}

	public static void FadeInAudio(AudioSource audio, float duration, float volume = 1.0f)
	{
		if (audio != null) {
			audio.DOFade(volume, duration);
		}
	}

	public static void FadeInAudio(AudioSource[] audio, float duration, float volume = 1.0f)
	{
		if (audio == null) {
			return;
		}

		for (int i = 0; i < audio.Length; i++) {
			audio[i].DOFade(volume, duration);
		}
	}

	public static void FadeInAudio(AudioSourceExtended[] audio, float duration, float volume = 1.0f)
	{
		if (audio == null) {
			return;
		}

		for (int i = 0; i < audio.Length; i++) {
			audio[i].audioSource.DOFade(volume, duration);
		}
	}

}
