using System.Collections;
using UnityEngine;

namespace GTArena.Utilities
{
    public class AudioRollOff : MonoBehaviour
    {
        public float maxDistance = 30f;
        public float minVolume = 0f;
        public float maxVolume = 1f;

        AudioSource source;

        void Start() => source = GetComponent<AudioSource>();

        void Update()
        {
            if (source == null) return;

            var distance = Vector3.Distance(transform.position, GorillaTagger.Instance.transform.position);
            var t = Mathf.Clamp01(distance / maxDistance);
            var volume = Mathf.Lerp(maxVolume, minVolume, t);
            source.volume = volume;
        }
    }
}