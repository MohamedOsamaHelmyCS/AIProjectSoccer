using UnityEngine;
public class HumanPlayer1Input : I_PlayerInput
{
    public Vector3 MovementInput()
    {
        Vector3 moveInput = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            moveInput.x = -1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput.x = 1.0f;
        }
        return moveInput;
    }
    public bool KickInput()
    {
        return Input.GetKeyDown(KeyCode.S);
    }

    public bool JumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
public class HumanPlayer2Input : I_PlayerInput
{
    public Vector3 MovementInput()
    {
        Vector3 moveInput = Vector3.zero;
        Debug.Log(Input.inputString);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput.x = -1.0f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput.x = 1.0f;
        }
        return moveInput;
    }

    public bool JumpInput()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public bool KickInput()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }
}