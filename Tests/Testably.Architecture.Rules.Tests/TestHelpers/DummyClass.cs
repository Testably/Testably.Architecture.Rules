namespace Testably.Architecture.Rules.Tests.TestHelpers;

// ReSharper disable UnusedMember.Local
internal class DummyClass
{
	public int? DummyProperty { get; set; }
	public int Value { get; }
	#pragma warning disable CS0414
	public int? DummyField = 0;
	#pragma warning restore CS0414

	public DummyClass(int value)
	{
		Value = value;
	}

	#pragma warning disable CS0067
	public event Dummy DummyEvent = null!;
	#pragma warning restore CS0067
	public delegate void Dummy();

	public void DummyMethod() { }
}
