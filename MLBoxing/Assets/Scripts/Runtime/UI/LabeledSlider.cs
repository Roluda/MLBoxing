using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MLBoxing.UI {
    public class LabeledSlider : MonoBehaviour {

        [SerializeField]
        Slider slider = default;
        [SerializeField]
        TMP_Text labelObject = default;

        public float value { get => slider.value; set => slider.value = value; }
        public string label {
            get => labelObject.text;
            set {
                labelObject.text = value;
                name = value;
            }
        }
    }
}
