using UnityEngine;

public class GameObjectCharacterController : MonoBehaviour
{
    private CharacterController _characterController;

    private string _entityId = "SangoTestCapsule001";

    private float _movespeed = 2f;

    private float _vertical;
    private float _horizontal;
    private Vector3 _moveDirection;

    private float _timeCounter = 0;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        _vertical = Input.GetAxis("Vertical");
        _horizontal = Input.GetAxis("Horizontal");
        _moveDirection = new Vector3(_horizontal, 0f, _vertical);

        _moveDirection = transform.TransformDirection(_moveDirection) * _movespeed;

        _characterController.Move(_moveDirection * _movespeed * Time.deltaTime);

    }

    private void Update()
    {
        if (_timeCounter > 0)
        {
            _timeCounter -= Time.deltaTime;
        }
        else
        {
            AOISystem.Instance.AddAOIActiveMoveEntity(_entityId, transform);
            AOISystem.Instance.SendAOIReqMessage();
            _timeCounter = 0.2f;
        }
    }
}
