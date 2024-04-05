using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class RoomRecord : BaseRecord<RoomIdentifier>
	{
		[ColumnName("Min Spawn Distance")] [SerializeField] private int _minSpawnDistance = default;
		public int MinSpawnDistance { get { return _minSpawnDistance; } set { if(!CheckEdit()) return; _minSpawnDistance = value; }}

		[ColumnName("Max Spawn Distance")] [SerializeField] private int _maxSpawnDistance = default;
		public int MaxSpawnDistance { get { return _maxSpawnDistance; } set { if(!CheckEdit()) return; _maxSpawnDistance = value; }}

		[ColumnName(" Min Spawn Distance Weight")] [SerializeField] private int _minSpawnDistanceWeight = default;
		public int MinSpawnDistanceWeight { get { return _minSpawnDistanceWeight; } set { if(!CheckEdit()) return; _minSpawnDistanceWeight = value; }}

		[ColumnName("Max Spawn Distance Weight")] [SerializeField] private int _maxSpawnDistanceWeight = default;
		public int MaxSpawnDistanceWeight { get { return _maxSpawnDistanceWeight; } set { if(!CheckEdit()) return; _maxSpawnDistanceWeight = value; }}

		[ColumnName("Min Biome Distance")] [SerializeField] private int _minBiomeDistance = default;
		public int MinBiomeDistance { get { return _minBiomeDistance; } set { if(!CheckEdit()) return; _minBiomeDistance = value; }}

		[ColumnName("Max Biome Distance")] [SerializeField] private int _maxBiomeDistance = default;
		public int MaxBiomeDistance { get { return _maxBiomeDistance; } set { if(!CheckEdit()) return; _maxBiomeDistance = value; }}

		[ColumnName("Min Room Size")] [SerializeField] private int _minRoomSize = default;
		public int MinRoomSize { get { return _minRoomSize; } set { if(!CheckEdit()) return; _minRoomSize = value; }}

		[ColumnName("Max Room Size")] [SerializeField] private int _maxRoomSize = default;
		public int MaxRoomSize { get { return _maxRoomSize; } set { if(!CheckEdit()) return; _maxRoomSize = value; }}

		[ColumnName("Min Doors")] [SerializeField] private int _minDoors = default;
		public int MinDoors { get { return _minDoors; } set { if(!CheckEdit()) return; _minDoors = value; }}

		[ColumnName("Max Doors")] [SerializeField] private int _maxDoors = default;
		public int MaxDoors { get { return _maxDoors; } set { if(!CheckEdit()) return; _maxDoors = value; }}

		[ColumnName("Min Resource Spawns")] [SerializeField] private int _minResourceSpawns = default;
		public int MinResourceSpawns { get { return _minResourceSpawns; } set { if(!CheckEdit()) return; _minResourceSpawns = value; }}

		[ColumnName("Max Resource Spawns")] [SerializeField] private int _maxResourceSpawns = default;
		public int MaxResourceSpawns { get { return _maxResourceSpawns; } set { if(!CheckEdit()) return; _maxResourceSpawns = value; }}

		[ColumnName("Resource Spawn Count Multiplier")] [SerializeField] private float _resourceSpawnCountMultiplier = default;
		public float ResourceSpawnCountMultiplier { get { return _resourceSpawnCountMultiplier; } set { if(!CheckEdit()) return; _resourceSpawnCountMultiplier = value; }}

		[ColumnName("Resource Spawn Weights")] [SerializeField] private ResourceSpawnWeightIdentifier[] _resourceSpawnWeights = default;
		[NonSerialized] private ResourceSpawnWeightRecord[] _resourceSpawnWeightsRecords = default;
		public ResourceSpawnWeightRecord[] ResourceSpawnWeights 
		{ 
			get 
			{ 
				if(_resourceSpawnWeightsRecords == null)
				{
					_resourceSpawnWeightsRecords = new ResourceSpawnWeightRecord[_resourceSpawnWeights.Length];
					for(int i = 0; i < _resourceSpawnWeightsRecords.Length; i++)
						_resourceSpawnWeightsRecords[i] = ModelManager.ResourceSpawnWeightModel.GetRecord(_resourceSpawnWeights[i]);
				}
				return _resourceSpawnWeightsRecords; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
				ResourceSpawnWeightIdentifier[] newData = new ResourceSpawnWeightIdentifier[value.Length];
				for(int i = 0; i < value.Length; i++)
				{
					ResourceSpawnWeightRecord record = value[i];
					if(record == null)
						newData[i] = ResourceSpawnWeightIdentifier.None;
					else
						newData[i] = record.Identifier;
				}
				_resourceSpawnWeights = newData;
				_resourceSpawnWeightsRecords = null;
			}
		}

		[ColumnName("Room Materials")] [SerializeField] private RoomMaterialsIdentifier _roomMaterials = default;
		[NonSerialized] private RoomMaterialsRecord _roomMaterialsRecord = default;
		public RoomMaterialsRecord RoomMaterials 
		{ 
			get 
			{ 
				if(_roomMaterialsRecord == null)
					_roomMaterialsRecord = ModelManager.RoomMaterialsModel.GetRecord(_roomMaterials);
				return _roomMaterialsRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _roomMaterials = RoomMaterialsIdentifier.None;
                else
                    _roomMaterials = value.Identifier;
				_roomMaterialsRecord = null;
			}
		}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public RoomModel model { get { return ModelManager.RoomModel; } }
        private RoomRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            RoomRecord editableCopy = new RoomRecord();
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

        private void CopyData(RoomRecord record)
        {
            record._minSpawnDistance = _minSpawnDistance;
            record._maxSpawnDistance = _maxSpawnDistance;
            record._minSpawnDistanceWeight = _minSpawnDistanceWeight;
            record._maxSpawnDistanceWeight = _maxSpawnDistanceWeight;
            record._minBiomeDistance = _minBiomeDistance;
            record._maxBiomeDistance = _maxBiomeDistance;
            record._minRoomSize = _minRoomSize;
            record._maxRoomSize = _maxRoomSize;
            record._minDoors = _minDoors;
            record._maxDoors = _maxDoors;
            record._minResourceSpawns = _minResourceSpawns;
            record._maxResourceSpawns = _maxResourceSpawns;
            record._resourceSpawnCountMultiplier = _resourceSpawnCountMultiplier;
            record._resourceSpawnWeights = _resourceSpawnWeights;
            record._roomMaterials = _roomMaterials;
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
