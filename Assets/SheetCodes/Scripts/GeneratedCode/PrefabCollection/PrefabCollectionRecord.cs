using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class PrefabCollectionRecord : BaseRecord<PrefabCollectionIdentifier>
	{
		//Does this type no longer exist? Delete from here..
		[ColumnName("Infinite Chest")] [SerializeField] private InfinityChestVisual _infiniteChest = default;
		public InfinityChestVisual InfiniteChest 
		{ 
			get { return _infiniteChest; } 
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
                _infiniteChest = value;
#endif
            }
        }
		//..To here

		//Does this type no longer exist? Delete from here..
		[ColumnName("Door Visual")] [SerializeField] private DoorVisual _doorVisual = default;
		public DoorVisual DoorVisual 
		{ 
			get { return _doorVisual; } 
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
                _doorVisual = value;
#endif
            }
        }
		//..To here

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public PrefabCollectionModel model { get { return ModelManager.PrefabCollectionModel; } }
        private PrefabCollectionRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            PrefabCollectionRecord editableCopy = new PrefabCollectionRecord();
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

        private void CopyData(PrefabCollectionRecord record)
        {
            record._infiniteChest = _infiniteChest;
            record._doorVisual = _doorVisual;
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
