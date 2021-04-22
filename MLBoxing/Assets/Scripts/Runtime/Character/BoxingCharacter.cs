using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.Character {
    public class BoxingCharacter : MonoBehaviour {
        public float height => m_height;

        [Header("Attributes")]
        [SerializeField, Range(0, 3)]
        float m_height = 1.7f;

        public GameObject head => m_head;
        public GameObject chest => chest;
        public GameObject leftFoot => m_leftFoot;

        public Vector3 position => transform.position;

        [Header("Character Setup")]
        [SerializeField]
        GameObject m_head = default;
        [SerializeField]
        GameObject m_leftHand = default;
        [SerializeField]
        GameObject m_rightHand = default;
        [SerializeField]
        GameObject m_chest = default;
        [SerializeField]
        GameObject m_leftFoot = default;
        [SerializeField]
        GameObject m_rightFoot = default;

        public IEnumerable<GameObject> allLimbs {
            get {
                yield return m_head;
                yield return m_leftHand;
                yield return m_rightHand;
                yield return m_chest;
                yield return m_leftFoot;
                yield return m_rightFoot;
            }
        }
    }
}
