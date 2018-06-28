using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public int id;

    void FixedUpdate ()
    {
        var movementState = GameController.instance.movementController.GetState(id);
        transform.position = new Vector3(movementState.x, movementState.y);
        GameController.instance.movementController.RequestMove(id, MovementAction.Forward);
    }
}
