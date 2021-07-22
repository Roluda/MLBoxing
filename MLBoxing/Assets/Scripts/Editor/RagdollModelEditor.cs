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

        if (GUILayout.Button("SelectActiveJoints")) {
            var model = (RagdollModel)target;
            Selection.objects = model.allJoints.Select(joint => joint.gameObject).ToArray();
        }
        if (GUILayout.Button("SelectActiveLimbs")) {
            var model = (RagdollModel)target;
            Selection.objects = model.allLimbs.Select(limb => limb.gameObject).ToArray();
        }
    }
}
