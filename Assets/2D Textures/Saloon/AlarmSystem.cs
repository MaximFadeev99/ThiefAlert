using System.Collections;
using UnityEngine;

public class AlarmControl : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Alarm _alarm;
    private AudioSource _alarmSound;
    private Thief _thief;
    private Coroutine _increaseCoroutine;
    private Coroutine _decreaseCoroutine;
    private bool _isDoorOpen = false;
    private bool _isThiefInside = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _alarm = FindObjectOfType<Alarm>();
        _alarmSound = _alarm.GetComponent<AudioSource>();
        _alarmSound.volume = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Thief>(out _thief))
        {
            _spriteRenderer.enabled = true;
            _isDoorOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _spriteRenderer.enabled = false;      
        _isDoorOpen = false;       
    }

    private void Update()
    {      
        if (Input.GetKeyUp(KeyCode.E) == true && _isDoorOpen == true)
        {
            _alarm.GetComponent<Animator>().SetBool("isActive", true);
            _thief.GetComponent<SpriteRenderer>().enabled = false;
            _spriteRenderer.enabled = false;
            _isThiefInside = true;
            
            if (_alarmSound.isPlaying == false) 
            {
                _alarmSound.volume = 0.001f;
                _alarmSound.Play();
            }           
            
            if (_decreaseCoroutine != null) 
                StopCoroutine(_decreaseCoroutine);
            
            _increaseCoroutine = StartCoroutine(IncreaseVolume(_alarmSound.volume));
        }

        if (Input.anyKey == true && _isThiefInside == true)
        {
            _spriteRenderer.enabled = true;
            _thief.GetComponent<SpriteRenderer>().enabled = true;
            _isThiefInside = false;
            _alarm.GetComponent<Animator>().SetBool("isActive", false);
            StopCoroutine(_increaseCoroutine);
            _decreaseCoroutine = StartCoroutine(DecreaseVolume(_alarmSound.volume));
        }

        if (_alarmSound.volume == 0f)
            _alarmSound.Stop();
    }

    private IEnumerator IncreaseVolume(float _currentVolume) 
    {       
        while (_currentVolume < 1f) 
        {
            _alarmSound.volume = Mathf.MoveTowards(_currentVolume, 1f, 0.0005f);
            _currentVolume = _alarmSound.volume;
            yield return null;
        }        
    }

    private IEnumerator DecreaseVolume(float _currentVolume)
    {
        while (_currentVolume > 0f) 
        {
            _alarmSound.volume = Mathf.MoveTowards(_currentVolume, 0f, 0.0005f);
            _currentVolume = _alarmSound.volume;
            yield return null;
        }

        _alarmSound.volume = 0f;
    }
}
