using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerController : CharacterBody2D, IPlayerControlsListener
{
	[Export] private PlayerInputs _playerInputs;
	
	#region Required Nodes
	[Export] private RayCast2D _collisionDetection;
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private AnimationTree _animationTree;
	[Export] private Area2D _hitBox;
	#endregion

	private Vector2 _inputDir;
	private Vector2 _velocity;
	private Vector2 _lastValidInput = Vector2.Zero;
	
	[Export] float _moveSpeed = 300f;
	private bool _isMoving = false;

	[Export] private int _tileSize = 32;

	public override void _Ready()
	{
		RegisterListeners();
	}

	public override void _Process(double delta)
	{
		if (Mathf.Abs(_inputDir.X) > 0 && Mathf.Abs(_inputDir.Y) > 0)
		{
			_inputDir = Vector2.Zero;
		}
		
		if (!_isMoving && _inputDir != Vector2.Zero)
		{
			MoveToNextTile();
		}
		
		MoveAndSlide();
	}

	public override void _ExitTree()
	{
		DeregisterListeners();
	}

	// Changes the player input to match the InputManager event firing.
	public void Move(Vector2 movement)
	{
		_inputDir = movement;
		
		if (_isMoving) { return;}
		
		_inputDir = new(Mathf.Sign(movement.X), Mathf.Sign(movement.Y));
	}

	// Handles Interaction Inputs, which so far does nothing.
	public void Interact() 
	{
		GD.Print("Interacting.");
	}

	// Handles the attack input.
	public void Attack() 
	{
		GD.Print("Attacking.");
	}

	/*
		Moves the player in a grid based on tile size and player input direction.
		If the player is next to something on the last input, player can no longer move in that direction.
	*/
	async void MoveToNextTile()
	{
		_collisionDetection.TargetPosition = _inputDir * _tileSize;
		_collisionDetection.ForceRaycastUpdate();

		if (_collisionDetection.IsColliding())
		{
			if (_collisionDetection.GetCollider() is not PlayerController)
			{
				return;
			}
		}
		
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

	// Registers the player inputs based on th eInputManager and PlayerInputs Resource
	void RegisterListeners()
	{
		_playerInputs.Movement += Move;
		_playerInputs.Interact += Interact;
		_playerInputs.Attack += Attack;
	}

	// Deregisters the events from RegisterListeners
	void DeregisterListeners()
	{
		_playerInputs.Movement -= Move;
		_playerInputs.Interact -= Interact;
		_playerInputs.Attack -= Attack;
	}
}
