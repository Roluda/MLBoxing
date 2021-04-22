using MLBoxing.Character;
using UnityEngine;

namespace MLBoxing.UI {
    public class RagdollManipulator : MonoBehaviour {
        enum Mode {
            Read,
            Write
        }
        [SerializeField]
        Mode mode = Mode.Write;

        [SerializeField]
        RagdollController observedRagdoll = default;

        [SerializeField]
        LabeledSlider neckX = default;
        [SerializeField]
        LabeledSlider neckY = default;
        [SerializeField]
        LabeledSlider chestX = default;
        [SerializeField]
        LabeledSlider chestY = default;
        [SerializeField]
        LabeledSlider leftShoulderX = default;
        [SerializeField]
        LabeledSlider leftShoulderY = default;
        [SerializeField]
        LabeledSlider leftElbowX = default;
        [SerializeField]
        LabeledSlider rightShoulderX = default;
        [SerializeField]
        LabeledSlider rightShoulderY = default;
        [SerializeField]
        LabeledSlider rightElbowX = default;
        [SerializeField]
        LabeledSlider leftHipX = default;
        [SerializeField]
        LabeledSlider leftHipY = default;
        [SerializeField]
        LabeledSlider leftKneeX = default;
        [SerializeField]
        LabeledSlider rightHipX = default;
        [SerializeField]
        LabeledSlider rightHipY = default;
        [SerializeField]
        LabeledSlider rightKneeX = default;

        private void OnValidate() {
            neckX.label = nameof(neckX);
            neckY.label = nameof(neckY);
            chestX.label = nameof(chestX);
            chestY.label = nameof(chestY);
            leftShoulderX.label = nameof(leftShoulderX);
            leftShoulderY.label = nameof(leftShoulderY);
            leftElbowX.label = nameof(leftElbowX);
            rightShoulderX.label = nameof(rightShoulderX);
            rightShoulderY.label = nameof(rightShoulderY);
            rightElbowX.label = nameof(rightElbowX);
            leftHipX.label = nameof(leftHipX);
            leftHipY.label = nameof(leftHipY);
            leftKneeX.label = nameof(leftKneeX);
            rightHipX.label = nameof(rightHipX);
            rightHipY.label = nameof(rightHipY);
            rightKneeX.label = nameof(rightKneeX);
        }

        // Update is called once per frame
        void Update() {
            CheckMouseInput();
            if (mode == Mode.Write) {
                SetRagdollValues();
            } else {
                SetSliderValues();
            }
        }

        void SetSliderValues() {
            if (!observedRagdoll) {
                return;
            }
            neckX.value = observedRagdoll.neckX;
            neckY.value = observedRagdoll.neckY;
            chestX.value = observedRagdoll.chestX;
            chestY.value = observedRagdoll.chestY;
            leftShoulderX.value = observedRagdoll.leftShoulderX;
            leftShoulderY.value = observedRagdoll.leftShoulderY;
            leftElbowX.value = observedRagdoll.leftElbowX;
            rightShoulderX.value = observedRagdoll.rightShoulderX;
            rightShoulderY.value = observedRagdoll.rightShoulderY;
            rightElbowX.value = observedRagdoll.rightElbowX;
            leftHipX.value = observedRagdoll.leftHipX;
            leftHipY.value = observedRagdoll.leftHipY;
            leftKneeX.value = observedRagdoll.leftKneeX;
            rightHipX.value = observedRagdoll.rightHipX;
            rightHipY.value = observedRagdoll.rightHipY;
            rightKneeX.value = observedRagdoll.rightKneeX;
        }

        void SetRagdollValues() {
            if (!observedRagdoll) {
                return;
            }
            observedRagdoll.neckX = neckX.value;
            observedRagdoll.neckY = neckY.value;
            observedRagdoll.chestX = chestX.value;
            observedRagdoll.chestY = chestY.value;
            observedRagdoll.leftShoulderX = leftShoulderX.value;
            observedRagdoll.leftShoulderY = leftShoulderY.value;
            observedRagdoll.leftElbowX = leftElbowX.value;
            observedRagdoll.rightShoulderX = rightShoulderX.value;
            observedRagdoll.rightShoulderY = rightShoulderY.value;
            observedRagdoll.rightElbowX = rightElbowX.value;
            observedRagdoll.leftHipX = leftHipX.value;
            observedRagdoll.leftHipY = leftHipY.value;
            observedRagdoll.leftKneeX = leftKneeX.value;
            observedRagdoll.rightHipX = rightHipX.value;
            observedRagdoll.rightHipY = rightHipY.value;
            observedRagdoll.rightKneeX = rightKneeX.value;
        }

        void CheckMouseInput() {
            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo)) {
                    var controller = hitInfo.collider.GetComponentInParent<RagdollController>();
                    if (controller) {
                        observedRagdoll = controller;
                    }
                }
            }
        }
    }
}
