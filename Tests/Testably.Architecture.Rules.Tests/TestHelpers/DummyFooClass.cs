namespace Testably.Architecture.Rules.Tests.TestHelpers;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
internal class DummyFooClass
{
	public int? DummyFooProperty1 { get; set; }
	public int? DummyFooProperty2 { get; set; }
	public int Value { get; }

	public delegate void DummyFoo();

	public void DummyFooMethod1()
	{
		DummyFooEvent1();
	}

	public void DummyFooMethod2()
	{
		DummyFooEvent2();
	}

	#pragma warning disable CS0414
	public int? DummyFooField1 = 1;
	public int? DummyFooField2 = 2;
	#pragma warning restore CS0414

	#pragma warning disable CS8618
	public DummyFooClass(int value)
	{
		Value = value;
	}

	public DummyFooClass(string otherValue, int value)
	{
		Value = value;
	}
	#pragma warning restore CS8618

	#pragma warning disable CS0067
	public event DummyFoo DummyFooEvent1;
	public event DummyFoo DummyFooEvent2;
	#pragma warning restore CS0067
}
