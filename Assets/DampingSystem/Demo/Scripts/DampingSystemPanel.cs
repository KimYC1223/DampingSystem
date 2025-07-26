using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DampingSystem.Demo
{
    public class DampingSystemPanel : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private Animator panelAnimator;
        [SerializeField] private string panelParamName;

        [Header("Slider")]
        [SerializeField] private Slider frequencySlider;
        [SerializeField] private Slider dampingRatioSlider;
        [SerializeField] private Slider initialResponseSlider;

        [Header("Input Field")]
        [SerializeField] private TMP_InputField frequencyInputField;
        [SerializeField] private TMP_InputField dampingRatioInputField;
        [SerializeField] private TMP_InputField initialResponseInputField;


        [Header("Preset")]
        [SerializeField] private DampingSystemInitialCondition[] presets;

        public DampingSystemInitialCondition CurrentDampingSystemCondition => new DampingSystemInitialCondition
        {
            Frequency = frequencySlider.value,
            DampingRatio = dampingRatioSlider.value,
            InitialResponse = initialResponseSlider.value
        };

        private event Action<DampingSystemInitialCondition> OnDampingSystemConditionChanged;
        private int panelParamHash;
        private bool isPanelClose;


        public void Init(Action<DampingSystemInitialCondition> onDampingSystemConditionChanged)
        {

            InitFields();
            InitUIElements();
            InitPanelAnimator();

            AddSliderListeners();
            AddInputFieldListeners();

            OnDampingSystemConditionChanged = onDampingSystemConditionChanged;
            OnDampingSystemConditionChanged?.Invoke(CurrentDampingSystemCondition);
        }

        private void InitFields()
        {
            panelParamHash = Animator.StringToHash(panelParamName);
            isPanelClose = false;
        }

        private void InitUIElements()
        {
            frequencyInputField.onEndEdit.RemoveAllListeners();
            dampingRatioInputField.onEndEdit.RemoveAllListeners();
            initialResponseInputField.onEndEdit.RemoveAllListeners();

            frequencySlider.onValueChanged.RemoveAllListeners();
            dampingRatioSlider.onValueChanged.RemoveAllListeners();
            initialResponseSlider.onValueChanged.RemoveAllListeners();
        }

        private void InitPanelAnimator()
        {
            panelAnimator.SetBool(panelParamHash, isPanelClose);
        }

        private void AddSliderListeners()
        {
            frequencySlider.onValueChanged.AddListener(_ =>
            {
                frequencyInputField.text = frequencySlider.value.ToString("F2");
                OnDampingSystemConditionChanged?.Invoke(CurrentDampingSystemCondition);
            });
            dampingRatioSlider.onValueChanged.AddListener(_ =>
            {
                dampingRatioInputField.text = dampingRatioSlider.value.ToString("F2");
                OnDampingSystemConditionChanged?.Invoke(CurrentDampingSystemCondition);
            });
            initialResponseSlider.onValueChanged.AddListener(_ =>
            {
                initialResponseInputField.text = initialResponseSlider.value.ToString("F2");
                OnDampingSystemConditionChanged?.Invoke(CurrentDampingSystemCondition);
            });
        }

        private void AddInputFieldListeners()
        {
            frequencyInputField.onEndEdit.AddListener(_ =>
            {
                frequencySlider.value = float.Parse(frequencyInputField.text);
                OnDampingSystemConditionChanged?.Invoke(CurrentDampingSystemCondition);
            });
            dampingRatioInputField.onEndEdit.AddListener(_ =>
            {
                dampingRatioSlider.value = float.Parse(dampingRatioInputField.text);
                OnDampingSystemConditionChanged?.Invoke(CurrentDampingSystemCondition);
            });
            initialResponseInputField.onEndEdit.AddListener(_ =>
            {
                initialResponseSlider.value = float.Parse(initialResponseInputField.text);
                OnDampingSystemConditionChanged?.Invoke(CurrentDampingSystemCondition);
            });
        }
        
        public void OnPanelToggleButtonClick()
        {
            isPanelClose = !isPanelClose;
            panelAnimator.SetBool(panelParamHash, isPanelClose);
        }

        public void OnPresetButtonClick(int index)
        {
            if (index < 0 || index >= presets.Length)
                throw new ArgumentException($"Invalid index : {index}");

            frequencySlider.value = presets[index].Frequency;
            dampingRatioSlider.value = presets[index].DampingRatio;
            initialResponseSlider.value = presets[index].InitialResponse;
        }
    }
}