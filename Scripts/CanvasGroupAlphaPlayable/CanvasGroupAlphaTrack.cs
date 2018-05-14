using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


namespace UnityTimelineTracks
{
    [TrackColor(0, 0, 0)]
    [TrackClipType(typeof(CanvasGroupAlpha))]
    [TrackBindingType(typeof(CanvasGroup))]
    public class CanvasGroupAlphaTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            // Hack to set the display name of the clip
            foreach (var c in GetClips())
            {
                var clip = (CanvasGroupAlpha)c.asset;
                c.displayName = string.Format("{0:0.00}", clip.Template.Alpha);
            }

            return ScriptPlayable<CanvasGroupAlphaMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
