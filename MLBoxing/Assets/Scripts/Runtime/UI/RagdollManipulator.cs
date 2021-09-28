using MLBoxing.Ragdoll;
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
        RagdollModel observedRagdoll = default;
        [SerializeField, EnumFlags]
        JointType manipulatedJoints = default;
        [SerializeField]
        Mode mode = Mode.Write;
        [SerializeField]
        LayerMask hitLayers = default;

        [Header("Monobehaviour Config")]
        [SerializeField]
        Transform spawnContext = default;
        [SerializeField]
        JointSlider sliderPrefab = default;

        List<JointSlider> currentSliders = new List<JointSlider>();

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
            if (observedRagdoll) {
                foreach (var joint in observedRagdoll.FilterJoints(manipulatedJoints)) {
                    var newSlider = Instantiate(sliderPrefab, spawnContext);
                    newSlider.observedArticulation = joint;
                    newSlider.read = mode == Mode.Read;
                    newSlider.label = joint.jointType.ToString();
                    currentSliders.Add(newSlider);
                }
            }
        }

        // Update is called once per frame
        void Update() {
            CheckMouseInput();
        }

        public void Write(bool write) {
            mode = write ? Mode.Write : Mode.Read;
            currentSliders.ForEach(slider => slider.read = mode == Mode.Read);
        }

        void CheckMouseInput() {
            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo,Mathf.Infinity, hitLayers)) {
                    var controller = hitInfo.collider.GetComponentInParent<RagdollModel>();
                    if (controller) {
                        observedRagdoll = controller;
                        RefreshSliders();
                    }
                }
            }
        }
    }
}
