using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RUT.Tools.Localization;

namespace RUT.Examples.Localization
{
    /// <summary>
    /// ExampleInitializer class.
    /// </summary>
    public class ExampleInitializer : MonoBehaviour
    {
        #region Public properties
        public Button frenchButton;
        public Button englishButton;
        public Button invalidButton;
        [Space(5)]
        public LocalizationControllerByStringIDAsset localizationController;

        //For this example I'm using the scriptable object version of the localization controller.
        //This allows me to create an asset unrelated to the scene and reuse it whenever I need.

        public static LocalizationControllerByStringIDAsset LocalizationController
        { get { return FindObjectOfType< ExampleInitializer >().localizationController; } }
        #endregion

        #region Unity
        private void Start()
        {
            //Set start language.
            localizationController.SetLanguage("english");

            frenchButton.onClick.AddListener(() => { localizationController.SetLanguage("french"); });
            englishButton.onClick.AddListener(() => { localizationController.SetLanguage("english"); });
            invalidButton.onClick.AddListener(() => { localizationController.SetLanguage("EMPTY"); });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }

        private void OnDestroy()
        {
            //Don't forget to clear the scriptable object manually or use an instance instead.
            localizationController.Clear();
        }
        #endregion
    }
}