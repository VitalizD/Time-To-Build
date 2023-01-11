using UnityEngine;
using System.Collections.Generic;

namespace Service.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private const string VOLUME_SOUNDS_KEY = "VOLUME_SOUNDS";

        [SerializeField] private TypedAudio[] _clips;

        private AudioSource _audioSource;
        private readonly Dictionary<Sound, AudioClip[]> _sounds = new();
        private readonly Dictionary<Sound, Queue<AudioSource>> _playingSounds = new();

        public void Save()
        {
            PlayerPrefs.SetFloat(VOLUME_SOUNDS_KEY, _audioSource.volume);
        }

        public void Load()
        {
            _audioSource.volume = PlayerPrefs.GetFloat(VOLUME_SOUNDS_KEY, _audioSource.volume);
        }

        public void Play(Sound sound, AudioSource source, int index)
        {
            if (!_sounds.ContainsKey(sound))
                return;

            if (source == null)
                source = _audioSource;

            if (_sounds[sound].Length == 0)
            {
                Debug.LogWarning("There is no audio clip " + sound.ToString());
                return;
            }
            if (!_sounds.ContainsKey(sound))
                AddLoopAudio(sound, source);

            source.PlayOneShot(_sounds[sound][index]);
        }

        public void Play(Sound sound, AudioSource source)
        {
            if (!_sounds.ContainsKey(sound))
                return;
            Play(sound, source, Random.Range(0, _sounds[sound].Length));
        }

        public void PlayOneStream(Sound sound, AudioSource source)
        {
            if (source == null)
                return;

            source.Stop();
            source.PlayOneShot(GetRandomClip(sound));
        }

        public void PlayLoop(Sound sound, AudioSource source)
        {
            if (source == null)
                return;

            source.loop = true;
            AddLoopAudio(sound, source);
            source.PlayOneShot(GetRandomClip(sound));
        }

        public void Stop(Sound sound)
        {
            if (!_playingSounds.ContainsKey(sound) || _playingSounds[sound].Count == 0)
                return;

            _playingSounds[sound].Dequeue().Stop();
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _audioSource = GetComponent<AudioSource>();

            foreach (var clip in _clips)
                _sounds.Add(clip.Type, clip.Clips);
        }

        private void Update()
        {
            foreach (var element in _playingSounds)
            {
                foreach (var sound in element.Value)
                {
                    if (sound.isPlaying)
                        continue;
                    sound.PlayOneShot(GetRandomClip(element.Key));
                }
            }
        }

        private AudioClip GetRandomClip(Sound sound) => _sounds[sound][Random.Range(0, _sounds[sound].Length)];

        private void AddLoopAudio(Sound sound, AudioSource source)
        {
            if (_playingSounds.ContainsKey(sound))
                _playingSounds[sound].Enqueue(source);
            else
                _playingSounds.Add(sound, new Queue<AudioSource>(new[] { source }));
        }
    }

    [System.Serializable]
    public class TypedAudio
    {
        public Sound Type;
        public AudioClip[] Clips;
    }
}