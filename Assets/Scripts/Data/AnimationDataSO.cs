using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Data
{
    [CreateAssetMenu(fileName = "NewAnimationData", menuName = "AnimationData")]
    public class AnimationDataSO : ScriptableObject
    {
        public string animationName;
        public AnimationClip animationClip;
    }
}
