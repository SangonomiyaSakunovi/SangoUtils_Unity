using UnityEngine;

public abstract class BaseObjectBehaviour : MonoBehaviour
{
    protected Transform MoveTargetPosition { get; set; }
    
    protected virtual void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {

    }

    private void UpdateRotation()
    {

    }

    private void UpdateScale()
    {

    }
}
