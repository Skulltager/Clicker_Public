using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class ResourceSpawnRecord : BaseRecord<ResourceSpawnIdentifier>
	{
		[ColumnName("Min Distance")] [SerializeField] private int _minDistance = default;
		public int MinDistance { get { return _minDistance; } set { if(!CheckEdit()) return; _minDistance = value; }}

		[ColumnName("Max Distance")] [SerializeField] private int _maxDistance = default;
		public int MaxDistance { get { return _maxDistance; } set { if(!CheckEdit()) return; _maxDistance = value; }}

		[ColumnName("Min Distance Spawn Weight")] [SerializeField] private int _minDistanceSpawnWeight = default;
		public int MinDistanceSpawnWeight { get { return _minDistanceSpawnWeight; } set { if(!CheckEdit()) return; _minDistanceSpawnWeight = value; }}

		[ColumnName("Max Distance Spawn Weight")] [SerializeField] private int _maxDistanceSpawnWeight = default;
		public int MaxDistanceSpawnWeight { get { return _maxDistanceSpawnWeight; } set { if(!CheckEdit()) return; _maxDistanceSpawnWeight = value; }}

		[ColumnName("Min Distance Min Amount")] [SerializeField] private int _minDistanceMinAmount = default;
		public int MinDistanceMinAmount { get { return _minDistanceMinAmount; } set { if(!CheckEdit()) return; _minDistanceMinAmount = value; }}

		[ColumnName("Min Distance Max Amount")] [SerializeField] private int _minDistanceMaxAmount = default;
		public int MinDistanceMaxAmount { get { return _minDistanceMaxAmount; } set { if(!CheckEdit()) return; _minDistanceMaxAmount = value; }}

		[ColumnName("Max Distance Min Amount")] [SerializeField] private int _maxDistanceMinAmount = default;
		public int MaxDistanceMinAmount { get { return _maxDistanceMinAmount; } set { if(!CheckEdit()) return; _maxDistanceMinAmount = value; }}

		[ColumnName("Max Distance Max Amount")] [SerializeField] private int _maxDistanceMaxAmount = default;
		public int MaxDistanceMaxAmount { get { return _maxDistanceMaxAmount; } set { if(!CheckEdit()) return; _maxDistanceMaxAmount = value; }}

		[ColumnName("Distance Amount Factor")] [SerializeField] private float _distanceAmountFactor = default;
		public float DistanceAmountFactor { get { return _distanceAmountFactor; } set { if(!CheckEdit()) return; _distanceAmountFactor = value; }}

		[ColumnName("Resource Type")] [SerializeField] private ResourceTypeIdentifier _resourceType = default;
		[NonSerialized] private ResourceTypeRecord _resourceTypeRecord = default;
		public ResourceTypeRecord ResourceType 
		{ 
			get 
			{ 
				if(_resourceTypeRecord == null)
					_resourceTypeRecord = ModelManager.ResourceTypeModel.GetRecord(_resourceType);
				return _resourceTypeRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _resourceType = ResourceTypeIdentifier.None;
                else
                    _resourceType = value.Identifier;
				_resourceTypeRecord = null;
			}
		}

		[ColumnName("Repawn Timer")] [SerializeField] private int _repawnTimer = default;
		public int RepawnTimer { get { return _repawnTimer; } set { if(!CheckEdit()) return; _repawnTimer = value; }}

		[ColumnName("Tool Required")] [SerializeField] private bool _toolRequired = default;
		public bool ToolRequired { get { return _toolRequired; } set { if(!CheckEdit()) return; _toolRequired = value; }}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public ResourceSpawnModel model { get { return ModelManager.ResourceSpawnModel; } }
        private ResourceSpawnRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            ResourceSpawnRecord editableCopy = new ResourceSpawnRecord();
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

        private void CopyData(ResourceSpawnRecord record)
        {
            record._minDistance = _minDistance;
            record._maxDistance = _maxDistance;
            record._minDistanceSpawnWeight = _minDistanceSpawnWeight;
            record._maxDistanceSpawnWeight = _maxDistanceSpawnWeight;
            record._minDistanceMinAmount = _minDistanceMinAmount;
            record._minDistanceMaxAmount = _minDistanceMaxAmount;
            record._maxDistanceMinAmount = _maxDistanceMinAmount;
            record._maxDistanceMaxAmount = _maxDistanceMaxAmount;
            record._distanceAmountFactor = _distanceAmountFactor;
            record._resourceType = _resourceType;
            record._repawnTimer = _repawnTimer;
            record._toolRequired = _toolRequired;
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
