namespace Testably.Architecture.Rules.Tests.TestHelpers;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
[DummyBar]
internal class DummyBarClass : DummyBarBase
{
	public int? DummyBarProperty1 { get; set; }
	public int? DummyBarProperty2 { get; set; }

	public delegate void DummyBar();

	public void DummyBarMethod1()
	{
		DummyBarEvent1();
	}

	public void DummyBarMethod2()
	{
		DummyBarEvent2();
	}

	#pragma warning disable CS0414
	public int? DummyBarField1 = 1;
	public int? DummyBarField2 = 2;
	#pragma warning restore CS0414

	#pragma warning disable CS8618
	public DummyBarClass(int value)
		: base(value)
	{
	}

	public DummyBarClass(string otherValue, int value)
		: base(value)
	{
	}
	#pragma warning restore CS8618

	#pragma warning disable CS0067
	public event DummyBar DummyBarEvent1;
	public event DummyBar DummyBarEvent2;
	#pragma warning restore CS0067
}
