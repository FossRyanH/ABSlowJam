using Godot;
using System;

public partial class InputManager : Singleton<InputManager>
{
    #region Input Resources
    [Export] private PlayerInputs _playerInputs;
    #endregion
    
    // Vector for input
    private Vector2 _inputVector;

    public override void _Input(InputEvent @event)
    {
        _inputVector = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
        
        if (_inputVector != Vector2.Zero)
        {
            _playerInputs.HandleMovement(_inputVector);
        }
        else
        {
            _playerInputs.HandleMovement(_inputVector);
        }

        if (@event.IsActionPressed("Interact"))
        {
            _playerInputs.HandleInteract();
        }
        else if (@event.IsActionPressed("Attack"))
        {
            _playerInputs.HandleAttack();
        }
    }
}
