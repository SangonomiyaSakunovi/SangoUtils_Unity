using SangoUtils_Unity_App.Entity;
using SangoUtils_FixedNum;
using UnityEngine;
using SangoUtils_Bases_UnityEngine;
using SangoUtils_Logger;

namespace SangoUtils_Unity_App.Controller
{
    public class PlayerController : BaseObjectController
    {
        private CharacterController _characterController;
        private PlayerEntity _playerEntity = null;

        public string EntityID { get; set; } = "";

        //private float _movespeed = 2f;

        private float _vertical;
        private float _horizontal;
        private Vector3 _moveDirection;

        private Vector3 _positionLast;
        private Quaternion _rotationLast;
        private Vector3 _scaleLast;

        public Vector3 PositionTarget { get; set; } 

        public bool IsCurrent { get; set; } = false;
        public bool IsLerp { get; set; } = false;

        public void SetPlayerEntity(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
        }

        private void Update()
        {
            if (_playerEntity != null)
            {
                if (_playerEntity.LogicPosition != _playerEntity.LogicPositionLast)
                {
                    transform.position = Vector3.Lerp(transform.position, _playerEntity.LogicPosition.ConvertToVector3(), Time.deltaTime * 1);
                }
            }
        }

        private void FixedUpdate()
        {
            if (transform.hasChanged)
            {
                SangoLogger.Done("Transform has changed!");
                transform.hasChanged = false;
            }
            if (_playerEntity != null)
            {
                UpdateMoveKey();
                _playerEntity.Update();
            }
        }

        private Vector2 lastKeyDir = Vector2.zero;
        private void UpdateMoveKey()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector2 keyDir = new(h, v);
            if (keyDir != lastKeyDir)
            {
                if (h != 0 || v != 0)
                {
                    keyDir = keyDir.normalized;
                }
                InputMoveKey(keyDir);
                lastKeyDir = keyDir;
            }
        }

        private Vector2 lastStickDir = Vector2.zero;
        private void InputMoveKey(Vector2 dir)
        {
            if (EntityID == CacheService.Instance.EntityCache.PlayerEntity_This.EntityID)
            {
                if (!dir.Equals(lastStickDir))
                {

                    Vector3 dirVector3 = new(dir.x, 0, dir.y);
                    //dirVector3 = Quaternion.Euler(0, 45, 0) * dirVector3;
                    FixedVector3 logicDirection = FixedVector3.Zero;
                    if (dir != Vector2.zero)
                    {
                        logicDirection.X = (FixedInt)dirVector3.x;
                        logicDirection.Y = (FixedInt)dirVector3.y;
                        logicDirection.Z = (FixedInt)dirVector3.z;
                    }
                    //_playerEntity.SendMoveKey(logicDirection);

                    _playerEntity.CalcMoveResult(logicDirection);
                    lastStickDir = dir;
                }
            }

        }
    }
}