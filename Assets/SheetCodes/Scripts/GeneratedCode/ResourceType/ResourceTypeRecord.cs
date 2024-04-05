using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class ResourceTypeRecord : BaseRecord<ResourceTypeIdentifier>
	{
		[ColumnName("Interactable Tools")] [SerializeField] private ItemCategoryIdentifier _interactableTools = default;
		[NonSerialized] private ItemCategoryRecord _interactableToolsRecord = default;
		public ItemCategoryRecord InteractableTools 
		{ 
			get 
			{ 
				if(_interactableToolsRecord == null)
					_interactableToolsRecord = ModelManager.ItemCategoryModel.GetRecord(_interactableTools);
				return _interactableToolsRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _interactableTools = ItemCategoryIdentifier.None;
                else
                    _interactableTools = value.Identifier;
				_interactableToolsRecord = null;
			}
		}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public ResourceTypeModel model { get { return ModelManager.ResourceTypeModel; } }
        private ResourceTypeRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            ResourceTypeRecord editableCopy = new ResourceTypeRecord();
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

        private void CopyData(ResourceTypeRecord record)
        {
            record._interactableTools = _interactableTools;
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
