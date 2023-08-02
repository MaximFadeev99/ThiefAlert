using UnityEngine;

public class Door : MonoBehaviour
{
    private Thief _thief;
    private Alarm _alarm;
    private bool _isDoorOpen;
    private bool _isThiefInside;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        _alarm = FindAnyObjectByType<Alarm>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _thief)) 
        {
            GetComponent<SpriteRenderer>().enabled = true;
            _isDoorOpen = true;
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        _isDoorOpen = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && _isDoorOpen)
        {
            _thief.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            _isThiefInside = true;
            _alarm.ChangeVolume(_isThiefInside);
        }

        if (Input.anyKey && _isThiefInside)
        {
            _thief.GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            _isThiefInside = false;
            _alarm.ChangeVolume(_isThiefInside);
        }
    }
}
