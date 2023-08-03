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
            StartReaction(true);
        }

        if (Input.anyKey && _isThiefInside)
        {
            StartReaction(false);
        }
    }

    private void StartReaction(bool hasEnteredHouse) 
    {
        _thief.GetComponent<SpriteRenderer>().enabled = !hasEnteredHouse;
        GetComponent<SpriteRenderer>().enabled = !hasEnteredHouse;
        _isThiefInside = hasEnteredHouse;
        _alarm.ChangeVolume(_isThiefInside);
    }
}
