using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool<T> where T : Component
{
    [SerializeField] Transform _parent;
    public Transform Parent => _parent;
    [SerializeField] T _prefab;
    private List<T> _items = new List<T>();

    public T[] ActiveItems
    {
        get
        {
            List<T> _itemesActive = new List<T>();

            for (int i = 0; i < _items.Count; i++)
                if (_items[i].gameObject.activeSelf)
                    _itemesActive.Add(_items[i]);

            return _itemesActive.ToArray();
        }
    }

    public T Get
    {
        get
        {
            for (int i = 0; i < _items.Count; i++)
                if (_items[i].gameObject && !_items[i].gameObject.activeSelf)
                    return _items[i];


            return AddNewItem();
        }
    }

    private T AddNewItem()
    {
        var item = GameObject.Instantiate(_prefab, _parent);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        Init(item);
        _items.Add(item);
        item.name = typeof(T) + "_" + _items.Count;
        return item;
    }

    public void DeactiveItems()
    {
        for (int i = 0; i < _items.Count; i++)
            if (_items[i].gameObject.activeSelf)
                _items[i].gameObject.SetActive(false);
    }

    public T GetActive
    {
        get
        {
            T item = Get;
            item.gameObject.SetActive(true);
            return item;
        }
    }

    protected virtual void Init(T item)
    {
    }
}

[System.Serializable]
public class Pool
{
    [SerializeField] Transform _parent;
    [SerializeField] GameObject _prefab;
    private List<GameObject> _items = new List<GameObject>();

    public GameObject[] ActiveItems
    {
        get
        {
            List<GameObject> _itemesActive = new List<GameObject>();

            for (int i = 0; i < _items.Count; i++)
                if (_items[i].gameObject.activeSelf)
                    _itemesActive.Add(_items[i]);

            return _itemesActive.ToArray();
        }
    }

    public GameObject Get
    {
        get
        {
            for (int i = 0; i < _items.Count; i++)
                if (_items[i].gameObject && !_items[i].gameObject.activeSelf)
                    return _items[i];

            return AddNewItem();
        }
    }

    private GameObject AddNewItem()
    {
        var item = GameObject.Instantiate(_prefab, _parent);
        Init(item);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        _items.Add(item);
        item.name = "Pool_" + _items.Count;
        return item;
    }

    public GameObject GetActive
    {
        get
        {
            GameObject item = Get;
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public void DeactiveItems()
    {
        for (int i = 0; i < _items.Count; i++)
            if (_items[i].gameObject && _items[i].gameObject.activeSelf)
                _items[i].SetActive(false);
    }

    protected virtual void Init(GameObject item)
    {
    }
}