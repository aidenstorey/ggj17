using System.Collections.Generic;

public class task_queue
{
	task current_task = null;
	List<task> tasks;

	public task_queue()
	{
		this.tasks = new List<task>();
	}

	public void push_back( params task[] tasks )
	{
		this.tasks.AddRange( tasks );
	}

	public void push_front( params task[] tasks )
	{
		this.tasks.InsertRange( 0, tasks );
	}

	public void insert_at( int index, params task[] tasks )
	{
		this.tasks.InsertRange( index, tasks );
	}

	public void remove_at( int index )
	{
		this.tasks.RemoveAt( index );
	}

	public void clear()
	{
		this.current_task = null;
		this.tasks.Clear();
	}

	public void update()
	{
		if ( this.current_task == null )
		{
			if ( this.tasks.Count > 0 )
			{
				this.current_task = this.tasks[ 0 ];
				this.tasks.RemoveAt( 0 );

				this.current_task.on_start();
			}
			else
			{
				return;
			}
		}

		this.current_task.update();

		if ( this.current_task != null && this.current_task.is_completed )
		{
			this.current_task = null;
		}
	}
}
