using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private GameObject Row;

    [SerializeField]
    private int RowInitialCount;

    [SerializeField]
    private int RowMaxCount;

    [SerializeField]
    private Vector3 RowStep;

    private Queue<RowController> Rows = new Queue<RowController>();

    private int RowIndexer;

    private void Awake()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent += CreateRow;
        Overlord.Progress.Changed += OnProgressChanged;
    }

    private void OnProgressChanged(GameProgress obj)
    {
        if (obj == GameProgress.Beginning)
            OnStart();
    }

    void Start ()
    {
        OnStart();
    }

    private void OnStart()
    {
        RowIndexer = 0;

        while (Rows.Count > 0)
            Rows.Dequeue().Deconstruct();

        for (int i = 0; i < RowInitialCount; i++)
            CreateRow();
    }

    private void CreateRow(NextPlatformEvent ev)
    {
        CreateRow();
    }

    private void CreateRow()
    {
        RowIndexer++;
        var row = Instantiate(Row, transform);
        row.transform.localPosition = RowStep * RowIndexer;

        var controller = row.GetComponent<RowController>();
        controller.Init(2f * (RowIndexer % 2) - 1f);
        Rows.Enqueue(controller);

        if (RowIndexer > RowMaxCount)
            Rows.Dequeue().Deconstruct();
    }

    private void OnDestroy()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent -= CreateRow;
        Overlord.Progress.Changed -= OnProgressChanged;
    }
}
