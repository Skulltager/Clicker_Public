using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SheetCodes
{
	//Generated code, do not edit!

	[Serializable]
	public class CraftingInputRecord : BaseRecord<CraftingInputIdentifier>
	{
		[ColumnName("Amount")] [SerializeField] private int _amount = default;
		public int Amount { get { return _amount; } set { if(!CheckEdit()) return; _amount = value; }}

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

        protected bool runtimeEditingEnabled { get { return originalRecord != null; } }
        public CraftingInputModel model { get { return ModelManager.CraftingInputModel; } }
        private CraftingInputRecord originalRecord = default;

        public override void CreateEditableCopy()
        {
#if UNITY_EDITOR
            if (runtimeEditingEnabled)
                return;

            CraftingInputRecord editableCopy = new CraftingInputRecord();
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

        private void CopyData(CraftingInputRecord record)
        {
            record._amount = _amount;
            record._item = _item;
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
