using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerController : CharacterBody2D, IPlayerControlsListener
{
	[Export] private PlayerInputs _playerInputs;

	private Vector2 _inputDir;
	private Vector2 _velocity;
	
	[Export] float _moveSpeed = 300f;
	private bool _isMoving = false;

	[Export] private int _tileSize = 32;

	public override void _Ready()
	{
		_playerInputs.Movement += Move;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_inputDir != Vector2.Zero && !_isMoving)
		{
			MoveToNextTile();
		}
	}

	public override void _ExitTree()
	{
		_playerInputs.Movement -= Move;
	}

	public void Move(Vector2 movement)
	{
		_inputDir = movement;
		
		if (_isMoving) { return;}
		
		if (!_isMoving)
		{
			if (Mathf.Abs(movement.X) > Mathf.Abs(movement.Y))
			{
				_inputDir = new(Mathf.Sign(movement.X), 0);
			}
			else
			{
				_inputDir = new(0, Mathf.Sign(movement.Y));
			}
		}
	}

	public void Interact() 
	{
		GD.Print("Interacting.");
	}

	public void Attack() 
	{
		GD.Print("Attacking.");
	}

	async void MoveToNextTile()
	{
		_isMoving = true;
		int delayTime = 1;

		Vector2 startingPos = Position;
		Vector2 targetPos = startingPos + _inputDir * _tileSize;

		while (Position.DistanceTo(targetPos) > 1f)
		{
			Position = Position.MoveToward(targetPos, _moveSpeed * (float)GetProcessDeltaTime());
			await Task.Delay(delayTime);
		}

		Position = targetPos;
		_isMoving = false;
	}
}
