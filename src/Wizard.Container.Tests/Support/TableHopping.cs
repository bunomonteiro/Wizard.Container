namespace Wizard.Container.Tests.Support
{
	public class TableHopping : Magic
	{
		public TableHopping() { }
		public TableHopping(string name, int timePerTable) : base(name) { this.TimePerTable = timePerTable; }
		public TableHopping(int timePerTable) : this(null, timePerTable) { }
		public int TimePerTable { get; set; }
	}
}