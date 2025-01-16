
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create AnimationData")]

public class AnimationData : ScriptableObject
{
    [Header("�S�g�̊p�x")] public float[] wholeRotation;
    [Header("�r�̎�O���p�x")] public float[] armForwardRotation;
    [Header("�r�̉��p�x")] public float[] armBackRotation;
    [Header("���̎�O�̊p�x")] public float[] handForwardRotation;
    [Header("���̉��p�x")] public float[] handBackRotation;
    [Header("�������̉��p�x")] public float[] legForwardRotation;
    [Header("���̉��p�x")] public float[] footForwardRotation;
    [Header("�������̎�O�p�x")] public float[] legBackRotation;
    [Header("���̎�O�p�x")] public float[] footBackRotation;
}
