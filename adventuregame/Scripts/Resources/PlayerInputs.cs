using Godot;
using System;

[GlobalClass]
public partial class PlayerInputs : Resource
{
	public event Action<Vector2> Movement;
	public event Action Attack, Interact;
	
	public void HandleMovement(Vector2 movement) => Movement?.Invoke(movement);
	public void HandleAttack() => Attack?.Invoke();
	public void HandleInteract() => Interact?.Invoke();
}
