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
        [SerializeField] private Animator leaderAnimator;
        [SerializeField] private Transform leader;
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
        private int lineRendererMainTextSTId;
        private int lineRendererBaseSize;
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
            lineRendererMainTextSTId = Shader.PropertyToID("_MainTex_ST");
            panel.Init(SetDampingSystem);
            useCameraFollowing = false;
            lineRendererBaseSize = 16;
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
            targetPosition = leader.position + followerOffset;
            follower.position = dampingSystem.Calculate(targetPosition);

            if (useCameraFollowing == false)
                return;

            cameraAnchor.position = follower.position;
        }

        private void LateUpdate()
        {
            if (leader.position == follower.position)
                return;

            lineRenderer.SetPosition(0, leader.position);
            lineRenderer.SetPosition(1, follower.position);

            float distance = Vector3.Distance(leader.position, follower.position);
            float textureScale = lineRendererBaseSize / distance;

            lineRendererPropertyBlock.SetVector(lineRendererMainTextSTId, new Vector4(textureScale, 1f, 0f, 0f));
            lineRenderer.SetPropertyBlock(lineRendererPropertyBlock);
        }

        public void OnUseCameraFollowingButtonClick()
        {
            useCameraFollowing = !useCameraFollowing;
            SetUiState();

            lineRenderer.gameObject.SetActive(useCameraFollowing == false);
            follower.gameObject.SetActive(useCameraFollowing == false);

            if (useCameraFollowing)
                return;

            cameraAnchor.position = cameraAnchorInitialPosition;
        }

        public void OnSpeedButtonClick(int index)
        {
            if (index < 0 || index >= speedSettings.Length)
                return;

            speedIndex = index;
            leaderAnimator.speed = speedSettings[speedIndex].SpeedSliderValue;

            SetUiState();
        }
    }
}