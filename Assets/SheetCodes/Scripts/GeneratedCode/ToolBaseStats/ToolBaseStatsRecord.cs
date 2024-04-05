using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class ToolBaseStatsRecord : BaseRecord<ToolBaseStatsIdentifier>
	{
		[ColumnName("Quality Level")] [SerializeField] private ItemQualityIdentifier _qualityLevel = default;
		[NonSerialized] private ItemQualityRecord _qualityLevelRecord = default;
		public ItemQualityRecord QualityLevel 
		{ 
			get 
			{ 
				if(_qualityLevelRecord == null)
					_qualityLevelRecord = ModelManager.ItemQualityModel.GetRecord(_qualityLevel);
				return _qualityLevelRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _qualityLevel = ItemQualityIdentifier.None;
                else
                    _qualityLevel = value.Identifier;
				_qualityLevelRecord = null;
			}
		}

		[ColumnName("Item")] [SerializeField] private ItemIdentifier _item = default;
		[NonSerialized] private ItemRecord _itemRecord = default;
		public ItemRecord Item 
		{ 
			get 
			{ 
				if(_itemRecord == null)
					_itemRecord = ModelManager.ItemModel.GetRecord(_item);
				return _itemRecord; 
			} 
			set
			{
				if(!CheckEdit())
					return;
					
                if (value == null)
                    _item = ItemIdentifier.None;
                else
                    _item = value.Identifier;
				_itemRecord = null;
			}
		}

		[ColumnName("Durability")] [SerializeField] private int _durability = default;
		public int Durability { get { return _durability; } set { if(!CheckEdit()) return; _durability = value; }}

		[ColumnName("Damage")] [SerializeField] private int _damage = default;
		public int Damage { get { return _damage; } set { if(!CheckEdit()) return; _damage = value; }}

		[ColumnName("Item Drop Rate Multiplier")] [SerializeField] private float _itemDropRateMultiplier = default;
		public float ItemDropRateMultiplier { get { return _itemDropRateMultiplier; } set { if(!CheckEdit()) return; _itemDropRateMultiplier = value; }}

		[ColumnName("Prospecting")] [SerializeField] private int _prospecting = default;
		public int Prospecting { get { return _prospecting; } set { if(!CheckEdit()) return; _prospecting = value; }}

		[ColumnName("Token Drop Rate Multiplier")] [SerializeField] private float _tokenDropRateMultiplier = default;
		public float TokenDropRateMultiplier { get { return _tokenDropRateMultiplier; } set { if(!CheckEdit()) return; _tokenDropRateMultiplier = value; }}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public ToolBaseStatsModel model { get { return ModelManager.ToolBaseStatsModel; } }
        private ToolBaseStatsRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            ToolBaseStatsRecord editableCopy = new ToolBaseStatsRecord();
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

        private void CopyData(ToolBaseStatsRecord record)
        {
            record._qualityLevel = _qualityLevel;
            record._item = _item;
            record._durability = _durability;
            record._damage = _damage;
            record._itemDropRateMultiplier = _itemDropRateMultiplier;
            record._prospecting = _prospecting;
            record._tokenDropRateMultiplier = _tokenDropRateMultiplier;
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
