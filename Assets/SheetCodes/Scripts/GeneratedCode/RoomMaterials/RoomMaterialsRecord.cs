using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class RoomMaterialsRecord : BaseRecord<RoomMaterialsIdentifier>
	{
		[ColumnName("Ceiling")] [SerializeField] private CellMaterialIdentifier _ceiling = default;
		[NonSerialized] private CellMaterialRecord _ceilingRecord = default;
		public CellMaterialRecord Ceiling 
		{ 
			get 
			{ 
				if(_ceilingRecord == null)
					_ceilingRecord = ModelManager.CellMaterialModel.GetRecord(_ceiling);
				return _ceilingRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _ceiling = CellMaterialIdentifier.None;
                else
                    _ceiling = value.Identifier;
				_ceilingRecord = null;
			}
		}

		[ColumnName("Walls")] [SerializeField] private CellMaterialIdentifier _walls = default;
		[NonSerialized] private CellMaterialRecord _wallsRecord = default;
		public CellMaterialRecord Walls 
		{ 
			get 
			{ 
				if(_wallsRecord == null)
					_wallsRecord = ModelManager.CellMaterialModel.GetRecord(_walls);
				return _wallsRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _walls = CellMaterialIdentifier.None;
                else
                    _walls = value.Identifier;
				_wallsRecord = null;
			}
		}

		[ColumnName("Floor")] [SerializeField] private CellMaterialIdentifier _floor = default;
		[NonSerialized] private CellMaterialRecord _floorRecord = default;
		public CellMaterialRecord Floor 
		{ 
			get 
			{ 
				if(_floorRecord == null)
					_floorRecord = ModelManager.CellMaterialModel.GetRecord(_floor);
				return _floorRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _floor = CellMaterialIdentifier.None;
                else
                    _floor = value.Identifier;
				_floorRecord = null;
			}
		}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public RoomMaterialsModel model { get { return ModelManager.RoomMaterialsModel; } }
        private RoomMaterialsRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            RoomMaterialsRecord editableCopy = new RoomMaterialsRecord();
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

        private void CopyData(RoomMaterialsRecord record)
        {
            record._ceiling = _ceiling;
            record._walls = _walls;
            record._floor = _floor;
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
