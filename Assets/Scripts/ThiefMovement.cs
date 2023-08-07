using UnityEngine;

[RequireComponent (typeof(Animator))]

public class ThiefMovement : MonoBehaviour
{
    private readonly int Speed = Animator.StringToHash(nameof(Speed));

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool isPressed = Input.anyKey;
        float coveredDistanceWalk = 2;
        float coveredDistanceIdle = 0;
        float xPositionChange = coveredDistanceWalk * Time.deltaTime;

        switch (isPressed)
        {
            case true:

                if (Input.GetKey(KeyCode.D))
                {
                    _animator.SetFloat(Speed, coveredDistanceWalk);
                    transform.Translate(xPositionChange, 0, 0);
                    break;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    _animator.SetFloat(Speed, coveredDistanceWalk);
                    transform.Translate(-xPositionChange, 0, 0);
                    break;
                }

                break;

            case false:
                _animator.SetFloat(Speed, coveredDistanceIdle);
                break;
        }
    }
}
