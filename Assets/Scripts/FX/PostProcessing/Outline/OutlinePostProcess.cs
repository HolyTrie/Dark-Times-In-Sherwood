using System;
using UnityEngine.Rendering.PostProcessing;

namespace PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.AfterStack, m_PostProcessName)]
    public sealed class PostProcessOutlineSettings : PostProcessEffectSettings
    {
        public const string m_PostProcessName = "Outline";
        public FloatParameter thickness = new() { value = 1f};
        public FloatParameter depthMin = new() { value = 0f};
        public FloatParameter depthMax = new() { value = 1f};
    }
}