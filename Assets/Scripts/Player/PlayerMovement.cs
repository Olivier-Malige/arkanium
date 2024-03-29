﻿using System;
using UnityEngine;

namespace ScoreSpace.Player
{
    public enum Position
    {
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left,
        TopLeft,
        Top,
        TopRight,
    }
    public class PlayerMovement : MonoBehaviour
    {
        public Position CurrentPosition => _currentPosition;

        private Position _currentPosition = Position.Right;
        [SerializeField] private Transform _sprite;
        
        private Rigidbody2D _rb;
        private PlayerInput _playerInput;
        [SerializeField]
        private float _baseSpeed;
        [SerializeField] private float _slowSpeedFireHolding = 50;
        private float _speed;
        private static readonly int XAxis = Animator.StringToHash("xAxis");

        [SerializeField] private int _speedBonusMax = 200;
        private float _speedBonus = 0;


        public float SpeedBonus
        {
            get => _speedBonus;
            set => _speedBonus = Mathf.Clamp(value, 0, _speedBonusMax );
        }
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            _speed = _baseSpeed;
        }

        private void Update()
        {
            RotatePlayer();
            CheckHoldingFireAndSlowSpeed();
        }

        private void RotatePlayer()
        {
            var inputThreshold = 0.2;
            if (_playerInput.Horizontal < -inputThreshold && _playerInput.Vertical > -inputThreshold &&
                _playerInput.Vertical < inputThreshold)
            {
                _currentPosition = Position.Left;
                _sprite.rotation = Quaternion.Euler(0, 0, -180);
            }
            else if (_playerInput.Horizontal < -inputThreshold && _playerInput.Vertical < -inputThreshold)
            {
                _currentPosition = Position.BottomLeft;
                _sprite.rotation = Quaternion.Euler(0, 0, -135);
            }
            else if (_playerInput.Horizontal > inputThreshold && _playerInput.Vertical > -inputThreshold &&
                     _playerInput.Vertical < inputThreshold)
            {
                _currentPosition = Position.Right;
                _sprite.rotation = Quaternion.identity;
            }
            else if (_playerInput.Horizontal < -inputThreshold && _playerInput.Vertical > inputThreshold)
            {
                _currentPosition = Position.TopLeft;
                _sprite.rotation = Quaternion.Euler(0, 0, 135);
            }
            else if (_playerInput.Horizontal > inputThreshold && _playerInput.Vertical > inputThreshold)
            {
                _currentPosition = Position.TopRight;
                _sprite.rotation = Quaternion.Euler(0, 0, 45);
            }
            else if (_playerInput.Horizontal > inputThreshold && _playerInput.Vertical < -inputThreshold)
            {
                _currentPosition = Position.BottomRight;
                _sprite.rotation = Quaternion.Euler(0, 0, -45);
            }
            else if (_playerInput.Vertical < -inputThreshold && _playerInput.Horizontal > -inputThreshold &&
                     _playerInput.Horizontal < inputThreshold)
            {
                _currentPosition = Position.Bottom;
                _sprite.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (_playerInput.Vertical > inputThreshold && _playerInput.Horizontal > -inputThreshold &&
                     _playerInput.Horizontal < inputThreshold)
            {
                _currentPosition = Position.Top;
                _sprite.rotation = Quaternion.Euler(0, 0, 90);
            }
        }

        private void FixedUpdate()
        {
            if (_playerInput.Horizontal != 0 && _playerInput.Vertical != 0)
            {
                _rb.velocity = new Vector2(_playerInput.Horizontal * _speed/ 1.15f  * Time.fixedDeltaTime, _playerInput.Vertical * _speed/ 1.15f * Time.fixedDeltaTime);
            }
            else
            {
                _rb.velocity = new Vector2(_playerInput.Horizontal * _speed * Time.fixedDeltaTime, _playerInput.Vertical* _speed * Time.fixedDeltaTime);
            }
            
          
        }
        
        private void CheckHoldingFireAndSlowSpeed()
        {
            if (_playerInput.IsFireHolding)
            {
                _speed = _baseSpeed - _slowSpeedFireHolding + SpeedBonus;
            }
            else
            {
                _speed = _baseSpeed + SpeedBonus;
            }
        }
    }
}
