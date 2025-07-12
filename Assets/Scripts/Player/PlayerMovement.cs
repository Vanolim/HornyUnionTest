using Data;
using Player.Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerMovement
    {
        private readonly IPlayer _player;
        private readonly PlayerMovementSetting _playerMovementSetting;
        private readonly Camera _camera;
        
        private MoveState _currentMoveState;
        private float _playerSpeed;
        private float _moveDirection;

        public PlayerMovement(IPlayer player, PlayerMovementSetting playerMovementSetting, Camera camera)
        {
            _player = player;
            _playerMovementSetting = playerMovementSetting;
            _camera = camera;
            
            SwitchToIdle();
        }

        public void Tick(float dt)
        {
            switch (_currentMoveState)
            {
                case MoveState.Idle:
                    TryTransitToMoveState();
                    break;
                case MoveState.Moving:
                    if(TryTransitToIdle())
                        break;
                    
                    Move(dt);
                    break;
            }
        }
        
        private void SwitchToIdle()
        {
            _player.PlayIdleAnimation();
            _currentMoveState = MoveState.Idle;
        }

        private void TransitToMoving()
        {
            _currentMoveState = MoveState.Moving;
        }

        private void Move(float dt)
        {
            float distanceX = GetDistanceToCursor();
            _moveDirection = Mathf.Sign(distanceX);
            float absDistanceX = Mathf.Abs(distanceX);

            float slowDownDistance =_playerMovementSetting.MinTargetDistanceToDeceleration;
            float maxSpeed = _playerMovementSetting.MaxSpeed;
            float acceleration = _playerMovementSetting.AccelerationFactor;
            float decelerationPower = _playerMovementSetting.DecelerationFactor;

            float targetSpeed = 0f;
            if (absDistanceX > 0.01f)
            {
                if (absDistanceX > slowDownDistance)
                {
                    targetSpeed = _moveDirection * maxSpeed;
                }
                else
                {
                    float t = absDistanceX / slowDownDistance;
                    float slowedSpeed = maxSpeed * Mathf.Pow(t, decelerationPower);
                    targetSpeed = _moveDirection * slowedSpeed;
                }
            }

            if (Mathf.Sign(_playerSpeed) != Mathf.Sign(distanceX) && Mathf.Abs(_playerSpeed) > 0.01f)
            {
                _playerSpeed = 0f;
            }

            _playerSpeed = Mathf.MoveTowards(_playerSpeed, targetSpeed, acceleration * dt);

            Vector3 newPosition = _player.Position.position + new Vector3(_playerSpeed * dt, 0f, 0f);
            IsMovementPossible(ref newPosition);
            
            _player.SetPosition(newPosition);

            float moveSpeed = Mathf.Abs(_playerSpeed);
            float speedFactor = moveSpeed / maxSpeed;
            float animProgress = GetAnimationProgress(speedFactor);
            
            _player.PlayMoveAnimation(animProgress, speedFactor);

            TryFlip();
        }

        private void TryTransitToMoveState()
        {
            float minDistanceToCursorToMove = _playerMovementSetting.MinTargetDistanceToMove;

            float distance = Mathf.Abs(GetDistanceToCursor());
            
            if (distance > minDistanceToCursorToMove)
            {
                TransitToMoving();
            }
        }

        private bool TryTransitToIdle()
        {
            float minDistanceToCursorToMove = _playerMovementSetting.MinTargetDistanceToMove;

            if (Mathf.Abs(GetDistanceToCursor()) <= minDistanceToCursorToMove)
            {
                SwitchToIdle();
                return true;
            }

            return false;
        }

        private float GetDistanceToCursor()
        {
            Vector2 cursorWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            float distance = cursorWorldPos.x - _player.Position.position.x;
            return distance;
        }

        private void IsMovementPossible(ref Vector3 newPosition)
        {
            float leftBorder = _playerMovementSetting.LeftMovementBorder;
            float rightBorder = _playerMovementSetting.RightMovementBorder;
            
            newPosition.x = Mathf.Clamp(newPosition.x, leftBorder, rightBorder);
        }
        
        private float GetAnimationProgress(float speedFactor)
        {
            float playerPositionX = _player.Position.position.x;
            float stepProgress = speedFactor <= 0.5f ? _playerMovementSetting.WalkStepProgress : 
                _playerMovementSetting.RunStepProgress;
            
            float leftBorder = _playerMovementSetting.LeftMovementBorder;
            float rightBorder = _playerMovementSetting.RightMovementBorder;

            float animationProgress = Mathf.InverseLerp(leftBorder, rightBorder, playerPositionX);
            
            if (_moveDirection < 0)
            {
                animationProgress =  1 - animationProgress;
            }
            
            return (animationProgress * stepProgress) % 1f;
        }
        
        private void TryFlip()
        {
            if (_player.IsAiming == false)
            {
                if (_moveDirection > 0)
                {
                    _player.Flip(false);
                }
                else
                {
                    _player.Flip(true);
                }
            }
        }
        
        private enum MoveState
        {
            Idle,
            Moving
        }
    }
}