using UnityEngine;
using UnityEngine.Audio;

public partial class AudioManager
{
    [Header("Player")]
    [Tooltip("足音")]
    public AudioResource footStep;

    private GameObject _player;
    private CharacterController _controller;

    private float walkCount;

    [Tooltip("歩幅")]
    public float stepWidth;


    private void PlayerAudioInitialize()
    {
        _player = GameObject.FindWithTag("Player");
        _controller = _player.GetComponent<CharacterController>();
    }

    private void PlayerAudioUpdate()
    {
        OnWalkUpdate();
    }

    private void OnWalkUpdate()
    {
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        walkCount += currentHorizontalSpeed;
        if (walkCount > stepWidth)
        {
            sourcePlayerSE.Play(footStep);
            walkCount = 0.0f;
        }
    }
}
