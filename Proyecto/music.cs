using System;
using System.IO;
using NAudio.Wave;

public class BackgroundMusic
{
    private string[] _audioFilePaths;
    private IWavePlayer _outputDevice;
    private AudioFileReader _audioFile;
    private bool _isStoppedManually;

    public BackgroundMusic(string[] audioFilePaths)
    {
        _audioFilePaths = audioFilePaths;
        _isStoppedManually = false;
    }

    public void Play(int trackIndex)
    {
        if (_outputDevice != null && _outputDevice.PlaybackState == PlaybackState.Playing)
        {
            _outputDevice.Stop();
        }

        _audioFile = new AudioFileReader(_audioFilePaths[trackIndex]);
        _outputDevice = new WaveOutEvent();
        _outputDevice.Init(_audioFile);
        _outputDevice.PlaybackStopped += OnPlaybackStopped;
        _outputDevice.Play();
    }

    private void OnPlaybackStopped(object sender, StoppedEventArgs e)
    {
        if (!_isStoppedManually)
        {
            _audioFile.Position = 0; // Reinicia el audio al principio
            _outputDevice.Play();
        }
    }

    public void Stop()
    {
        if (_outputDevice != null)
        {
            _isStoppedManually = true;
            _outputDevice.Stop();
        }
    }
}