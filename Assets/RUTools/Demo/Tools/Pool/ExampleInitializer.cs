using UnityEngine;
using UnityEngine.UI;

namespace RUT.Examples.Pool
{
    /// <summary>
    /// ExampleInitializer class.
    /// </summary>
    public class ExampleInitializer : MonoBehaviour
    {
        #region Public properties
        public Text poolText;
        public Text multiPoolText1;
        public Text multiPoolText2;
        [Space(5)]
        public Button spawn1Button;
        public Button spawn2Button;
        public Button spawn3Button;
        public Button dispose1Button;
        public Button dispose2Button;
        public Button dispose3Button;
        public Button disposeAll1Button;
        public Button disposeAll2Button;
        public Button disposeAll3Button;
        [Space(5)]
        public PoolExample1 example1;
        #endregion

        #region Private properties
        private int _totalPoolItems = -1;
        private int _freePoolItems = 0;
        private int _usedPoolItems = 0;

        private int _totalMultiPoolItems = -1;
        private int _freeMultiPoolItems = 0;
        private int _usedMultiPoolItems = 0;

        private string _poolInfoTemplate = "Total: {0} | Free: {1} | Used: {2}";
        #endregion

        #region API
        #endregion

        #region Unity
        private void Awake()
        {
            spawn1Button.onClick.AddListener(example1.SpawnCube1);
            spawn2Button.onClick.AddListener(example1.SpawnCube2);
            spawn3Button.onClick.AddListener(example1.SpawnCube3);
            dispose1Button.onClick.AddListener(example1.DisposeCube1);
            dispose2Button.onClick.AddListener(example1.DisposeCube2);
            dispose3Button.onClick.AddListener(example1.DisposeCube3);
            disposeAll1Button.onClick.AddListener(example1.DisposeAllFromPool);
            disposeAll2Button.onClick.AddListener(example1.DisposeAllFromMultiPool);
            disposeAll3Button.onClick.AddListener(example1.DisposeAllFromMultiPool);
        }

        //Update pool info on UI.
        private void Update()
        {
            int totalPoolItems = example1.pool.TotalItems;
            int freePoolItems = example1.pool.FreeItems;
            int usedPoolItems = example1.pool.UsedItems;

            int totalMultiPoolItems = example1.multiPool.TotalItems;
            int freeMultiPoolItems = example1.multiPool.FreeItems;
            int usedMultiPoolItems = example1.multiPool.UsedItems;

            if (totalPoolItems != _totalPoolItems || freePoolItems != _freePoolItems || usedPoolItems != _usedPoolItems)
            {
                _totalPoolItems = totalPoolItems;
                _freePoolItems = freePoolItems;
                _usedPoolItems = usedPoolItems;

                if (poolText != null)
                {
                    poolText.text = string.Format(_poolInfoTemplate, _totalPoolItems, _freePoolItems, _usedPoolItems);
                }
            }

            if (totalMultiPoolItems != _totalMultiPoolItems || freeMultiPoolItems != _freeMultiPoolItems || usedMultiPoolItems != _usedMultiPoolItems)
            {
                _totalMultiPoolItems = totalMultiPoolItems;
                _freeMultiPoolItems = freeMultiPoolItems;
                _usedMultiPoolItems = usedMultiPoolItems;

                if (multiPoolText1 != null)
                {
                    multiPoolText1.text = string.Format(_poolInfoTemplate, _totalMultiPoolItems, _freeMultiPoolItems, _usedMultiPoolItems);
                }
                if (multiPoolText2 != null)
                {
                    multiPoolText2.text = string.Format(_poolInfoTemplate, _totalMultiPoolItems, _freeMultiPoolItems, _usedMultiPoolItems);
                }
            }
        }
        #endregion
    }
}