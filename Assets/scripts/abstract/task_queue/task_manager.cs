using System.Collections.Generic;

public class task_manager< T > where T : struct, System.IConvertible
{
	T default_queue = default( T );
	Dictionary<T, task_queue> task_queues;

	public task_manager()
	{
		if ( !typeof( T ).IsEnum )
		{
			throw new System.ArgumentException( "T must be an enumerated type" );
		}

		this.task_queues = new Dictionary<T, task_queue>();

		foreach ( var t in helper.iterate_enum<T>() )
		{
			this.task_queues.Add( t, new task_queue() );
		}
	}

	public void push_back( params task[] tasks )
	{
		this.task_queues[ this.default_queue ].push_back( tasks );
	}

	public void push_back( T queue, params task[] tasks )
	{
		this.task_queues[ queue ].push_back( tasks );
	}

	public void push_front( params task[] tasks )
	{
		this.task_queues[ this.default_queue ].push_front( tasks );
	}

	public void push_front( T queue, params task[] tasks )
	{
		this.task_queues[ queue ].push_front( tasks );
	}

	public void insert_at( int index, params task[] tasks )
	{
		this.task_queues[ this.default_queue ].insert_at( index, tasks );
	}

	public void insert_at( T queue, int index, params task[] tasks )
	{
		this.task_queues[ queue ].insert_at( index, tasks );
	}

	public void remove_at( int index )
	{
		this.task_queues[ this.default_queue ].remove_at( index );
	}

	public void remove_at( T queue, int index )
	{
		this.task_queues[ queue ].remove_at( index );
	}

	public void clear()
	{
		this.task_queues[ this.default_queue ].clear();
	}

	public void clear(T queue)
	{
		this.task_queues[ queue ].clear();
	}

	public void clear_all()
	{
		foreach ( KeyValuePair<T, task_queue> p in this.task_queues )
		{
			p.Value.clear();
		}
	}

	public void update()
	{
		foreach ( KeyValuePair<T, task_queue> p in this.task_queues )
		{
			p.Value.update();
		}
	}
}
