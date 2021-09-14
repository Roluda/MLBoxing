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

        public ArticulationController observedArticulation = default;
        public bool read = true;

        public string label {
            get => labelObject.text;
            set {
                labelObject.text = value;
                name = value;
            }
        }

        private void Start() {
            sliderX.value = observedArticulation.inputX;
            sliderY.value = observedArticulation.inputY;
            sliderZ.value = observedArticulation.inputZ;
            sliderX.gameObject.SetActive(observedArticulation.dof.HasFlag(DOF.X));
            sliderY.gameObject.SetActive(observedArticulation.dof.HasFlag(DOF.Y));
            sliderZ.gameObject.SetActive(observedArticulation.dof.HasFlag(DOF.Z));
        }

        private void FixedUpdate() {
            if (read) {
                sliderX.value = observedArticulation.inputX;
                sliderY.value = observedArticulation.inputY;
                sliderZ.value = observedArticulation.inputZ;
            } else {
                observedArticulation.SetInputX(sliderX.value);
                observedArticulation.SetInputY(sliderY.value);
                observedArticulation.SetInputZ(sliderZ.value);
            }
        }
    }
}
