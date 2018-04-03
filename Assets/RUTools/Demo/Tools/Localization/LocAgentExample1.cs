using RUT.Tools.Localization;

namespace RUT.Examples.Localization
{
    /// <summary>
    /// LocAgentExample1 class.
    /// </summary>
    public abstract class LocAgentExample1 : LocAgentByStringID
    {
        #region Private properties
        protected ILocalizationController<string> _controller;
        #endregion

        #region Unity
        protected virtual void Awake()
        {
            //Localization agents must be made manually because they need a reference of a
            //ILocalizationController in order to get data from it when the language is changed.
            //Getting this reference can be done in a lot of ways, by singleton, direct reference,
            //automatic injection, ... so I'm only giving the base abstract class that holds the ID,
            //the rest must be added like in this script and its children.

            //Here I choose to get the reference with a static reference from the initializer class.
            //Not the best approach for a real project but it does the trick here.

            _controller = ExampleInitializer.LocalizationController;
            _controller.OnLanguageChanged += ReloadContent;
        }
        #endregion
    }
}