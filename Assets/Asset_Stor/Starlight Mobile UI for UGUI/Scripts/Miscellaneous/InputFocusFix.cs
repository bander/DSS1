using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Input))]
    public class InputFocusFix : MonoBehaviour
    {
        [SerializeField] private Color m_Color;
        [SerializeField] private Text m_Target;

        protected void Awake()
        {
            InputField input = this.gameObject.GetComponent<InputField>();

            if (input != null)
            {
                input.onValueChanged.AddListener(OnValueChange);
            }
        }

        public void OnValueChange(string str)
        {
            if (this.m_Target != null && string.IsNullOrEmpty(str))
            {
                this.m_Target.canvasRenderer.SetColor(this.m_Color);
            }
        }
    }
}