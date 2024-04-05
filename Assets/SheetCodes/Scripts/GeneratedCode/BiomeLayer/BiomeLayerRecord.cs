using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class BiomeLayerRecord : BaseRecord<BiomeLayerIdentifier>
	{
		[ColumnName("Size Factor")] [SerializeField] private float _sizeFactor = default;
		public float SizeFactor { get { return _sizeFactor; } set { if(!CheckEdit()) return; _sizeFactor = value; }}

		[ColumnName("Roughness")] [SerializeField] private float _roughness = default;
		public float Roughness { get { return _roughness; } set { if(!CheckEdit()) return; _roughness = value; }}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public BiomeLayerModel model { get { return ModelManager.BiomeLayerModel; } }
        private BiomeLayerRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            BiomeLayerRecord editableCopy = new BiomeLayerRecord();
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

        private void CopyData(BiomeLayerRecord record)
        {
            record._sizeFactor = _sizeFactor;
            record._roughness = _roughness;
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
