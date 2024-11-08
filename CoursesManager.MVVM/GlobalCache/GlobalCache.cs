public class GlobalCache
{
    private readonly int _capacity;
    private readonly Dictionary<string, LinkedListNode<(string Key, object Value)>> _cacheMap;
    private readonly LinkedList<(string Key, object Value)> _usageOrder;

    private static readonly Lazy<GlobalCache> _instance = new Lazy<GlobalCache>(() => new GlobalCache(10));

    private GlobalCache(int capacity)
    {
        _capacity = capacity;
        _cacheMap = new Dictionary<string, LinkedListNode<(string Key, object Value)>>();
        _usageOrder = new LinkedList<(string Key, object Value)>();
    }

    public static GlobalCache Instance => _instance.Value;

    public object Get(string key)
    {
        if (!_cacheMap.ContainsKey(key))
            throw new KeyNotFoundException();

        var node = _cacheMap[key];
        _usageOrder.Remove(node);
        _usageOrder.AddFirst(node);

        return node.Value.Value;
    }

    public void Put(string key, object value)
    {
        if (_cacheMap.ContainsKey(key))
        {
            var node = _cacheMap[key];
            node.Value = (key, value);
            _usageOrder.Remove(node);
            _usageOrder.AddFirst(node);
        }
        else
        {
            if (_cacheMap.Count >= _capacity)
            {
                var lruNode = _usageOrder.Last;
                if (lruNode != null)
                {
                    _cacheMap.Remove(lruNode.Value.Key);
                    _usageOrder.RemoveLast();
                }
            }

            var newNode = new LinkedListNode<(string Key, object Value)>((key, value));
            _usageOrder.AddFirst(newNode);
            _cacheMap[key] = newNode;
        }
    }
}
