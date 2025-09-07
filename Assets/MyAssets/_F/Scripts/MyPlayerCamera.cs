using UnityEngine;
using Unity.Cinemachine;

public class MyPlayerCamera : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private GameObject _player;
    private CharacterController _controller;

    public float amplitudeGainRatio = 1.0f;
    public float frequencyGainRatio = 1.0f;
    private float originalAmplitudeGain;
    private float originalFrequencyGain;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
        _player = GameObject.FindWithTag("Player");
        _controller = _player.GetComponent<CharacterController>();
        originalAmplitudeGain = _cinemachineBasicMultiChannelPerlin.AmplitudeGain;
        originalFrequencyGain = _cinemachineBasicMultiChannelPerlin.FrequencyGain;
    }

    void Update()
    {
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = originalAmplitudeGain + (currentHorizontalSpeed * amplitudeGainRatio);
        _cinemachineBasicMultiChannelPerlin.FrequencyGain = originalFrequencyGain + (currentHorizontalSpeed * frequencyGainRatio);;
    }
}
