﻿using System.Collections;
using Cinemachine;
using ScoreSpace.Patterns;
using UnityEngine;

namespace ScoreSpace.Core
{
    public class CameraShakeCinemachine : MonoSingleton<CameraShakeCinemachine>
    {

        private float _shakeDuration = 0.3f; // Time the Camera Shake effect will last
        public float ShakeAmplitude = 1.2f; // Cinemachine Noise Profile Parameter
        public float ShakeFrequency = 2.0f; // Cinemachine Noise Profile Parameter

        private float _shakeElapsedTime = 0f;

        public float ShakeDuration
        {
            get => _shakeDuration;
            set
            {
                _shakeDuration = value;
                _shakeElapsedTime = _shakeDuration;
            }
        }

        // Cinemachine Shake
        public CinemachineVirtualCamera VirtualCamera;
        private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

        // Use this for initialization
        void Start()
        {
            // Get Virtual Camera Noise Profile
            if (VirtualCamera != null)
                virtualCameraNoise =
                    VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }

        // Update is called once per frame
        void Update()
        {
            //     if (Input.GetKey(KeyCode.S))
            // {
            //     ShakeElapsedTime = ShakeDuration;
            // }

            // If the Cinemachine componet is not set, avoid update
            if (VirtualCamera != null && virtualCameraNoise != null)
            {
                // If Camera Shake effect is still playing
                if (_shakeElapsedTime > 0)
                {
                    // Set Cinemachine Camera Noise parameters
                    virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                    virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                    // Update Shake Timer
                    _shakeElapsedTime -= Time.deltaTime;
                }
                else
                {
                    // If Camera Shake effect is over, reset variables
                    virtualCameraNoise.m_AmplitudeGain = 0f;
                    _shakeElapsedTime = 0f;
                }
            }
        }
    }
}