using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class WorldResourceRecord : BaseRecord<WorldResourceIdentifier>
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

		//Does this type no longer exist? Delete from here..
		[ColumnName("Minimap Icon")] [SerializeField] private UnityEngine.Texture _minimapIcon = default;
		public UnityEngine.Texture MinimapIcon 
		{ 
			get { return _minimapIcon; } 
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
                _minimapIcon = value;
#endif
            }
        }
		//..To here

		[ColumnName("Base Horizontal Scale")] [SerializeField] private float _baseHorizontalScale = default;
		public float BaseHorizontalScale { get { return _baseHorizontalScale; } set { if(!CheckEdit()) return; _baseHorizontalScale = value; }}

		[ColumnName("Base Vertical Scale")] [SerializeField] private float _baseVerticalScale = default;
		public float BaseVerticalScale { get { return _baseVerticalScale; } set { if(!CheckEdit()) return; _baseVerticalScale = value; }}

		[ColumnName("Horizontal Scale Per Unit")] [SerializeField] private float _horizontalScalePerUnit = default;
		public float HorizontalScalePerUnit { get { return _horizontalScalePerUnit; } set { if(!CheckEdit()) return; _horizontalScalePerUnit = value; }}

		[ColumnName("Vertical Scale Per Unit")] [SerializeField] private float _verticalScalePerUnit = default;
		public float VerticalScalePerUnit { get { return _verticalScalePerUnit; } set { if(!CheckEdit()) return; _verticalScalePerUnit = value; }}

		[ColumnName("Max Unit Scaling")] [SerializeField] private int _maxUnitScaling = default;
		public int MaxUnitScaling { get { return _maxUnitScaling; } set { if(!CheckEdit()) return; _maxUnitScaling = value; }}

		[ColumnName("Height Offset")] [SerializeField] private float _heightOffset = default;
		public float HeightOffset { get { return _heightOffset; } set { if(!CheckEdit()) return; _heightOffset = value; }}

		[ColumnName("Health Per Unit")] [SerializeField] private int _healthPerUnit = default;
		public int HealthPerUnit { get { return _healthPerUnit; } set { if(!CheckEdit()) return; _healthPerUnit = value; }}

		[ColumnName("Collider Base Radius")] [SerializeField] private float _colliderBaseRadius = default;
		public float ColliderBaseRadius { get { return _colliderBaseRadius; } set { if(!CheckEdit()) return; _colliderBaseRadius = value; }}

		[ColumnName("Collider Radius Per Unit")] [SerializeField] private float _colliderRadiusPerUnit = default;
		public float ColliderRadiusPerUnit { get { return _colliderRadiusPerUnit; } set { if(!CheckEdit()) return; _colliderRadiusPerUnit = value; }}

		[ColumnName("Name")] [SerializeField] private string _name = default;
		public string Name { get { return _name; } set { if(!CheckEdit()) return; _name = value; }}

		[ColumnName("Item Drops")] [SerializeField] private ItemDropIdentifier[] _itemDrops = default;
		[NonSerialized] private ItemDropRecord[] _itemDropsRecords = default;
		public ItemDropRecord[] ItemDrops 
		{ 
			get 
			{ 
				if(_itemDropsRecords == null)
				{
					_itemDropsRecords = new ItemDropRecord[_itemDrops.Length];
					for(int i = 0; i < _itemDropsRecords.Length; i++)
						_itemDropsRecords[i] = ModelManager.ItemDropModel.GetRecord(_itemDrops[i]);
				}
				return _itemDropsRecords; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
				ItemDropIdentifier[] newData = new ItemDropIdentifier[value.Length];
				for(int i = 0; i < value.Length; i++)
				{
					ItemDropRecord record = value[i];
					if(record == null)
						newData[i] = ItemDropIdentifier.None;
					else
						newData[i] = record.Identifier;
				}
				_itemDrops = newData;
				_itemDropsRecords = null;
			}
		}

		[ColumnName("Resource Spawn")] [SerializeField] private ResourceSpawnIdentifier _resourceSpawn = default;
		[NonSerialized] private ResourceSpawnRecord _resourceSpawnRecord = default;
		public ResourceSpawnRecord ResourceSpawn 
		{ 
			get 
			{ 
				if(_resourceSpawnRecord == null)
					_resourceSpawnRecord = ModelManager.ResourceSpawnModel.GetRecord(_resourceSpawn);
				return _resourceSpawnRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _resourceSpawn = ResourceSpawnIdentifier.None;
                else
                    _resourceSpawn = value.Identifier;
				_resourceSpawnRecord = null;
			}
		}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public WorldResourceModel model { get { return ModelManager.WorldResourceModel; } }
        private WorldResourceRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            WorldResourceRecord editableCopy = new WorldResourceRecord();
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

        private void CopyData(WorldResourceRecord record)
        {
            record._material = _material;
            record._icon = _icon;
            record._minimapIcon = _minimapIcon;
            record._baseHorizontalScale = _baseHorizontalScale;
            record._baseVerticalScale = _baseVerticalScale;
            record._horizontalScalePerUnit = _horizontalScalePerUnit;
            record._verticalScalePerUnit = _verticalScalePerUnit;
            record._maxUnitScaling = _maxUnitScaling;
            record._heightOffset = _heightOffset;
            record._healthPerUnit = _healthPerUnit;
            record._colliderBaseRadius = _colliderBaseRadius;
            record._colliderRadiusPerUnit = _colliderRadiusPerUnit;
            record._name = _name;
            record._itemDrops = _itemDrops;
            record._resourceSpawn = _resourceSpawn;
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
