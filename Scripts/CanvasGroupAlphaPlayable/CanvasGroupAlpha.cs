using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


namespace UnityTimelineTracks
{

    [Serializable]
    public class CanvasGroupAlphaBehaviour : PlayableBehaviour
    {
        [SerializeField, Range(0, 1)]
        public float Alpha;
    }


    public class CanvasGroupAlphaMixerBehaviour : PlayableBehaviour
    {
        CanvasGroup m_trackBinding;
        float m_initialValue;

        public override void OnGraphStop(Playable playable)
        {
            if (m_trackBinding != null)
            {
                // 初期値の復旧
                m_trackBinding.alpha = m_initialValue;
                m_trackBinding = null;
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var trackBinding = playerData as CanvasGroup;
            if (trackBinding == null)
            {
                return;
            }

            if (m_trackBinding == null)
            {
                m_trackBinding = trackBinding;
                // 初期値の記憶
                m_initialValue = m_trackBinding.alpha;
                return;
            }

            // 全clipのalpha値をweightに応じて合計する
            int inputCount = playable.GetInputCount();
            float alpha = 0;
            for (int i = 0; i < inputCount; ++i)
            {
                var inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<CanvasGroupAlphaBehaviour>)playable.GetInput(i);
                var input = inputPlayable.GetBehaviour();
                alpha += input.Alpha * inputWeight;
            }

            // 反映
            m_trackBinding.alpha = alpha;
        }
    }


    [Serializable]
    public class CanvasGroupAlpha : PlayableAsset, ITimelineClipAsset
    {
        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        [SerializeField]
        public CanvasGroupAlphaBehaviour Template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<CanvasGroupAlphaBehaviour>.Create(graph, Template);
        }
    }
}
