using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedProperty<T>
{
    protected ActionProperty<T> mCurrentValue = new ActionProperty<T>();
    protected T mStartValue;
    protected T mFinishValue;

    private float mAnimationTime;
    private AnimationCurve mAnimationCurve;

    private float mState = 1f;

    public T TargetValue
    {
        set
        {
            mState = 0f;
            mStartValue = mCurrentValue.Value;
            mFinishValue = value;
        }
    }

    public T Value
    {
        set
        {
            mState = 1f;
            mFinishValue = value;
            mCurrentValue.Value = value;
        }
        get
        {
            return mCurrentValue.Value;
        }
    }

    public AnimatedProperty(T value, float animationTime, AnimationCurve animationCurve = null, Action<T> setter = null)
    {
        mCurrentValue.Value = value;
        mAnimationTime = animationTime;
        mAnimationCurve = animationCurve ?? AnimationCurve.Linear(0f, 0f, 1f, 1f);
        if (setter != null)
            mCurrentValue.Changed += setter;
    }

    public void Update()
    {
        if (mState == 1f)
            return;

        mState = Mathf.Clamp01(mState + Time.deltaTime / mAnimationTime);

        var lerpValue = mAnimationCurve.Evaluate(mState);
        mCurrentValue.Value = Lerp(lerpValue);
    }

    protected virtual T Lerp(float value)
    {
        return mCurrentValue.Value;
    }
}

public class AnimatedFloat : AnimatedProperty<float>
{
    public AnimatedFloat(float value, float animationTime, AnimationCurve animationCurve = null, Action<float> setter = null) : base(value, animationTime, animationCurve, setter)
    {
    }

    protected override float Lerp(float value)
    {
        return Mathf.Lerp(mStartValue, mFinishValue, value);
    }
}

public class AnimatedVector3 : AnimatedProperty<Vector3>
{
    public AnimatedVector3(Vector3 value, float animationTime, AnimationCurve animationCurve = null, Action<Vector3> setter = null) : base(value, animationTime, animationCurve, setter)
    {
    }

    protected override Vector3 Lerp(float value)
    {
        return Vector3.Lerp(mStartValue, mFinishValue, value);
    }
}

public class AnimatedSphereVector3 : AnimatedProperty<Vector3>
{
    public AnimatedSphereVector3(Vector3 value, float animationTime, AnimationCurve animationCurve = null, Action<Vector3> setter = null) : base(value, animationTime, animationCurve, setter)
    {
    }

    protected override Vector3 Lerp(float value)
    {
        return Vector3.Slerp(mStartValue, mFinishValue, value);
    }
}

public class AnimatedQuaternion : AnimatedProperty<Quaternion>
{
    public AnimatedQuaternion(Quaternion value, float animationTime, AnimationCurve animationCurve = null, Action<Quaternion> setter = null) : base(value, animationTime, animationCurve, setter)
    {
    }

    protected override Quaternion Lerp(float value)
    {
        return Quaternion.Slerp(mStartValue, mFinishValue, value);
    }
}




//public class AnimatedTransformPosition
//{
//    protected ActionProperty<Vector3> mCurrentValue = new ActionProperty<Vector3>();
//    protected NullableTransformPosition mStartTransform = new NullableTransformPosition();
//    protected NullableTransformPosition mFinishTransform = new NullableTransformPosition();

//    private float mAnimationTime;
//    private AnimationCurve mAnimationCurve;

//    private float mState = 1f;

//    public Transform TargetValue
//    {
//        set
//        {
//            if (mState == 1f)
//            {
//                mStartTransform.Dispose();
//                mStartTransform = mFinishTransform;
//                mFinishTransform = new NullableTransformPosition();
//            }
//            else
//            {
//                mStartTransform.Position = Value;
//            }

//            mFinishTransform.Transform = value;
//            mState = 0f;
//        }
//    }

//    public Vector3 Value
//    {
//        set
//        {
//            mState = 1f;
//            mFinishTransform.Position = value;
//            mCurrentValue.Value = value;
//        }
//        get
//        {
//            return mCurrentValue.Value;
//        }
//    }

//    public AnimatedTransformPosition(Transform value, float animationTime, AnimationCurve animationCurve = null, Action<Vector3> setter = null)
//    {
//        mFinishTransform.Transform = value;
//        mCurrentValue.Value = value.position;

//        mAnimationTime = animationTime;
//        mAnimationCurve = animationCurve ?? AnimationCurve.Linear(0f, 0f, 1f, 1f);

//        if (setter != null)
//            mCurrentValue.Changed += setter;
//    }

//    public void Update()
//    {
//        mState = Mathf.Clamp01(mState + Time.deltaTime / mAnimationTime);

