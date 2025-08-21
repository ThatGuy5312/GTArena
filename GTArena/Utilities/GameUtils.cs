using System.Collections;
using UnityEngine;

namespace GTArena.Utilities
{
    public class GameUtils : MonoBehaviour
    {
        public static (AudioSource, AudioRollOff) PlayClip(AudioClip clip, Vector3 position)
        {
            var source = new GameObject("Audio Object").AddComponent<AudioSource>();
            source.gameObject.transform.position = position;
            var aro = source.gameObject.AddComponent<AudioRollOff>();
            source.clip = clip;
            source.loop = false;
            source.Play();
            Destroy(source.gameObject, clip.length);
            return (source, aro);
        }

        public static (AudioSource, AudioRollOff) PlayClip(AudioClip clip, Transform parent)
        {
            var source = new GameObject("Audio Object").AddComponent<AudioSource>();
            source.gameObject.transform.parent = parent;
            source.gameObject.transform.localPosition = Vector3.zero;
            var aro = source.gameObject.AddComponent<AudioRollOff>();
            source.clip = clip;
            source.loop = false;
            source.Play();
            Destroy(source.gameObject, clip.length);
            return (source, aro);
        }
    }
}