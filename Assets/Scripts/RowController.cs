using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowController : MonoBehaviour
{
    [SerializeField]
    private GameObject Platform;

    [SerializeField]
    private float Width;

    [SerializeField]
    private float MinSpeed;

    [SerializeField]
    private float MaxSpeed;

    [SerializeField]
    private float MinPlatformLength;

    [SerializeField]
    private float MaxPlatformLength;

    [SerializeField]
    private float MinSpaceLength;

    [SerializeField]
    private float MaxSpaceLength;

    private HashSet<Transform> Platforms = new HashSet<Transform>();
    private float CurrentPoint;
    private float Speed;
    private Transform Trash = null;
    private float Side;

    public void Init(float side)
    {
        Side = side;
    }

    void Start()
    {
        if (Mathf.Abs(Side) != 1f)
            Side = Random.value > 0.5f ? -1 : 1;

        Speed = Side * GetRandomBetween(MinSpeed, MaxSpeed);
        CurrentPoint = - Width;

        while (CurrentPoint < Width)
            CreateNewPlatform();
    }

    private void CreateNewPlatform()
    {
        var block = PlatformPool.GetObject().transform;
        block.SetParent(transform);

        var size = GetRandomBetween(MinPlatformLength, MaxPlatformLength);
        block.localPosition = new Vector3(- Side * (CurrentPoint + size * 0.5f), 0f, 0f);
        block.localScale = new Vector3(size, block.localScale.y, block.localScale.z);
        CurrentPoint += size + GetRandomBetween(MinSpaceLength, MaxSpaceLength);
        Platforms.Add(block);
    }

    private float GetRandomBetween(float min, float max)
    {
        return Random.value * (max - min) + min;
    }

    void FixedUpdate()
    {
        CurrentPoint -= Side * Speed * Time.fixedDeltaTime;

        if (CurrentPoint < Width)
            CreateNewPlatform();

        foreach (var item in Platforms)
        {
            item.localPosition += new Vector3(Speed * Time.fixedDeltaTime, 0f, 0f);
            if (item.localPosition.x * Side > Width)
                Trash = item;
        }

        if (Trash != null)
        {
            Platforms.Remove(Trash);
            PlatformPool.ReturnObject(Trash.gameObject);
            Trash = null;
        }
    }

    public void Deconstruct()
    {
        foreach (var item in Platforms)
            PlatformPool.ReturnObject(item.gameObject);

        Destroy(gameObject);
    }
}
