using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionProperty<T>
{
    protected T mValue;
    public event Action<T> Changed;

    public virtual T Value
    {
        get { return mValue; }
        set
        {
            if (mValue == null || !mValue.Equals(value))
            {
                mValue = value;
                if (Changed != null)
                    Changed(mValue);
            }

        }
    }
}

public class ClampedActionProperty<T> : ActionProperty<T> where T : IComparable
{
    private T mMinValue;
    private T mMaxValue;

    public ClampedActionProperty(T min, T max)
    {
        mMinValue = min;
        mMaxValue = max;
    }

    public override T Value
    {
        get { return mValue; }
        set
        {
            if (value.CompareTo(mMaxValue) > 0)
                value = mMaxValue;

            if (value.CompareTo(mMinValue) < 0)
                value = mMinValue;

            base.Value = value;
        }
    }
}

public class RestrictedActionProperty<T> : ActionProperty<T>
{
    private T mMinValue;
    private T mMaxValue;
    public delegate bool Checking(T arg);
    private event Checking ValueCheck;

    public RestrictedActionProperty(Checking check)
    {
        ValueCheck = check;
    }

    public override T Value
    {
        get { return mValue; }
        set
        {
            if (ValueCheck(value))
                base.Value = value;
        }
    }
}