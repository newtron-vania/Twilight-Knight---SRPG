using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Data
{
    [CreateAssetMenu(fileName = "NewAnimationData", menuName = "AnimationData")]
    public class AnimationDataSO : ScriptableObject
    {
        public string animationName;
        public AnimationClip animationClip;

        public AnimationDataSO Clone()
        {
            AnimationDataSO data = new AnimationDataSO();
            data.animationName = this.animationName;
            data.animationClip = this.animationClip;

            return data;
        }
    }
}
