using UnityEngine;
using Unity.Cinemachine;

public class MyPlayerCamera : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private GameObject _player;
    private CharacterController _controller;

    [Header("camera")]
    [Tooltip("揺れの大きさ")]
    public float amplitudeGainRatio = 1.0f;
    [Tooltip("揺れの速さ")]
    public float frequencyGainRatio = 1.0f;
    private float originalAmplitudeGain;
    private float originalFrequencyGain;


    [Header("light")]
    private Light handyLight;
    [Tooltip("スマホライトのON/OFF")]
    public bool OnHandyLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
        _player = GameObject.FindWithTag("Player");
        _controller = _player.GetComponent<CharacterController>();
        originalAmplitudeGain = _cinemachineBasicMultiChannelPerlin.AmplitudeGain;
        originalFrequencyGain = _cinemachineBasicMultiChannelPerlin.FrequencyGain;

        handyLight = transform.Find("HandyLight").GetComponent<Light>();
    }

    void Update()
    {
        OnWalkUpdate();
        OnHandyLightUpdate();
    }

    // 移動速度に応じてカメラを揺らす
    private void OnWalkUpdate()
    {
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = originalAmplitudeGain + (currentHorizontalSpeed * amplitudeGainRatio);
        _cinemachineBasicMultiChannelPerlin.FrequencyGain = originalFrequencyGain + (currentHorizontalSpeed * frequencyGainRatio); ;
    }

    // スマホのライト
    private void OnHandyLightUpdate()
    {
        if (OnHandyLight)
        {
            handyLight.intensity = 20.0f;
        }
        else
        {
            handyLight.intensity = 0.0f;
        }

    }
}
