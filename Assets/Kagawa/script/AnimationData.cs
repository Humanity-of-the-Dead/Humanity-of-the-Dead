
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create AnimationData")]

public class AnimationData : ScriptableObject
{
    [Header("‘Sg‚ÌŠp“x")] public float[] wholeRotation;
    [Header("˜r‚Ìè‘O•ûŠp“x")] public float[] armForwardRotation;
    [Header("˜r‚Ì‰œŠp“x")] public float[] armBackRotation;
    [Header("èñ‚Ìè‘O‚ÌŠp“x")] public float[] handForwardRotation;
    [Header("èñ‚Ì‰œŠp“x")] public float[] handBackRotation;
    [Header("‘¾‚à‚à‚Ì‰œŠp“x")] public float[] legForwardRotation;
    [Header("‘«‚Ì‰œŠp“x")] public float[] footForwardRotation;
    [Header("‘¾‚à‚à‚Ìè‘OŠp“x")] public float[] legBackRotation;
    [Header("‘«‚Ìè‘OŠp“x")] public float[] footBackRotation;
}
