using Service.Sounds;
using UnityEngine;

namespace Service
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayAudioOnStart : MonoBehaviour
    {
        [SerializeField] private Sound _sound;
        [SerializeField] private bool _loop;

        private void Start()
        {
            if (_loop)
                SoundManager.Instance.PlayLoop(_sound, GetComponent<AudioSource>());
            else
                SoundManager.Instance.Play(_sound, GetComponent<AudioSource>());
        }
    }
}
