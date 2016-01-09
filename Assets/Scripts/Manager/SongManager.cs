using UnityEngine;
using System.Collections;

public class SongManager : MonoBehaviour {

    [SerializeField]
    AudioClip[] songs;
    AudioSource source;

    public enum Song { Menu, Loading, MoveMode, KillMode}

    void Awake() {
        source = GetComponent<AudioSource>();
    }

    AudioClip GetRelatedAudioClip(Song song) {
        switch (song) {
            case Song.Menu: return songs[0];
            default: return null;
        }
    }

    public void PlaySong(Song song) {
        source.clip = GetRelatedAudioClip(song);
        source.Play();
    }
}
