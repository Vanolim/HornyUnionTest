using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerAimData", menuName = "Data/PlayerAimData")]
    public class PlayerAimData : ScriptableObject
    {
        public float AddPositionYToApplyAimFactor;
        public float AimRadius = 1.5f;
        public float AimAngleOffset = 10f;
        public float AimAngleOffsetBehindPlayer = 10f;
    }
}