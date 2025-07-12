using Data;
using Player;
using UnityEngine;

namespace Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private Player.Player _player;
        
        [SerializeField]
        private PlayerMovementSetting _playerMovementSetting;

        [SerializeField]
        private PlayerAimData _playerAimData;
        
        private PlayerAim _playerAim;
        private PlayerMovement _playerMovement;

        private void Start()
        {
            var camera = Camera.main;

            _playerAim = new PlayerAim(_playerAimData, camera, _player);
            _playerMovement = new PlayerMovement(_player, _playerMovementSetting, camera);
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            
            _playerAim.Tick(dt);
            _playerMovement.Tick(dt);
        }
    }
}