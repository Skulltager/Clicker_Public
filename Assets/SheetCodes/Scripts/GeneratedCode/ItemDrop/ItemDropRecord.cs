using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class ItemDropRecord : BaseRecord<ItemDropIdentifier>
	{
		[ColumnName("Drop Chance")] [SerializeField] private float _dropChance = default;
		public float DropChance { get { return _dropChance; } set { if(!CheckEdit()) return; _dropChance = value; }}

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

		[ColumnName("Min Amount")] [SerializeField] private int _minAmount = default;
		public int MinAmount { get { return _minAmount; } set { if(!CheckEdit()) return; _minAmount = value; }}

		[ColumnName("Max Amount")] [SerializeField] private int _maxAmount = default;
		public int MaxAmount { get { return _maxAmount; } set { if(!CheckEdit()) return; _maxAmount = value; }}

		[ColumnName("Prospecting Requirement")] [SerializeField] private int _prospectingRequirement = default;
		public int ProspectingRequirement { get { return _prospectingRequirement; } set { if(!CheckEdit()) return; _prospectingRequirement = value; }}

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public ItemDropModel model { get { return ModelManager.ItemDropModel; } }
        private ItemDropRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            ItemDropRecord editableCopy = new ItemDropRecord();
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

        private void CopyData(ItemDropRecord record)
        {
            record._dropChance = _dropChance;
            record._item = _item;
            record._minAmount = _minAmount;
            record._maxAmount = _maxAmount;
            record._prospectingRequirement = _prospectingRequirement;
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
