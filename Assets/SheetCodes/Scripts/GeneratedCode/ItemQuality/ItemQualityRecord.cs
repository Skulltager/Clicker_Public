using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class ItemQualityRecord : BaseRecord<ItemQualityIdentifier>
	{
		[ColumnName("Name")] [SerializeField] private string _name = default;
		public string Name { get { return _name; } set { if(!CheckEdit()) return; _name = value; }}

		//Does this type no longer exist? Delete from here..
		[ColumnName("Icon")] [SerializeField] private UnityEngine.Sprite _icon = default;
		public UnityEngine.Sprite Icon 
		{ 
			get { return _icon; } 
            set
            {
                if (!CheckEdit())
                    return;
#if UNITY_EDITOR
                if (value != null)
                {
                    string assetPath = AssetDatabase.GetAssetPath(value);
                    if(string.IsNullOrEmpty(assetPath))
                    {
                        Debug.LogError("SheetCodes: Reference Objects must be a direct reference from your project folder.");
                        return;
                    }
                }
                _icon = value;
#endif
            }
        }
		//..To here

		[ColumnName("Quality Text")] [SerializeField] private string _qualityText = default;
		public string QualityText { get { return _qualityText; } set { if(!CheckEdit()) return; _qualityText = value; }}

		[ColumnName("Minimum Quality Value")] [SerializeField] private long _minimumQualityValue = default;
		public long MinimumQualityValue { get { return _minimumQualityValue; } set { if(!CheckEdit()) return; _minimumQualityValue = value; }}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public ItemQualityModel model { get { return ModelManager.ItemQualityModel; } }
        private ItemQualityRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            ItemQualityRecord editableCopy = new ItemQualityRecord();
            editableCopy.Identifier = Identifier;
            editableCopy.originalRecord = this;
            CopyData(editableCopy);
            model.SetEditableCopy(editableCopy);
#else
            Debug.LogError("SheetCodes: Creating an editable record does not work in buolds. See documentation 'Editing your data at runtime' for more information.");
#endif
        }

        public override void SaveToScriptableObject()
        {
#if UNITY_EDITOR
            if (!runtimeEditingEnabled)
            {
                Debug.LogWarning("SheetCodes: Runtime Editing is not enabled for this object. Either you are not using the editable copy or you're trying to edit in a build.");
                return;
            }
            CopyData(originalRecord);
            model.SaveModel();
#else
            Debug.LogError("SheetCodes: Saving to ScriptableObject does not work in builds. See documentation 'Editing your data at runtime' for more information.");
#endif
        }

        private void CopyData(ItemQualityRecord record)
        {
            record._name = _name;
            record._icon = _icon;
            record._qualityText = _qualityText;
            record._minimumQualityValue = _minimumQualityValue;
        }

        private bool CheckEdit()
        {
            if (runtimeEditingEnabled)
                return true;

            Debug.LogWarning("SheetCodes: Runtime Editing is not enabled for this object. Either you are not using the editable copy or you're trying to edit in a build.");
            return false;
        }
    }
}
