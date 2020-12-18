using UnityEngine;
using System.Collections.Generic;

public sealed class ObjectPool<T> 
{
	public delegate T Generator();

	private Queue<T> _pool;
	private Generator _generator;

	private ObjectPool() {}
	public ObjectPool(Generator generator)
	{
		_pool = new Queue<T>();
		_generator = generator;
	}

	public T Dequeue()
	{
		T item;

		if (_pool.Count > 0) {
			item = _pool.Dequeue();
		} else {
			item = _generator.Invoke();
		}

		return item;
	}

	public void Enqueue(T item)
	{
		_pool.Enqueue(item);
	}

	public T[] ToArray()
	{
		return _pool.ToArray();
	}

}
