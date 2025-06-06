
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create AnimationData")]

public class AnimationData : ScriptableObject
{
    [Header("全身の角度")] public float[] wholeRotation;
    [Header("腕の手前方角度")] public float[] armForwardRotation;
    [Header("腕の奥角度")] public float[] armBackRotation;
    [Header("手首の手前の角度")] public float[] handForwardRotation;
    [Header("手首の奥角度")] public float[] handBackRotation;
    [Header("太ももの奥角度")] public float[] legForwardRotation;
    [Header("足の奥角度")] public float[] footForwardRotation;
    [Header("太ももの手前角度")] public float[] legBackRotation;
    [Header("足の手前角度")] public float[] footBackRotation;
}
