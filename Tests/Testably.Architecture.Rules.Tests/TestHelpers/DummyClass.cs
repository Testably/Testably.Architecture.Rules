namespace Testably.Architecture.Rules.Tests.TestHelpers;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
internal class DummyClass
{
	public int? DummyProperty1 { get; set; }
	public int? DummyProperty2 { get; set; }
	public int Value { get; }

	#pragma warning disable CS0414
	public int? DummyField1 = 1;
	public int? DummyField2 = 2;
	#pragma warning restore CS0414

	#pragma warning disable CS8618
	public DummyClass(int value)
	{
		Value = value;
	}

	public DummyClass(string otherValue, int value)
	{
		Value = value;
	}
	#pragma warning restore CS8618

	public delegate void Dummy();
	#pragma warning disable CS0067
	public event Dummy DummyEvent1;
	public event Dummy DummyEvent2;
	#pragma warning restore CS0067

	public void DummyMethod1()
	{
		DummyEvent1();
	}

	public void DummyMethod2()
	{
		DummyEvent2();
	}
}
