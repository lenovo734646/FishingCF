using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : DDOLSingleton<ObjectPoolManager>
{
    private List<Pool> poolList = new List<Pool>();

    public Transform Spawn(Transform trans, Transform parent = null)
    {
        return Spawn(trans, Vector3.zero, Quaternion.identity, parent);
    }

    public Transform Spawn(Transform trans, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        return Spawn(trans.gameObject, pos, rot, parent).transform;
    }

    public  GameObject Spawn(GameObject obj, Transform parent = null)
    {
        return Spawn(obj, Vector3.zero, Quaternion.identity, parent);
    }

    public GameObject Spawn(GameObject obj, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        return _Spawn(obj, pos, rot, parent);
    }

    private GameObject _Spawn(GameObject obj, Vector3 pos, Quaternion rot, Transform parent)
    {
        if (obj == null)
        {
            Debug.Log("NullReferenceException: obj unspecified");
            return null;
        }

        int ID = GetPoolID(obj);
        if (ID == -1)
            ID = _New(obj);

        return poolList[ID].Spawn(pos, rot, parent);
    }

    public void UnspawnChildren(Transform parent)
    {
        if (parent == null)
        {
            Debug.LogError("UnspawnChildren parent == null");
            return;
        }

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Unspawn(parent.GetChild(i));
        }
    }

    public void Unspawn(Transform trans)
    {
        _Unspawn(trans.gameObject);
    }

    public void Unspawn(GameObject obj)
    {
        _Unspawn(obj);
    }

    private void _Unspawn(GameObject obj)
    {
        for (int i = 0, imax = poolList.Count; i < imax; i++)
        {
            if(poolList[i].Unspawn(obj))
                return;
        }

        Destroy(obj);
    }

    public int New(Transform trans, int count = 1)
    {
        return _New(trans.gameObject, count);
    }

    public int New(GameObject obj, int count = 1)
    {
        return _New(obj, count);
    }

    private int _New(GameObject obj, int count = 1)
    {
        int ID = GetPoolID(obj);
        if (ID != -1)
        {
            poolList[ID].MachObjectCount(count);
        }
        else
        {
            Pool pool = new Pool();
            pool.prefab = obj;
            pool.MachObjectCount(count);
            poolList.Add(pool);
            ID = poolList.Count - 1;
        }

        return ID;
    }

    public int GetPoolID(GameObject obj)
    {
        for (int i = 0, imax = poolList.Count; i < imax; i++)
        {
            if (poolList[i].prefab == obj)
                return i;
        }

        return -1;
    }

    public void ClearAll()
    {
        for (int i = 0, imax = poolList.Count; i < imax; i++)
            poolList[i].Clear();
        poolList.Clear();
    }

    public Transform GetOPMTransform()
    {
        return transform;
    }
}

[System.Serializable]
public class Pool
{
    public GameObject prefab;
    public List<GameObject> inactiveList = new List<GameObject>();
    public List<GameObject> activeList = new List<GameObject>();
    public int max = 1000;

    public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent = null)
    {
        GameObject obj = null;
        if (inactiveList.Count == 0)
        {
            obj = (GameObject)MonoBehaviour.Instantiate(prefab, pos, rot);
        }
        else
        {
            obj = inactiveList[0];
            inactiveList.RemoveAt(0);
        }
        if (obj == null)
        {
            Debug.LogError("ObjectPoolManager : " + prefab + "失败");
            return null;
        }
        obj.transform.SetParent(parent, false);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = pos;
        obj.transform.localRotation = rot;
        obj.SetActive(true);
        activeList.Add(obj);
        return obj;
    }

    public bool Unspawn(GameObject obj)
    {
        if (activeList.Contains(obj))
        {
            obj.SetActive(false);
            obj.transform.SetParent(ObjectPoolManager.Instance.GetOPMTransform());
            inactiveList.Add(obj);
            activeList.Remove(obj);
            return true;
        }
        return false;
    }

    public void MachObjectCount(int count)
    {
        if (count > max)
            return;

        int currentCount = activeList.Count + inactiveList.Count;
        for (int i = currentCount; i < count; i++)
        {
            GameObject obj = (GameObject)MonoBehaviour.Instantiate(prefab);
            obj.transform.SetParent(ObjectPoolManager.Instance.GetOPMTransform());
            obj.SetActive(false);
            inactiveList.Add(obj);
        }
    }

    public void Clear()
    {
        for (int i = 0, imax = inactiveList.Count; i < imax; i++)
        {
            if (inactiveList[i] != null)
                MonoBehaviour.Destroy(inactiveList[i]);
        }

        for (int i = 0, imax = activeList.Count; i < imax; i++)
        {
            if (activeList[i] != null)
                MonoBehaviour.Destroy(activeList[i]);
        }

        inactiveList.Clear();
        activeList.Clear();
    }
}