//        var lerpValue = mAnimationCurve.Evaluate(mState);
//        mCurrentValue.Value = Lerp(lerpValue);
//    }

//    protected virtual Vector3 Lerp(float value)
//    {
//        return Vector3.Lerp(mStartTransform.Position, mFinishTransform.Position, value);
//    }
//}

//public class NullableTransformPosition : IDisposable
//{
//    private Transform mTransform = null;
//    private Vector3 mPosition = Vector3.zero;

//    private GameObjectTracker mTracker;

//    public Transform Transform
//    {
//        set
//        {
//            if (mTracker != null)
//                mTracker.Rescue();

//            mTransform = value;
//            mTracker = mTransform.gameObject.AddComponent<GameObjectTracker>();
//            mTracker.DistressCall += (() => Position = mTransform.position);
//        }
//    }

//    public Vector3 Position
//    {
//        set
//        {
//            if (mTracker != null)
//                mTracker.Rescue();

//            mTransform = null;
//            mPosition = value;
//        }
//        get
//        {
//            if (mTransform)
//                return mTransform.position;
//            else
//                return mPosition;
//        }
//    }

//    public void Dispose()
//    {
//        if (mTracker != null)
//            mTracker.Rescue();
//    }
//}



public class AnimatedPropertyLinearFloat
{
    protected ActionProperty<float> mCurrentValue = new ActionProperty<float>();
    protected float mMaxSpeed;

    public float TargetValue
    {
        set;
        get;
    }

    public float Value
    {
        get
        {
            return mCurrentValue.Value;
        }
    }

    public AnimatedPropertyLinearFloat(float value, float maxSpeed, Action<float> setter = null)
    {
        mCurrentValue.Value = value;
        mMaxSpeed = maxSpeed;
        if (setter != null)
            mCurrentValue.Changed += setter;
    }

    public void Update()
    {
        mCurrentValue.Value += Mathf.Clamp(TargetValue - mCurrentValue.Value, -mMaxSpeed * Time.deltaTime, mMaxSpeed * Time.deltaTime);
    }
}

public class AnimatedPropertySquareFloat
{
    protected ActionProperty<float> mCurrentValue = new ActionProperty<float>();
    protected float mCurrentSpeed = 0f;
    protected float mMaxAcceleration;

    public float TargetValue
    {
        set;
        get;
    }

    public float Value
    {
        get
        {
            return mCurrentValue.Value;
        }
    }

    public AnimatedPropertySquareFloat(float value, float maxAcceleration, Action<float> setter = null)
    {
        mCurrentValue.Value = value;
        mMaxAcceleration = maxAcceleration;
        if (setter != null)
            mCurrentValue.Changed += setter;
    }

    public void Update()
    {
        var targetSpeed = Mathf.Sqrt(2f * mMaxAcceleration * Mathf.Abs(TargetValue - mCurrentValue.Value)) * Mathf.Sign(TargetValue - mCurrentValue.Value);
        mCurrentSpeed += Mathf.Clamp(targetSpeed - mCurrentSpeed, -mMaxAcceleration * Time.deltaTime, mMaxAcceleration * Time.deltaTime);
        mCurrentValue.Value += Mathf.Clamp(TargetValue - mCurrentValue.Value, -Mathf.Abs(mCurrentSpeed) * Time.deltaTime, Mathf.Abs(mCurrentSpeed) * Time.deltaTime);
    }
}

public class AnimatedPropertyExponentalFloat
{
    protected ActionProperty<float> mCurrentValue = new ActionProperty<float>();
    protected float mCurrentSpeed = 0f;
    protected float mTimeResponse;
    protected float mMinSpeed = 0.1f;

    public float TargetValue
    {
        set;
        get;
    }

    public float Value
    {
        get
        {
            return mCurrentValue.Value;
        }
    }

    public AnimatedPropertyExponentalFloat(float value, float timeResponse, Action<float> setter = null)
    {
        mCurrentValue.Value = value;
        mTimeResponse = timeResponse;
        if (setter != null)
            mCurrentValue.Changed += setter;
    }

    public void Update()
    {
        mCurrentSpeed = Mathf.Max(mMinSpeed, mCurrentSpeed);
        var targetSpeed = mTimeResponse * Mathf.Abs(TargetValue - Value);
        mCurrentSpeed = Mathf.Clamp(targetSpeed, mCurrentSpeed / Mathf.Exp(mTimeResponse * Time.deltaTime), mCurrentSpeed * Mathf.Exp(mTimeResponse * Time.deltaTime));
        mCurrentValue.Value = Mathf.Clamp(TargetValue,  mCurrentValue.Value - mCurrentSpeed * Time.deltaTime, mCurrentValue.Value + mCurrentSpeed * Time.deltaTime);
    }
}