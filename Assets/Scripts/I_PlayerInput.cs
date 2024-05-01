using UnityEngine;
public interface I_PlayerInput
{
    Vector3 MovementInput();
    bool JumpInput();
    bool KickInput();
}