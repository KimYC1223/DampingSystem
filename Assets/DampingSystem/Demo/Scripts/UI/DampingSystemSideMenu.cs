using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DampingSystem.Demo
{
    public class DampingSystemSideMenu : MonoBehaviour
    {
        private const string GITHUB_URL = "https://github.com/KimYC1223/DampingSystem";
        private const string BLOG_URL = "https://KimYC1223.github.io";
        private const string REFERENCE_URL = "https://www.youtube.com/watch?v=KPoeNZZ6H4s";

        [Header("Animator")]
        [SerializeField] private Animator sideMenuAnimator;
        [SerializeField] private Animator transitionAnimator;

        private Coroutine loadSceneCoroutine;
        private int animatorParamIsShow;
        private int animatorParamTransition;
        private bool isShow;

        private void OnEnable()
        {
            animatorParamIsShow = Animator.StringToHash("IsShow");
            animatorParamTransition = Animator.StringToHash("TransitionOut");
            SetVisible(false);
        }

        private void SetVisible(bool visible)
        {
            isShow = visible;
            sideMenuAnimator.SetBool(animatorParamIsShow, visible);
        }

        private void LoadScene(string sceneName)
        {
            if (string.Equals(SceneManager.GetActiveScene().name, sceneName) || loadSceneCoroutine != null)
                return;

            transitionAnimator.SetTrigger(animatorParamTransition);
            loadSceneCoroutine = StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            do
            {
                yield return waitForEndOfFrame;
            } while (transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

            SceneManager.LoadScene(sceneName);
        }

        public void HandleOnTouchBlockerClick() => SetVisible(false);

        public void HandleOnMenuButtonClick() => SetVisible(!isShow);

        public void HandleOnVector2DemoButtonClick() => LoadScene(nameof(DemoSceneName.Vector2DemoScene));

        public void HandleOnVector3DemoButtonClick() => LoadScene(nameof(DemoSceneName.Vector3DemoScene));

        public void HandleOnObjectTargetDemoButtonClick() => LoadScene(nameof(DemoSceneName.ObjectTargetDemoScene));

        public void HandleOnCardDemoButtonClick() => LoadScene(nameof(DemoSceneName.CardDemoScene));

        public void HandleOnVisitReferenceVideoButtonClick() => Application.OpenURL(REFERENCE_URL);

        public void HandleOnVisitGitRepositoryButtonClick() => Application.OpenURL(GITHUB_URL);

        public void HandleOnVisitBlogButtonClick() => Application.OpenURL(BLOG_URL);
    }
}