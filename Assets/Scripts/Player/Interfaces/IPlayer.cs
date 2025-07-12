using Spine.Unity;
using UnityEngine;

namespace Player.Interfaces
{
    public interface IPlayer
    {
        SkeletonAnimation SkeletonAnimation { get; }
        Transform Position { get; }
        bool IsAiming { get; }
        Vector2 AimTargetRadiusPoint { get; }
        
        void PlayIdleAnimation();
        void PlayMoveAnimation(float progressX = 0f, float moveSpeed = 0f);
        void SetPosition(Vector3 position);
        void Flip(bool isLeft);
        void SetAimPosition(Vector2 aimPosition);
        void SwitchAim(bool isPlayAim);
    }
}