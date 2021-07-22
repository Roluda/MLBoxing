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
        Slider sliderZ = default;
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
            sliderX.value = attachedJoint.inputX;
            sliderY.value = attachedJoint.inputY;
            sliderZ.value = attachedJoint.inputZ;
            sliderX.gameObject.SetActive(attachedJoint.axis.HasFlag(JointAxis.X));
            sliderY.gameObject.SetActive(attachedJoint.axis.HasFlag(JointAxis.Y));
            sliderZ.gameObject.SetActive(attachedJoint.axis.HasFlag(JointAxis.Z));
        }

        private void FixedUpdate() {
            if (read) {
                sliderX.value = attachedJoint.inputX;
                sliderY.value = attachedJoint.inputY;
                sliderZ.value = attachedJoint.inputZ;
            } else {
                attachedJoint.inputX = sliderX.value;
                attachedJoint.inputY = sliderY.value;
                attachedJoint.inputZ = sliderZ.value;
            }
        }
    }
}
