namespace Testably.Architecture.Rules.Tests.TestHelpers;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
internal class DummyClass
{
	public int? DummyProperty { get; set; }
	public int Value { get; }
	#pragma warning disable CS0414
	public int? DummyField = 0;
	#pragma warning restore CS0414

	#pragma warning disable CS8618
	public DummyClass(int value)
	{
		Value = value;
	}
	#pragma warning restore CS8618

	#pragma warning disable CS0067
	public event Dummy DummyEvent;
	#pragma warning restore CS0067
	public delegate void Dummy();

	public void DummyMethod()
	{
		DummyEvent();
	}
}
