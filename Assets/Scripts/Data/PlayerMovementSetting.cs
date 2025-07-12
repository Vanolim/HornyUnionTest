using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerMovementSetting", menuName = "Data/PlayerMovementSetting")]
    public class PlayerMovementSetting : ScriptableObject
    {
        public float MinTargetDistanceToMove = 0.1f;
        public float MinTargetDistanceToDeceleration = 0.2f;
        public float AccelerationFactor = 20f;
        public float DecelerationFactor = 25f;
        public float MaxSpeed = 8f;
        public float LeftMovementBorder = -8f;
        public float RightMovementBorder = 8f;
        public float WalkSpeedThreshold = 1f;
        public float WalkStepProgress = 6f;
        public float RunStepProgress = 6f;
    }
}