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
            var data = new AnimationDataSO();
            data.animationName = animationName;
            data.animationClip = animationClip;

            return data;
        }
    }
}