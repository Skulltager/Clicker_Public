using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class BiomeRecord : BaseRecord<BiomeIdentifier>
	{
		[ColumnName("Distance To center")] [SerializeField] private int _distanceToCenter = default;
		public int DistanceToCenter { get { return _distanceToCenter; } set { if(!CheckEdit()) return; _distanceToCenter = value; }}

		[ColumnName("Clamp Strength")] [SerializeField] private float _clampStrength = default;
		public float ClampStrength { get { return _clampStrength; } set { if(!CheckEdit()) return; _clampStrength = value; }}

		[ColumnName("Scale Strength")] [SerializeField] private float _scaleStrength = default;
		public float ScaleStrength { get { return _scaleStrength; } set { if(!CheckEdit()) return; _scaleStrength = value; }}

		[ColumnName("Elevation Strength")] [SerializeField] private float _elevationStrength = default;
		public float ElevationStrength { get { return _elevationStrength; } set { if(!CheckEdit()) return; _elevationStrength = value; }}

		[ColumnName("Layers")] [SerializeField] private BiomeLayerIdentifier[] _layers = default;
		[NonSerialized] private BiomeLayerRecord[] _layersRecords = default;
		public BiomeLayerRecord[] Layers 
		{ 
			get 
			{ 
				if(_layersRecords == null)
				{
					_layersRecords = new BiomeLayerRecord[_layers.Length];
					for(int i = 0; i < _layersRecords.Length; i++)
						_layersRecords[i] = ModelManager.BiomeLayerModel.GetRecord(_layers[i]);
				}
				return _layersRecords; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
				BiomeLayerIdentifier[] newData = new BiomeLayerIdentifier[value.Length];
				for(int i = 0; i < value.Length; i++)
				{
					BiomeLayerRecord record = value[i];
					if(record == null)
						newData[i] = BiomeLayerIdentifier.None;
					else
						newData[i] = record.Identifier;
				}
				_layers = newData;
				_layersRecords = null;
			}
		}

		[ColumnName("Rooms")] [SerializeField] private RoomIdentifier[] _rooms = default;
		[NonSerialized] private RoomRecord[] _roomsRecords = default;
		public RoomRecord[] Rooms 
		{ 
			get 
			{ 
				if(_roomsRecords == null)
				{
					_roomsRecords = new RoomRecord[_rooms.Length];
					for(int i = 0; i < _roomsRecords.Length; i++)
						_roomsRecords[i] = ModelManager.RoomModel.GetRecord(_rooms[i]);
				}
				return _roomsRecords; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
				RoomIdentifier[] newData = new RoomIdentifier[value.Length];
				for(int i = 0; i < value.Length; i++)
				{
					RoomRecord record = value[i];
					if(record == null)
						newData[i] = RoomIdentifier.None;
					else
						newData[i] = record.Identifier;
				}
				_rooms = newData;
				_roomsRecords = null;
			}
		}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public BiomeModel model { get { return ModelManager.BiomeModel; } }
        private BiomeRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            BiomeRecord editableCopy = new BiomeRecord();
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

        private void CopyData(BiomeRecord record)
        {
            record._distanceToCenter = _distanceToCenter;
            record._clampStrength = _clampStrength;
            record._scaleStrength = _scaleStrength;
            record._elevationStrength = _elevationStrength;
            record._layers = _layers;
            record._rooms = _rooms;
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
