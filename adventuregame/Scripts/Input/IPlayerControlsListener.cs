using Godot;
using System;

public interface IPlayerControlsListener
{
	void Move(Vector2 movement);
	void Interact();
	void Attack();
}
