using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]

public class Alarm : MonoBehaviour 
{
    [SerializeField] private Door _door;

    private readonly int IsActive = Animator.StringToHash(nameof(IsActive));

    private Coroutine _increaseVolume;
    private Coroutine _decreaseVolume;
    private AudioSource _audioSource;
    private Animator _animator;
    private float _volumeChangePerFrame = 0.0005f;

    private void Awake()
    {
        float initialVolume = 0f;

        _animator = GetComponent<Animator>();
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
        _animator.SetBool(IsActive, true);

        if (_audioSource.isPlaying == false) 
            _audioSource.Play();

        _increaseVolume = StartCoroutine(MoveVolume(maxVolume));
    }

    private void DecreaseVolume()
    {
        float minVolume = 0f;

        StopRunningCoroutine(_increaseVolume);
        _animator.SetBool(IsActive, false);
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