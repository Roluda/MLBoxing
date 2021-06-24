using MLBoxing.Character;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.UI {
    public class RagdollManipulator : MonoBehaviour {
        enum Mode {
            Read,
            Write
        }

        [Header("Debug Tool")]
        [SerializeField]
        RagdollController observedRagdoll = default;
        [SerializeField, EnumFlags]
        JointType manipulatedJoints = default;
        [SerializeField]
        Mode mode = Mode.Write;

        [Header("Monobehaviour Config")]
        [SerializeField]
        Transform spawnContext = default;
        [SerializeField]
        JointSlider sliderPrefab = default;

        List<JointSlider> currentSliders = new List<JointSlider>();

        bool isDirty = false;
        JointType oldJoints;

        void OnValidate() {
            if (Application.isPlaying) {
                if(oldJoints != manipulatedJoints) {
                    oldJoints = manipulatedJoints;
                    RefreshSliders();
                }
                currentSliders.ForEach(slider => slider.read = mode == Mode.Read);
            }
        }

        void RefreshSliders() {
            currentSliders.ForEach(slider => Destroy(slider.gameObject));
            currentSliders.Clear();
            foreach (var joint in observedRagdoll.FilterJoints(manipulatedJoints)) {
                var newSlider = Instantiate(sliderPrefab, spawnContext);
                newSlider.attachedJoint = joint;
                newSlider.read = mode == Mode.Read;
                newSlider.label = joint.jointType.ToString();
                currentSliders.Add(newSlider);
            }
        }

        // Update is called once per frame
        void Update() {
            CheckMouseInput();
        }

        void CheckMouseInput() {
            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo)) {
                    var controller = hitInfo.collider.GetComponentInParent<RagdollController>();
                    if (controller) {
                        observedRagdoll = controller;
                        RefreshSliders();
                    }
                }
            }
        }
    }
}
