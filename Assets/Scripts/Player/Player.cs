using Player.Interfaces;
using Player.PlayerAnimation;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField]
        private PlayerAnimation.PlayerAnimation _playerAnimation;
        
        [SerializeField]
        private SkeletonAnimation _skeletonAnimation;

        [SpineBone(dataField: "skeletonAnimation")]
        public string _targetBoneName;

        [SerializeField]
        private Transform _aimTargetRadiusPoint;
        
        private Bone _aimTargetBone;

        public Vector2 AimTargetRadiusPoint => _aimTargetRadiusPoint.position;
        public SkeletonAnimation SkeletonAnimation => _skeletonAnimation;
        public Transform Position => transform;

        public bool IsAiming { get; private set; }

        private void Awake()
        {
            _aimTargetBone = _skeletonAnimation.Skeleton.FindBone(_targetBoneName);
        }
        
        public void PlayIdleAnimation()
        {
            _playerAnimation.PlayIdleAnimation();
        }

        public void PlayMoveAnimation(float progressX, float moveSpeed)
        {
            _playerAnimation.PlayMoveAnimation(progressX, moveSpeed);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Flip(bool isLeft)
        {
            _playerAnimation.Flip(isLeft);
        }

        public void SetAimPosition(Vector2 aimPosition)
        {
            _aimTargetBone.SetLocalPosition(aimPosition);
        }
        
        public void SwitchAim(bool isPlayAim)
        {
            if (IsAiming != isPlayAim)
            {
                IsAiming = isPlayAim;
                
                if(IsAiming)
                    _playerAnimation.SetAiming(true);
                else
                    _playerAnimation.SetAiming(false);
            }
        }
    }
}