using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{
    [SerializeField]
    private GameObject Prefab;

    [SerializeField]
    private int PreparedCount;

    private static Transform Root;
    private static PlatformPool Instance;

    private static Stack<GameObject> Objects = new Stack<GameObject>();

    private void Start()
    {
        Root = transform;
        Instance = this;

        for (int i = 0; i < PreparedCount; i++)
            ReturnObject(CreateObject());
    }

    private static GameObject CreateObject()
    {
        return Instantiate(Instance.Prefab);
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(Root);
        Objects.Push(obj);
    }

    public static GameObject GetObject()
    {
        if (Objects.Count > 0)
        {
            var obj = Objects.Pop();
            obj.SetActive(true);
            return obj;
        }
        else
            return CreateObject();
    }
}
