using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// From https://www.gamedeveloper.com/audio/coding-to-the-beat---under-the-hood-of-a-rhythm-game-in-unity
public class Conductor : MonoBehaviour
{
    public const float MIN_ACCURACY = 0.25f;

    public List<Song> Songs;

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    private float songBpm;

    //The number of seconds for each song beat
    private float secPerBeat;

    //Current song position, in seconds
    private float songPosition;

    //Current song position, in beats
    private float songPositionInBeats;

    //How many seconds have passed since the song started
    private float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    private AudioSource musicSource;

    //The offset to the first beat of the song in seconds
    private float firstBeatOffset;

    //the number of beats in each loop
    private float beatsPerLoop;

    //the total number of loops completed since the looping clip first started
    private int completedLoops = 0;

    //The current position of the song within the loop in beats.
    private float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    private float loopPositionInAnalog;

    private float positionInBeat { get => loopPositionInAnalog * beatsPerLoop % 1; }

    //Conductor instance
    private static Conductor instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();
        //PlaySong(Songs[0].Name);
    }

    private void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //check new beat
        if (Mathf.FloorToInt(songPositionInBeats) != Mathf.FloorToInt(songPosition / secPerBeat))
        {
            OnBeat?.Invoke(Mathf.FloorToInt(songPosition / secPerBeat));
        }
        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;

        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
    }

    private void LoadSongInfo(Song song)
    {
        songBpm = song.BPM;
        beatsPerLoop = song.BeatsPerLoop;
        firstBeatOffset = song.FirstBeatOffset;
        completedLoops = 0;

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Stop();
        musicSource.clip = song.AudioClip;
        musicSource.Play();
    }

    public static void PlaySong(string name)
    {
        instance.LoadSongInfo(instance.Songs.Find(a => a.Name == name));
    }

    public static float TimeSinceLastBeat { get => instance.positionInBeat; }

    public static float BeatAccuracy(bool allowNegative = false)
    {
        float trueAccuracy = 0.5f - Mathf.Min(instance.positionInBeat, 1 - instance.positionInBeat);
        if (!allowNegative)
        {
            return trueAccuracy > MIN_ACCURACY ? (trueAccuracy - MIN_ACCURACY) / (0.5f - MIN_ACCURACY) : 0;
        }
        else
        {
            return
                 trueAccuracy > MIN_ACCURACY ? (trueAccuracy - MIN_ACCURACY) / (0.5f - MIN_ACCURACY) :
                (trueAccuracy < (0.5f - MIN_ACCURACY) ? -(0.5f - MIN_ACCURACY - trueAccuracy) / (0.5f - MIN_ACCURACY) : 0);
        }
    }

    public static int SongPositionInBeats { get => Mathf.FloorToInt(instance.songPositionInBeats); }

    public static Action<int> OnBeat;
}

[System.Serializable]
public record Song
{
    public string Name;
    public AudioClip AudioClip;
    public float BPM;
    public float BeatsPerLoop;
    public float FirstBeatOffset;
}