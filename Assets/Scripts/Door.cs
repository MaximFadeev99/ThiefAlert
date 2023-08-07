using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Door : MonoBehaviour
{
    public Action<bool> OnEntered;

    private SpriteRenderer _spriteRenderer;
    private Thief _thief;
    private bool _isDoorOpen;
    private bool _isThiefInside;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _thief)) 
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
        if (Input.GetKeyUp(KeyCode.E) && _isDoorOpen)
        {
            RegisterBreakIn(true);
        }

        if (Input.anyKey && _isThiefInside)
        {
            RegisterBreakIn(false);
        }
    }

    private void RegisterBreakIn(bool hasEnteredHouse) 
    {
        _thief.GetComponent<SpriteRenderer>().enabled = !hasEnteredHouse;
        _spriteRenderer.enabled = !hasEnteredHouse;
        _isThiefInside = hasEnteredHouse;
        OnEntered.Invoke(_isThiefInside);
    }
}

