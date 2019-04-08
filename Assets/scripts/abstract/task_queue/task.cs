using UnityEngine;

public class task
{
	protected bool completed = false;

	public bool is_completed
	{
		get { return this.completed; }
	}

	public virtual void on_start()
	{
	}

	protected void mark_complete()
	{
		this.completed = true;
	}

	public virtual void update()
	{
		this.mark_complete();
	}
}

public class task_one_off : task
{
	public task_one_off()
	{
		this.mark_complete();
	}
}
public class wait : task
{
	float duration;

	public wait( float duration )
	{
		this.duration = duration;
	}

	public override void update()
	{
		this.duration -= Time.deltaTime;

		if ( this.duration <= 0.0f )
		{
			this.completed = true;
		}
	}
}

public class wait_for_task : task
{
	task task;

	public wait_for_task( task task )
	{
		this.task = task;
	}

	public override void update()
	{
		if ( this.task.is_completed )
		{
			this.completed = true;
		}
	}
}

public class tween : task
{
	GameObject game_object;

	Vector3 position_start;
	Vector3 position_delta;

	float duration;
	float time_elapsed;

	public tween( GameObject game_object, Vector3 position_start, Vector3 position_end, float duration )
	{
		this.game_object = game_object;

		this.position_start = position_start;
		this.position_delta = ( position_end - position_start ) / duration;

		this.duration = duration;
		this.time_elapsed = 0.0f;
	}

	public tween( GameObject game_object, Vector3 position_offset, float duration ) :
		this( game_object, game_object.transform.position, game_object.transform.position + position_offset, duration )
	{
		// ... 
	}

	public override void update()
	{
		this.time_elapsed = Mathf.Min( this.duration, this.time_elapsed + Time.deltaTime );

		this.game_object.transform.position = position_start + position_delta * this.time_elapsed;

		if ( this.time_elapsed == this.duration )
		{
			this.completed = true;
		}
	}
}
