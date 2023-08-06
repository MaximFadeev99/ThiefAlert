using System.Collections;
using UnityEngine;

public class Alarm : MonoBehaviour 
{
    [SerializeField] private Door _door;

    private const string ActivationBoolName = "isActive";

    private Coroutine _increaseVolume;
    private Coroutine _decreaseVolume;
    private AudioSource _audioSource;
    private float _volumeChangePerFrame = 0.0005f;

    private void Awake()
    {
        float initialVolume = 0f;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = initialVolume;
    }

    private void OnEnable()
    {
        _door.OnEntered += ChangeVolume;
    }

    private void OnDisable()
    {
        _door.OnEntered -= ChangeVolume;
    }

    public void ChangeVolume(bool isThiefInside) 
    {               
        if (isThiefInside)
        {
            IncreaseVolume();
        }
        else 
        {
            DecreaseVolume();
        }
    }

    private void StopRunningCoroutine(Coroutine coroutineToStop) 
    {
        if (coroutineToStop != null)
            StopCoroutine(coroutineToStop);
    }

    private void IncreaseVolume()
    {
        float maxVolume = 1f;

        StopRunningCoroutine(_decreaseVolume);
        GetComponent<Animator>().SetBool(ActivationBoolName, true);

        if (_audioSource.isPlaying == false) 
            _audioSource.Play();

        _increaseVolume = StartCoroutine(MoveVolume(maxVolume));
    }

    private void DecreaseVolume()
    {
        float minVolume = 0f;

        StopRunningCoroutine(_increaseVolume);
        GetComponent<Animator>().SetBool(ActivationBoolName, false);
        _decreaseVolume = StartCoroutine(MoveVolume(minVolume));

        if (_audioSource.volume == minVolume)
            _audioSource.Stop();
    }

    private IEnumerator MoveVolume(float targetVolume) 
    {
        while (_audioSource.volume != targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, _volumeChangePerFrame);
            yield return null;
        }
    }
}