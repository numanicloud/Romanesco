using Romanesco.Annotations;

namespace Romanesco.Sample
{
	[EditorSubtypingBase]
	public class BaseType
	{
		[EditorMember]
		public int BaseField { get; set; }

		public override string ToString() => $"BaseType; {BaseField}:int";
	}

	[EditorSubtypeName("パワフル型")]
	public class PowerfulType : BaseType
	{
		[EditorMember]
		public float PowerfulField { get; set; }

		public override string ToString() => $"PowerfulField; {PowerfulField}:float\n{base.ToString()}";
	}

	[EditorSubtypeName("インテリジェント型")]
	public class IntelligentType : BaseType
	{
		[EditorMember]
		public string IntelligentField { get; set; } = "";

		public override string ToString() => $"IntelligentType; {IntelligentField}:string\n{base.ToString()}";
	}

	public class SubtypingMain
	{
#nullable disable
		[EditorMember]
		public BaseType Value1 { get; set; }
		[EditorMember]
		public BaseType Value2 { get; set; }
		[EditorMember]
		public BaseType Value3 { get; set; }
#nullable restore
	}

	[EditorProject]
	public class SubtypingProject
	{
#nullable disable
		[EditorMember("Main")]
		public SubtypingMain Main { get; set; }
#nullable restore
	}
}
