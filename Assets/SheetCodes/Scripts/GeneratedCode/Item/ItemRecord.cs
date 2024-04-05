using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class ItemRecord : BaseRecord<ItemIdentifier>
	{
		//Does this type no longer exist? Delete from here..
		[ColumnName("Material")] [SerializeField] private UnityEngine.Material _material = default;
		public UnityEngine.Material Material 
		{ 
			get { return _material; } 
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
                _material = value;
#endif
            }
        }
		//..To here

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

		[ColumnName("Name")] [SerializeField] private string _name = default;
		public string Name { get { return _name; } set { if(!CheckEdit()) return; _name = value; }}

		[ColumnName("Item Category")] [SerializeField] private ItemCategoryIdentifier _itemCategory = default;
		[NonSerialized] private ItemCategoryRecord _itemCategoryRecord = default;
		public ItemCategoryRecord ItemCategory 
		{ 
			get 
			{ 
				if(_itemCategoryRecord == null)
					_itemCategoryRecord = ModelManager.ItemCategoryModel.GetRecord(_itemCategory);
				return _itemCategoryRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _itemCategory = ItemCategoryIdentifier.None;
                else
                    _itemCategory = value.Identifier;
				_itemCategoryRecord = null;
			}
		}

		[ColumnName("Weight")] [SerializeField] private long _weight = default;
		public long Weight { get { return _weight; } set { if(!CheckEdit()) return; _weight = value; }}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public ItemModel model { get { return ModelManager.ItemModel; } }
        private ItemRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            ItemRecord editableCopy = new ItemRecord();
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

        private void CopyData(ItemRecord record)
        {
            record._material = _material;
            record._icon = _icon;
            record._name = _name;
            record._itemCategory = _itemCategory;
            record._weight = _weight;
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
