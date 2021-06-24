using MLBoxing.Ragdoll;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MLBoxing.UI {
    public class JointSlider : MonoBehaviour {



        [SerializeField]
        Slider sliderX = default;
        [SerializeField]
        Slider sliderY = default;
        [SerializeField]
        TMP_Text labelObject = default;

        public RagdollJoint attachedJoint = default;
        public bool read = true;

        public string label {
            get => labelObject.text;
            set {
                labelObject.text = value;
                name = value;
            }
        }

        private void Start() {
            sliderX.value = attachedJoint.positionInputX;
            sliderY.value = attachedJoint.positionInputY;
        }

        private void FixedUpdate() {
            if (read) {
                sliderX.value = attachedJoint.positionInputX;
                sliderY.value = attachedJoint.positionInputY;
            } else {
                attachedJoint.positionInputX = sliderX.value;
                attachedJoint.positionInputY = sliderY.value;
            }
        }
    }
}
