using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DampingSystem.Demo
{
    public class DampingSystemVector3Demo : MonoBehaviour
    {

        [Flags]
        public enum Axis
        {
            X = 1 << 0,
            Y = 1 << 1,
            Z = 1 << 2
        }

        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField] private Transform cameraAnchor;
        [SerializeField] private Transform[] cameraAncestorList;

        [Header("Panel")]
        [SerializeField] private DampingSystemPanel panel;

        [Header("Color")]
        [SerializeField] private Color activeButtonColor;
        [SerializeField] private Color inactiveButtonColor;
        [SerializeField] private Color activeTextColor;
        [SerializeField] private Color inactiveTextColor;

        [Header("Button")]
        [SerializeField] private Button xAxisButton;
        [SerializeField] private Button yAxisButton;
        [SerializeField] private Button zAxisButton;
        [SerializeField] private TMP_Text xAxisButtonText;
        [SerializeField] private TMP_Text yAxisButtonText;
        [SerializeField] private TMP_Text zAxisButtonText;

        [Header("Controller")]
        [SerializeField] private Slider slider;

        private DampingSystemVector3 cubeRotationDampingSystem;
        private DampingSystemVector3 cameraPositionDampingSystem;
        private DampingSystemVector3 cameraRotationDampingSystem;
        private Vector3 targetCubeRotation;
        private Vector3 targetCameraPosition;
        private Vector3 targetCameraRotation;
        private Axis axis;
        private int cameraAnchorIndex;

        private void OnEnable()
        {
            InitUIElements();
            InitTargetRotation();
            InitField();

            AddSliderListeners();
        }

        private void InitUIElements()
        {
            slider.onValueChanged.RemoveAllListeners();
        }

        private void InitTargetRotation()
        {
            targetCubeRotation = target.localEulerAngles;
        }

        private void InitField()
        {
            panel.Init(SetDampingSystem);
            axis = Axis.Y;
            cameraAnchorIndex = 0;
        }

        private void SetDampingSystem(DampingSystemInitialCondition condition)
        {
            cubeRotationDampingSystem = new DampingSystemVector3(
                frequency: condition.Frequency,
                dampingRatio: condition.DampingRatio,
                initialResponse: condition.InitialResponse,
                initialCondition: targetCubeRotation);

            cameraPositionDampingSystem = new DampingSystemVector3(
                frequency: condition.Frequency,
                dampingRatio: condition.DampingRatio,
                initialResponse: condition.InitialResponse,
                initialCondition: targetCameraPosition);

            cameraRotationDampingSystem = new DampingSystemVector3(
                frequency: condition.Frequency,
                dampingRatio: condition.DampingRatio,
                initialResponse: condition.InitialResponse,
                initialCondition: targetCameraRotation);
        }

        private void Update()
        {
            target.localEulerAngles = cubeRotationDampingSystem.Calculate(targetCubeRotation);
            cameraAnchor.position = cameraPositionDampingSystem.Calculate(targetCameraPosition);
            cameraAnchor.localEulerAngles = cameraRotationDampingSystem.Calculate(targetCameraRotation);
        }

        private void AddSliderListeners()
        {
            slider.onValueChanged.AddListener(_ =>
            {
                targetCubeRotation = new Vector3(
                    axis.HasFlag(Axis.X) ? slider.value : targetCubeRotation.x,
                    axis.HasFlag(Axis.Y) ? slider.value : targetCubeRotation.y,
                    axis.HasFlag(Axis.Z) ? slider.value : targetCubeRotation.z);
            });
        }

        public void OnAxisButtonClick(int index)
        {
            if (index < 0 || index > 2)
                return;

            var buttonAxis = (Axis)(1 << index);
            axis ^= buttonAxis;

            xAxisButton.image.color = axis.HasFlag(Axis.X) ? activeButtonColor : inactiveButtonColor;
            yAxisButton.image.color = axis.HasFlag(Axis.Y) ? activeButtonColor : inactiveButtonColor;
            zAxisButton.image.color = axis.HasFlag(Axis.Z) ? activeButtonColor : inactiveButtonColor;

            xAxisButtonText.color = axis.HasFlag(Axis.X) ? activeTextColor : inactiveTextColor;
            yAxisButtonText.color = axis.HasFlag(Axis.Y) ? activeTextColor : inactiveTextColor;
            zAxisButtonText.color = axis.HasFlag(Axis.Z) ? activeTextColor : inactiveTextColor;
        }

        public void OnCameraAnchorLeftMoveButtonClick()
        {
            cameraAnchorIndex--;
            cameraAnchorIndex = cameraAnchorIndex < 0 ? cameraAncestorList.Length - 1 : cameraAnchorIndex;

            targetCameraPosition = cameraAncestorList[cameraAnchorIndex].position;
            targetCameraRotation = cameraAncestorList[cameraAnchorIndex].localEulerAngles;
        }

        public void OnCameraAnchorRightMoveButtonClick()
        {
            cameraAnchorIndex++;
            cameraAnchorIndex = cameraAnchorIndex >= cameraAncestorList.Length ? 0 : cameraAnchorIndex;

            targetCameraPosition = cameraAncestorList[cameraAnchorIndex].position;
            targetCameraRotation = cameraAncestorList[cameraAnchorIndex].localEulerAngles;
        }
    }
}