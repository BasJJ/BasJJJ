using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class GlobalCache
{
    private readonly int _capacity;
    private readonly ConcurrentDictionary<string, LinkedListNode<CacheItem>> _cacheMap;
    private readonly LinkedList<CacheItem> _usageOrder;
    private readonly object _lock = new object();

    private static readonly Lazy<GlobalCache> _instance = new Lazy<GlobalCache>(() => new GlobalCache(10));

    private GlobalCache(int capacity)
    {
        _capacity = capacity;
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
                if (_cacheMap.Count >= _capacity)
                {
                    EvictNonPermanentItem();
                }
                var newNode = new LinkedListNode<CacheItem>(new CacheItem(key, value, isPermanent));
                _usageOrder.AddFirst(newNode);
                _cacheMap[key] = newNode;
            }
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
#endif
}
