using System;
using System.Collections.Generic;

public class current_queue<T>
{
	List<T> queue = new List<T>();

	public void add( T _t )
	{
		this.queue.Add( _t );
	}

	public void remove( T _t )
	{
		this.queue.Remove( _t );
	}

	public T current
	{
		get
		{
			return this.queue.Count > 0 ? this.queue[ 0 ] : default( T );
		}
	}
}
