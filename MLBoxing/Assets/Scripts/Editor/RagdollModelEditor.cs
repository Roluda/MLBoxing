using MLBoxing.Ragdoll;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RagdollModel))]
public class RagdollModelEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("SelectActiveArticulations")) {
            var model = (RagdollModel)target;
            Selection.objects = model.allArticulations.Select(joint => joint.gameObject).ToArray();
        }
        if (Application.isPlaying) {
            if (GUILayout.Button("Reset Articuation")) {
                var model = (RagdollModel)target;
                model.Reset();
            }
        }
    }
}
