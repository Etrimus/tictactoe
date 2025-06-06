﻿using System.Collections;

namespace TicTacToe.Core;

public class ReadOnlyTwoDimensionalCollection<T>: ICollection<T>, ICollection
{
    private readonly T[,] _array;
    private readonly IEnumerable<T> _collection;

    public ReadOnlyTwoDimensionalCollection(T[,] source)
    {
        _array = source;
        _collection = _array.Cast<T>();
    }

    public T this[int i, int y] => _array[i, y];

    public bool IsReadOnly => true;

    int ICollection.Count => _array.Length;

    int ICollection<T>.Count => _array.Length;

    public bool IsSynchronized => _array.IsSynchronized;

    public object SyncRoot => _array.SyncRoot;

    #region ICollection

    public void CopyTo(Array array, int index)
    {
        _array.CopyTo(array, index);
    }

    #endregion

    #region ICollection<T>

    public bool Contains(T item)
    {
        return _collection.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _array.CopyTo(array, arrayIndex);
    }

    public void Add(T item)
    {
        throw new NotSupportedException();
    }

    public void Clear()
    {
        throw new NotSupportedException();
    }

    public bool Remove(T item)
    {
        throw new NotSupportedException();
    }

    #endregion

    #region IEnumerable

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region IEnumerable<T>

    public IEnumerator<T> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    #endregion

    public int GetLength(int dimension)
    {
        return _array.GetLength(dimension);
    }
}