using UnityEngine;
using System.Collections;

public class SongManager : MonoBehaviour {

    [SerializeField]
    private AudioClip[] songs;
    private AudioSource source;

    public enum Song { Menu, Loading, MoveMode, KillMode}

    void Awake() {
        source = GetComponent<AudioSource>();
    }

    AudioClip GetRelatedAudioClip(Song song) {
        switch (song) {
            case Song.MoveMode: return songs[0];
            default: return null;
        }
    }

    public void PlaySong(Song song) {
        source.clip = GetRelatedAudioClip(song);
        source.Play();
    }
}
