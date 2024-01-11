using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : BaseObjectController
{
    private CharacterController _characterController;
    private PlayerEntity _playerEntity;

    public string EntityID { get; set; } = "";

    private float _movespeed = 2f;

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

    private void Start()
    {
        if (IsCurrent)
        {
            _characterController = transform.AddComponent<CharacterController>();
        }
    }

    private void Update()
    {
        UpdateMoveKey();
    }

    private Vector2 lastKeyDir = Vector2.zero;
    private void UpdateMoveKey()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 keyDir = new Vector2(h, v);
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
        if (!dir.Equals(lastStickDir))
        {
            Vector3 dirVector3 = new Vector3(dir.x, 0, dir.y);
            dirVector3 = Quaternion.Euler(0, 45, 0) * dirVector3;
            TransformData logicDir = new();
            if (dir != Vector2.zero)
            {
                logicDir.Position = new(dirVector3.x, dirVector3.y, dirVector3.z);
            }
            _playerEntity.SendMoveKey(logicDir);
            lastStickDir = dir;
        }
    }
}
