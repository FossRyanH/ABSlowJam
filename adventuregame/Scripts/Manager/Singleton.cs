using Godot;
using System;

public partial class Singleton<T> : Node where T : Node
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				GD.PrintErr($"{typeof(T).Name} instance is null");
			}
			return _instance;
		}
	}

	public override void _EnterTree()
	{
		if (_instance == null)
		{
			_instance = this as T;
			Initialize();
		}
		else
		{
			GD.PrintErr($"{typeof(T).Name} istance already exists, deleting duplicate.");
			QueueFree();
		}
	}

	public override void _ExitTree()
	{
		if (_instance == this)
		{
			_instance = null;
		}
	}
	
	protected virtual void Initialize() {}
}
