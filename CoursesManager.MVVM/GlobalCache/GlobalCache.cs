using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class GlobalCache
{
    private int _capacity;
    private int _permanentItemCount;
    private readonly ConcurrentDictionary<string, LinkedListNode<CacheItem>> _cacheMap;
    private readonly LinkedList<CacheItem> _usageOrder;
    private readonly object _lock = new object();

    private static readonly Lazy<GlobalCache> _instance = new Lazy<GlobalCache>(() => new GlobalCache(10));

    private GlobalCache(int capacity)
    {
        _capacity = capacity;
        _permanentItemCount = 0;
        _cacheMap = new ConcurrentDictionary<string, LinkedListNode<CacheItem>>();
        _usageOrder = new LinkedList<CacheItem>();
    }

    public static GlobalCache Instance => _instance.Value;

    public object Get(string key)
    {
        if (!_cacheMap.ContainsKey(key))
            throw new KeyNotFoundException();

        lock (_lock)
        {
            var node = _cacheMap[key];
            _usageOrder.Remove(node);
            _usageOrder.AddFirst(node);
            return node.Value.Value;
        }
    }

    public void Put(string key, object value, bool isPermanent)
    {
        lock (_lock)
        {
            if (_cacheMap.TryGetValue(key, out var existingNode))
            {
                existingNode.Value = new CacheItem(key, value, isPermanent);
                _usageOrder.Remove(existingNode);
                _usageOrder.AddFirst(existingNode);
            }
            else
            {
                // Check if cache is full and only full of permanent items
                if (_cacheMap.Count >= _capacity && _permanentItemCount == _capacity)
                {
                    IncreaseCapacity();
                }

                // Create and add the new node to the cache
                var newNode = new LinkedListNode<CacheItem>(new CacheItem(key, value, isPermanent));
                _usageOrder.AddFirst(newNode);
                _cacheMap[key] = newNode;

                // The permanent item count will be updated in the CacheItem class itself
            }
        }
    }

    private void IncreaseCapacity()
    {
        _capacity += 5; // Increase the capacity by 5
    }

    private void DecreaseCapacity()
    {
        if (_capacity > 10) // Do not allow capacity to go below 10
        {
            _capacity--;
        }
    }

    private void EvictNonPermanentItem()
    {
        var node = _usageOrder.Last;
        while (node != null)
        {
            if (!node.Value.IsPermanent)
            {
                _cacheMap.TryRemove(node.Value.Key, out _);
                _usageOrder.Remove(node);
                break;
            }
            node = node.Previous;
        }
    }

    public void RemovePermanentItem(string key)
    {
        lock (_lock)
        {
            if (_cacheMap.ContainsKey(key))
            {
                var node = _cacheMap[key];
                if (node.Value.IsPermanent)
                {
                    _permanentItemCount--; // Decrease permanent item count
                    _cacheMap.TryRemove(key, out _);
                    _usageOrder.Remove(node);

                    // Adjust capacity when permanent item is removed
                    DecreaseCapacity();
                }
            }
        }
    }

    private class CacheItem
    {
        public string Key { get; }
        public object Value { get; set; }
        public bool IsPermanent { get; }

        public CacheItem(string key, object value, bool isPermanent)
        {
            Key = key;
            Value = value;
            IsPermanent = isPermanent;

            // Automatically adjust the permanent item count
            if (isPermanent)
            {
                // Update the permanent item count directly in GlobalCache
                GlobalCache.Instance._permanentItemCount++;
            }
        }
    }

    public int CurrentCapacity
    {
        get
        {
            lock (_lock)
            {
                return _capacity;
            }
        }
    }

#if DEBUG
    /// <summary>
    /// Clears the cache for unit testing purposes.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _cacheMap.Clear();
            _usageOrder.Clear();
        }
    }

    // This allows you to create a custom cache instance for testing purposes in DEBUG builds
    private static int _testCapacity = 10;

    // A method for unit tests to set the custom capacity
    public static void SetTestCapacity(int capacity)
    {
        _testCapacity = capacity;
    }

    // Factory method for creating a cache with custom capacity in debug mode
    public static GlobalCache CreateForTesting()
    {
        return new GlobalCache(_testCapacity);
    }
#endif
}
