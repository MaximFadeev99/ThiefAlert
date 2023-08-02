using System.Collections;
using UnityEngine;

public class Alarm : MonoBehaviour 
{
    private Coroutine _increaseVolume;
    private Coroutine _decreaseVolume;
    private AudioSource _audioSource;
    private float _volumeChangePerFram = 0.0005f;

    private void Awake()
    {
        float initialVolume = 0f;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = initialVolume;
    }

    public void ChangeVolume(bool isThiefInside) 
    {       
        if (isThiefInside)
        {
            if (_decreaseVolume != null)
                StopCoroutine(_decreaseVolume);

            GetComponent<Animator>().SetBool("isActive", true);
            _increaseVolume = StartCoroutine(IncreaseVolume());
        }
        else 
        {
            if (_increaseVolume != null) 
                StopCoroutine (_increaseVolume);

            GetComponent<Animator>().SetBool("isActive", false);
            _decreaseVolume = StartCoroutine(DecreaseVolume());
        }
    }

    private IEnumerator IncreaseVolume()
    {
        float maxVolume = 1f;

        if (_audioSource.isPlaying == false) 
            _audioSource.Play();

        while (_audioSource.volume < maxVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, maxVolume, _volumeChangePerFram);
            yield return null;
        }
    }

    private IEnumerator DecreaseVolume()
    {
        float minVolume = 0f;

        while (_audioSource.volume > minVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, minVolume, _volumeChangePerFram);
            yield return null;
        }

        _audioSource.volume = minVolume;
        _audioSource.Stop();
    }
}