using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player.PlayerAnimation
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation _skeletonAnimation;
        
        [SerializeField]
        private AnimationReferenceAsset _run, _idle, _aim, _walk;

        private PlayerAnimations _currentAnimation;

        private const float _moveAnimationMixDuration = 0.15f;

        public void Flip(bool isLeft)
        {
            if (_skeletonAnimation.Skeleton.FlipX != isLeft)
            {
                _skeletonAnimation.Skeleton.FlipX = isLeft;
            }
        }

        public void PlayIdleAnimation()
        {
            if (_currentAnimation != PlayerAnimations.Idle)
            {
                if (_currentAnimation == PlayerAnimations.Idle)
                    return;

                _skeletonAnimation.AnimationState.ClearTrack(0);
                _skeletonAnimation.skeleton.SetToSetupPose(); 
                _skeletonAnimation.timeScale = 1f;
                _skeletonAnimation.AnimationState.SetAnimation(0, _idle, true);
    
                _currentAnimation = PlayerAnimations.Idle;
            }
        }
        
        public void PlayMoveAnimation(float progressX, float moveSpeed)
        {
            if (_currentAnimation != PlayerAnimations.Moving)
            {
                _skeletonAnimation.AnimationState.ClearTrack(0);
                _currentAnimation = PlayerAnimations.Moving;
            }
            
            _skeletonAnimation.timeScale = 1f;

            AnimationReferenceAsset selectedAnim = moveSpeed < 0.5f ? _walk : _run;
            TrackEntry currentEntry = _skeletonAnimation.AnimationState.GetCurrent(0);
            
            if (currentEntry != null && currentEntry.Animation == selectedAnim.Animation)
            {
                float animDuration = currentEntry.Animation.Duration;
                currentEntry.TrackTime = progressX * animDuration;
                return;
            }
            
            TrackEntry newEntry = _skeletonAnimation.AnimationState.SetAnimation(0, selectedAnim, true);
            newEntry.TrackTime = progressX * newEntry.Animation.Duration;
            newEntry.MixDuration = _moveAnimationMixDuration;
        }
        
        public void SetAiming(bool isAiming)
        {
            if (isAiming)
            {
                TrackEntry aimTrack = _skeletonAnimation.AnimationState.SetAnimation(1, _aim, true);
                aimTrack.AttachmentThreshold = 1f;
                aimTrack.MixDuration = 0f;
            }
            else
            {
                _skeletonAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f);
            }
        }
    }
}