using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class CraftingRecipeRecord : BaseRecord<CraftingRecipeIdentifier>
	{
		[ColumnName("Category Category")] [SerializeField] private CraftingCategoryIdentifier _categoryCategory = default;
		[NonSerialized] private CraftingCategoryRecord _categoryCategoryRecord = default;
		public CraftingCategoryRecord CategoryCategory 
		{ 
			get 
			{ 
				if(_categoryCategoryRecord == null)
					_categoryCategoryRecord = ModelManager.CraftingCategoryModel.GetRecord(_categoryCategory);
				return _categoryCategoryRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _categoryCategory = CraftingCategoryIdentifier.None;
                else
                    _categoryCategory = value.Identifier;
				_categoryCategoryRecord = null;
			}
		}

		[ColumnName("Name")] [SerializeField] private string _name = default;
		public string Name { get { return _name; } set { if(!CheckEdit()) return; _name = value; }}

		[ColumnName("Input")] [SerializeField] private CraftingInputIdentifier[] _input = default;
		[NonSerialized] private CraftingInputRecord[] _inputRecords = default;
		public CraftingInputRecord[] Input 
		{ 
			get 
			{ 
				if(_inputRecords == null)
				{
					_inputRecords = new CraftingInputRecord[_input.Length];
					for(int i = 0; i < _inputRecords.Length; i++)
						_inputRecords[i] = ModelManager.CraftingInputModel.GetRecord(_input[i]);
				}
				return _inputRecords; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
				CraftingInputIdentifier[] newData = new CraftingInputIdentifier[value.Length];
				for(int i = 0; i < value.Length; i++)
				{
					CraftingInputRecord record = value[i];
					if(record == null)
						newData[i] = CraftingInputIdentifier.None;
					else
						newData[i] = record.Identifier;
				}
				_input = newData;
				_inputRecords = null;
			}
		}

		[ColumnName("Output")] [SerializeField] private CraftingOutputIdentifier[] _output = default;
		[NonSerialized] private CraftingOutputRecord[] _outputRecords = default;
		public CraftingOutputRecord[] Output 
		{ 
			get 
			{ 
				if(_outputRecords == null)
				{
					_outputRecords = new CraftingOutputRecord[_output.Length];
					for(int i = 0; i < _outputRecords.Length; i++)
						_outputRecords[i] = ModelManager.CraftingOutputModel.GetRecord(_output[i]);
				}
				return _outputRecords; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
				CraftingOutputIdentifier[] newData = new CraftingOutputIdentifier[value.Length];
				for(int i = 0; i < value.Length; i++)
				{
					CraftingOutputRecord record = value[i];
					if(record == null)
						newData[i] = CraftingOutputIdentifier.None;
					else
						newData[i] = record.Identifier;
				}
				_output = newData;
				_outputRecords = null;
			}
		}

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

		[ColumnName("Crafting Time")] [SerializeField] private long _craftingTime = default;
		public long CraftingTime { get { return _craftingTime; } set { if(!CheckEdit()) return; _craftingTime = value; }}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public CraftingRecipeModel model { get { return ModelManager.CraftingRecipeModel; } }
        private CraftingRecipeRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            CraftingRecipeRecord editableCopy = new CraftingRecipeRecord();
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

        private void CopyData(CraftingRecipeRecord record)
        {
            record._categoryCategory = _categoryCategory;
            record._name = _name;
            record._input = _input;
            record._output = _output;
            record._icon = _icon;
            record._craftingTime = _craftingTime;
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
