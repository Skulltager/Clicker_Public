using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class ResourceSpawnWeightRecord : BaseRecord<ResourceSpawnWeightIdentifier>
	{
		[ColumnName("Weight")] [SerializeField] private int _weight = default;
		public int Weight { get { return _weight; } set { if(!CheckEdit()) return; _weight = value; }}

		[ColumnName("Type")] [SerializeField] private ResourceTypeIdentifier _type = default;
		[NonSerialized] private ResourceTypeRecord _typeRecord = default;
		public ResourceTypeRecord Type 
		{ 
			get 
			{ 
				if(_typeRecord == null)
					_typeRecord = ModelManager.ResourceTypeModel.GetRecord(_type);
				return _typeRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _type = ResourceTypeIdentifier.None;
                else
                    _type = value.Identifier;
				_typeRecord = null;
			}
		}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public ResourceSpawnWeightModel model { get { return ModelManager.ResourceSpawnWeightModel; } }
        private ResourceSpawnWeightRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            ResourceSpawnWeightRecord editableCopy = new ResourceSpawnWeightRecord();
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

        private void CopyData(ResourceSpawnWeightRecord record)
        {
            record._weight = _weight;
            record._type = _type;
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
