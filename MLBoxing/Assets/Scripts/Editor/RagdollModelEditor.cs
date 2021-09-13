using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MLBoxing.Ragdoll;
using System.Linq;

[CustomEditor(typeof(RagdollModel))]
public class RagdollModelEditor : Editor
{

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("SelectActiveArticulations")) {
            var model = (RagdollModel)target;
            Selection.objects = model.allArticulations.Select(joint => joint.gameObject).ToArray();
        }
    }
}
