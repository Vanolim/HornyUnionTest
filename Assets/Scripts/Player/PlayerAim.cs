using Data;
using Player.Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerAim
    {
        private readonly PlayerAimData _playerAimData;
        private readonly Camera _camera;
        private readonly IPlayer _player;
        
        public PlayerAim(PlayerAimData playerAimData, Camera camera, IPlayer player)
        {
            _playerAimData = playerAimData;
            _camera = camera;
            _player = player;
        }

        public void Tick(float dt) 
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                var playerSkeletonAnimation = _player.SkeletonAnimation;
                Vector3 worldMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                
                Vector3 targetPosition;

                if (worldMousePosition.y < _player.AimTargetRadiusPoint.y + _playerAimData.AddPositionYToApplyAimFactor)
                {
                    Vector2 targetDirection = worldMousePosition - (Vector3)_player.AimTargetRadiusPoint;
                    float angleToCursor = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

                    if (IsPlayerFlip())
                    {
                        angleToCursor = 180f - angleToCursor;
                    }
                    
                    bool isBehind = (IsPlayerFlip() && worldMousePosition.x > _player.Position.position.x)
                                    || (!IsPlayerFlip() && worldMousePosition.x < _player.Position.position.x);

                    float modifiedAngle;

                    if (isBehind)
                    {
                        modifiedAngle = angleToCursor + _playerAimData.AimAngleOffsetBehindPlayer;
                    }
                    else
                    {
                        modifiedAngle = angleToCursor + _playerAimData.AimAngleOffset;
                    }

                    float rad = modifiedAngle * Mathf.Deg2Rad;
                    float radius = _playerAimData.AimRadius;

                    Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

                    if (IsPlayerFlip())
                    {
                        direction.x *= -1;
                    }

                    targetPosition = _player.AimTargetRadiusPoint + direction * radius;
                }
                else
                {
                    targetPosition = worldMousePosition;
                }

                Vector3 skeletonSpacePoint = playerSkeletonAnimation.transform.InverseTransformPoint(targetPosition);
                skeletonSpacePoint.x *= playerSkeletonAnimation.Skeleton.ScaleX;
                skeletonSpacePoint.y *= playerSkeletonAnimation.Skeleton.ScaleY;
                _player.SetAimPosition(skeletonSpacePoint);
                
                _player.SwitchAim(true);
            }
            else
            {
                _player.SwitchAim(false);
            }
        }

        private bool IsPlayerFlip() => _player.SkeletonAnimation.Skeleton.FlipX;
    }
}