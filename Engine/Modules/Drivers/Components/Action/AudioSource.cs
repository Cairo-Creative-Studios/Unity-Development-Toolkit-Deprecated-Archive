//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;

namespace UDT.Drivers
{
    [Serializable]
    public class AudioSource : Driver<DriverTemplate_AudioSource>
    {
        UnityEngine.AudioSource audioSource;

        void Start()
        {
            //Initialize the Audio Source Component
            audioSource = (UnityEngine.AudioSource)GetProperty("audioSourceComponent");

            if (audioSource == null)
            {
                audioSource = (UnityEngine.AudioSource)SetProperty("audioSourceComponent", rootTransform.Find(template.audioSourcePath).gameObject.GetComponent<UnityEngine.AudioSource>());

                if (audioSource == null)
                {
                    audioSource = (UnityEngine.AudioSource)SetProperty("audioSourceComponent", rootTransform.Find(template.audioSourcePath).gameObject.AddComponent<UnityEngine.AudioSource>());
                }
            }
        }

        /// <summary>
        /// Plays the specified Audio Clip
        /// </summary>
        /// <param name="audioClip">Audio clip.</param>
        public void PlayOneShot(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }

        /// <summary>
        /// Plays an Audio Clip from one of the Audio Clip Pools in the Template, by Name
        /// </summary>
        /// <param name="clipName">Clip name.</param>
        public void PlayOneShot(string clipName)
        {
            if (template.audioClips.ContainsKey(clipName))
                audioSource.PlayOneShot(template.audioClips[clipName][UnityEngine.Random.Range(0, template.audioClips[clipName].Count - 1)]);
        }
    }
}
