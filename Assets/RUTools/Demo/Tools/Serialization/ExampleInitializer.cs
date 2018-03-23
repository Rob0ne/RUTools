using UnityEngine;
using UnityEngine.UI;

namespace RUT.Examples.Serialization
{
    /// <summary>
    /// ExampleInitializer class.
    /// </summary>
    public class ExampleInitializer : MonoBehaviour
    {
        #region Public properties
        public Button xmlSerializeButton;
        public Button xmlDeserializeButton;
        public Button jsonSerializeButton;
        public Button jsonDeserializeButton;
        public Button binarySerializeButton;
        public Button binaryDeserializeButton;
        [Space(5)]
        public InputField intInput;
        public InputField floatInput;
        public InputField stringInput;
        [Space(5)]
        public Text intOutput;
        public Text floatOutput;
        public Text stringOutput;
        [Space(5)]
        public SerializationExample1 example1;
        #endregion

        #region Unity
        private void Awake()
        {
            xmlSerializeButton.onClick.AddListener(SaveData);
            xmlSerializeButton.onClick.AddListener(example1.XmlSerialize);
            xmlDeserializeButton.onClick.AddListener(example1.XmlDeserialize);
            xmlDeserializeButton.onClick.AddListener(LoadData);

            jsonSerializeButton.onClick.AddListener(SaveData);
            jsonSerializeButton.onClick.AddListener(example1.JsonSerialize);
            jsonDeserializeButton.onClick.AddListener(example1.JsonDeserialize);
            jsonDeserializeButton.onClick.AddListener(LoadData);

            binarySerializeButton.onClick.AddListener(SaveData);
            binarySerializeButton.onClick.AddListener(example1.BinarySerialize);
            binaryDeserializeButton.onClick.AddListener(example1.BinaryDeserialize);
            binaryDeserializeButton.onClick.AddListener(LoadData);
        }
        #endregion

        #region Private methods
        private void SaveData()
        {
            if (intInput != null)
                example1.data.intValue = string.IsNullOrEmpty(intInput.text) ? 0 : System.Convert.ToInt32(intInput.text);
            if (floatInput != null)
                example1.data.floatValue = string.IsNullOrEmpty(floatInput.text) ? 0.0f : float.Parse(floatInput.text);
            if (stringInput != null)
                example1.data.stringValue = stringInput.text;
        }

        private void LoadData()
        {
            if (intOutput != null)
                intOutput.text = example1.data.intValue.ToString();
            if (floatOutput != null)
                floatOutput.text = example1.data.floatValue.ToString();
            if (stringOutput != null)
                stringOutput.text = example1.data.stringValue;
        }
        #endregion
    }
}