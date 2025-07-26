using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DampingSystem.Demo
{
    public class DampingSystemFollowerDemo : MonoBehaviour
    {
        [Serializable]
        public class Speed
        {
            public float SpeedSliderValue;
            public Button SpeedButton;
            public TMP_Text SpeedButtonText;
        }

        [Header("Target")]
        [SerializeField] private Animator leader;
        [SerializeField] private Transform follower;
        [SerializeField] private Vector3 followerOffset;
        [SerializeField] private Transform cameraAnchor;
        [SerializeField] private LineRenderer lineRenderer;

        [Header("Panel")]
        [SerializeField] private DampingSystemPanel panel;

        [Header("Color")]
        [SerializeField] private Color activeButtonColor;
        [SerializeField] private Color inactiveButtonColor;
        [SerializeField] private Color activeTextColor;
        [SerializeField] private Color inactiveTextColor;

        [Header("Button")]
        [SerializeField] private Button useCameraFollowingButton;
        [SerializeField] private TMP_Text useCameraFollowingButtonText;

        [Header("Speed Setting")]
        [SerializeField] private Speed[] speedSettings;

        private MaterialPropertyBlock lineRendererPropertyBlock;
        private DampingSystemVector3 dampingSystem;
        private Vector3 cameraAnchorInitialPosition;
        private Vector3 targetPosition;
        private bool useCameraFollowing;
        private int speedIndex;

        private void OnEnable()
        {
            InitTargetPosition();
            InitField();

            SetUiState();
        }

        private void OnDisable()
        {
            lineRendererPropertyBlock.Clear();
            lineRendererPropertyBlock = null;
        }

        private void InitTargetPosition()
        {
            targetPosition = follower.position;
            cameraAnchorInitialPosition = cameraAnchor.position;
        }

        private void InitField()
        {
            lineRendererPropertyBlock = new MaterialPropertyBlock();
            
            panel.Init(SetDampingSystem);
            useCameraFollowing = false;
            speedIndex = 2;
        }

        private void SetUiState()
        {
            var speedSetting = speedSettings[speedIndex];

            foreach (var setting in speedSettings)
            {
                if (setting == speedSetting)
                {
                    speedSetting.SpeedButton.image.color = activeButtonColor;
                    speedSetting.SpeedButtonText.color = activeTextColor;
                    continue;
                }

                setting.SpeedButton.image.color = inactiveButtonColor;
                setting.SpeedButtonText.color = inactiveTextColor;
            }

            useCameraFollowingButton.image.color = useCameraFollowing ? activeButtonColor : inactiveButtonColor;
            useCameraFollowingButtonText.color = useCameraFollowing ? activeTextColor : inactiveTextColor;
        }

        private void SetDampingSystem(DampingSystemInitialCondition condition)
        {
            dampingSystem = new DampingSystemVector3(
                frequency: condition.Frequency,
                dampingRatio: condition.DampingRatio,
                initialResponse: condition.InitialResponse,
                initialCondition: targetPosition);
        }

        private void Update()
        {
            targetPosition = leader.transform.position + followerOffset;
            follower.position = dampingSystem.Calculate(targetPosition);

            if (useCameraFollowing == false)
                return;

            cameraAnchor.position = follower.position;
        }

        private void LateUpdate()
        {
            if (leader.transform.position == follower.position)
                return;

            lineRenderer.SetPosition(0, leader.transform.position);
            lineRenderer.SetPosition(1, follower.position);

            // 라인 렌더러의 텍스처 스케일 변경
            float distance = Vector3.Distance(leader.transform.position, follower.position);
            float textureScale = 16f / distance; // 원하는 스케일링 factor 조정
            
            // MaterialPropertyBlock을 사용하여 텍스처 스케일 설정
            lineRendererPropertyBlock.SetVector("_MainTex_ST", new Vector4(textureScale, 1f, 0f, 0f));
            lineRenderer.SetPropertyBlock(lineRendererPropertyBlock);
        }

        public void OnUseCameraFollowingButtonClick()
        {
            useCameraFollowing = !useCameraFollowing;
            SetUiState();

            if (useCameraFollowing)
                return;

            cameraAnchor.position = cameraAnchorInitialPosition;
        }

        public void OnSpeedButtonClick(int index)
        {
            if (index < 0 || index >= speedSettings.Length)
                return;

            speedIndex = index;
            leader.speed = speedSettings[speedIndex].SpeedSliderValue;

            SetUiState();
        }
    }
}