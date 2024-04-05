using System;
using UnityEngine;
using System.Linq;

namespace SheetCodes
{
	//Generated code, this script is only generated once and will not be overwritted!
	//You can add code to this script to iterate your records but don't remove the generated code!

	[Serializable]
	public class ToolBaseStatsModel : BaseModel<ToolBaseStatsRecord, ToolBaseStatsIdentifier>
	{
		[SerializeField] private ToolBaseStatsRecord[] records = default;
		protected override ToolBaseStatsRecord[] Records { get { return records; } }

		//Add your code below this line
		public ToolBaseStatsRecord GetMatchingRecord(ItemQualityRecord qualityRecord, ItemRecord itemRecord)
		{
			return Array.Find(records, i => i.QualityLevel == qualityRecord && i.Item == itemRecord);
		}
	}
}
