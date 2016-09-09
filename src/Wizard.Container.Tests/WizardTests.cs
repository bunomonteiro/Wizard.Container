using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wizard.Container.Tests.Support;

namespace Wizard.Container.Tests
{
	[TestClass] public sealed class WizardTests
	{
		#region Config

		private const string InstanceName = "magic",
		                     Name = "name";

		private const int TimePerTable = 15;
		private Wizard _wizard;

		[TestInitialize] public void Setup()
		{
			this._wizard = new Wizard();
			this._wizard.Register<Magic>(args => new TableHopping(Name, TimePerTable));
			this._wizard.Register<Magic, TableHopping>(InstanceName);
		}

		[TestCleanup] public void TearDown() { this._wizard.UnregisterAll(); }

		#endregion

		#region Tests

		[TestMethod] public void Checking_A_Registered_Magic_Using_Type_Test()
		{
			// act
			var isRegistered = this._wizard.IsRegistered(typeof (Magic));

			// assert
			Assert.IsTrue(isRegistered);
		}

		[TestMethod] public void Checking_A_Registered_Magic_Using_Generic_Test()
		{
			// act
			var isRegistered = this._wizard.IsRegistered<Magic>();

			// assert
			Assert.IsTrue(isRegistered);
		}

		[TestMethod] public void Checking_A_Registered_Magic_Using_Type_And_And_Named_Instance_Test()
		{
			// act
			var isRegistered = this._wizard.IsRegistered(typeof (Magic), InstanceName);

			// assert
			Assert.IsTrue(isRegistered);
		}

		[TestMethod] public void Checking_A_Registered_Magic_Using_Generic_And_And_Named_Instance_Test()
		{
			// act
			var isRegistered = this._wizard.IsRegistered<Magic>(InstanceName);

			// assert
			Assert.IsTrue(isRegistered);
		}

		[TestMethod] public void Registering_A_Magic_Using_One_Type_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register(typeof (Magic));

			// act
			var isRegistered = this._wizard.IsRegistered(typeof (Magic));

			// assert
			Assert.IsTrue(isRegistered);
		}

		[TestMethod] public void Registering_A_Magic_Using_One_Type_And_Named_Instance_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register(typeof (Magic), InstanceName);

			// act
			var isRegistered = this._wizard.IsRegistered(typeof (Magic), InstanceName);

			// assert
			Assert.IsTrue(isRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Registering_A_Magic_Using_One_Generic_Type_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register<Magic>();

			// act
			var isRegistered = this._wizard.IsRegistered<Magic>();

			// assert
			Assert.IsTrue(isRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Registering_A_Magic_Using_One_Generic_Type_And_Named_Instance_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register<Magic>(InstanceName);

			// act
			var isRegistered = this._wizard.IsRegistered<Magic>(InstanceName);

			// assert
			Assert.IsTrue(isRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Registering_A_Magic_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register(typeof (Magic), typeof (TableHopping));

			// act
			var isRegistered = this._wizard.IsRegistered(typeof (Magic));

			// assert
			Assert.IsTrue(isRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Registering_A_Magic_Using_Generics_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register<Magic, TableHopping>();

			// act
			var isRegistered = this._wizard.IsRegistered<Magic>();

			// assert
			Assert.IsTrue(isRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Registering_A_Magic_Using_Generics_And_Named_Instance_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register<Magic, TableHopping>(InstanceName);

			// act
			var isRegistered = this._wizard.IsRegistered<Magic>(InstanceName);

			// assert
			Assert.IsTrue(isRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Registering_A_Magic_Using_Delegate_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register(typeof (Magic), args => new TableHopping(Name, TimePerTable));

			// act
			var isRegistered = this._wizard.IsRegistered(typeof (Magic));

			// assert
			Assert.IsTrue(isRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Registering_A_Magic_Using_Delegate_And_Generics_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register<Magic>(args => new TableHopping(Name, TimePerTable));

			// act
			var isRegistered = this._wizard.IsRegistered<Magic>();

			// assert
			Assert.IsTrue(isRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Unregistering_A_Magic_Test()
		{
			// act
			this._wizard.Unregister(typeof (Magic));
			var isRegistered = this._wizard.IsRegistered(typeof (Magic));

			// assert
			Assert.IsFalse(isRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Unregistering_A_Magic_Using_Named_Instance_Test()
		{
			// act
			this._wizard.Unregister(typeof (Magic), InstanceName);
			var isRegistered = this._wizard.IsRegistered(typeof (Magic));
			var isNamedInstanceRegistered = this._wizard.IsRegistered(typeof (Magic), InstanceName);

			// assert
			Assert.IsTrue(isRegistered);
			Assert.IsFalse(isNamedInstanceRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Unregistering_A_Magic_Using_Generics_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register<Magic, TableHopping>();

			// act
			this._wizard.Unregister<Magic>();
			var isRegistered = this._wizard.IsRegistered<Magic>();

			// assert
			Assert.IsFalse(isRegistered);
			Assert.AreEqual(0, this._wizard.Count);
		}

		[TestMethod] public void Unregistering_A_Magic_Using_Generics_And_Named_Instance_Test()
		{
			// act
			this._wizard.Unregister<Magic>(InstanceName);
			var isRegistered = this._wizard.IsRegistered<Magic>();
			var isNamedInstanceRegistered = this._wizard.IsRegistered<Magic>(InstanceName);

			// assert
			Assert.IsTrue(isRegistered);
			Assert.IsFalse(isNamedInstanceRegistered);
			Assert.AreEqual(1, this._wizard.Count);
		}

		[TestMethod] public void Creating_A_Magic_Test()
		{
			// act
			var magic = this._wizard.Create(typeof (Magic));

			// assert
			Assert.IsInstanceOfType(magic, typeof (Magic));
			Assert.AreEqual(2, this._wizard.Count);
		}

		[TestMethod] public void Creating_A_Magic_Using_Generics_Test()
		{
			// act
			var magic = this._wizard.Create<Magic>();

			// assert
			Assert.IsInstanceOfType(magic, typeof (Magic));
			Assert.AreEqual(2, this._wizard.Count);
		}

		[TestMethod] public void Creating_A_Magic_Using_Generics_And_Named_Instance_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			const string instanceName2 = "magic2";
			this._wizard.Register<Magic>(args => new TableHopping(Name, TimePerTable), instanceName2);

			// act
			var magic = this._wizard.Create<Magic>(instanceName2);

			// assert
			Assert.IsInstanceOfType(magic, typeof (TableHopping));
			Assert.AreEqual(1, this._wizard.Count);
			Assert.AreEqual(((TableHopping) magic).Name, Name);
			Assert.AreEqual(((TableHopping) magic).TimePerTable, TimePerTable);
		}

		[TestMethod] public void Creating_A_Magic_Using_Arguments_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register(typeof (Magic), args => new TableHopping(args.Name, args.TimePerTable));
			var nameRandom = Guid.NewGuid().ToString();
			var timeRandom = new Random().Next(60);

			// act
			var magic = this._wizard.Create(typeof (Magic), new
			{
				Name = nameRandom,
				TimePerTable = timeRandom
			});

			// assert
			Assert.IsInstanceOfType(magic, typeof (TableHopping));
			Assert.AreEqual(1, this._wizard.Count);
			Assert.AreEqual(((TableHopping) magic).Name, nameRandom);
			Assert.AreEqual(((TableHopping) magic).TimePerTable, timeRandom);
		}

		[TestMethod] public void Creating_A_Magic_Using_Arguments_And_Named_Instance_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			const string instanceName2 = "magic2";
			this._wizard.Register(typeof (Magic), args => new TableHopping(args.Name, args.TimePerTable), instanceName2);
			var nameRandom = Guid.NewGuid().ToString();
			var timeRandom = new Random().Next(60);

			// act
			var magic = this._wizard.Create(typeof (Magic), new
			{
				Name = nameRandom,
				TimePerTable = timeRandom
			}, instanceName2);

			// assert
			Assert.IsInstanceOfType(magic, typeof (TableHopping));
			Assert.AreEqual(1, this._wizard.Count);
			Assert.AreEqual(((TableHopping) magic).Name, nameRandom);
			Assert.AreEqual(((TableHopping) magic).TimePerTable, timeRandom);
		}

		[TestMethod] public void Creating_A_Magic_Using_Generics_And_Arguments_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			this._wizard.Register<Magic>(args => new TableHopping(args.Name, args.TimePerTable));
			var nameRandom = Guid.NewGuid().ToString();
			var timeRandom = new Random().Next(60);

			// act
			var magic = this._wizard.Create<Magic>(new
			{
				Name = nameRandom,
				TimePerTable = timeRandom
			});

			// assert
			Assert.IsInstanceOfType(magic, typeof (TableHopping));
			Assert.AreEqual(1, this._wizard.Count);
			Assert.AreEqual(magic.Name, nameRandom);
			Assert.AreEqual(((TableHopping) magic).TimePerTable, timeRandom);
		}

		[TestMethod] public void Creating_A_Magic_Using_Generics_And_Arguments_And_Named_Instance_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			const string instanceName2 = "magic2";
			this._wizard.Register<Magic>(args => new TableHopping(args.Name, args.TimePerTable), instanceName2);
			var nameRandom = Guid.NewGuid().ToString();
			var timeRandom = new Random().Next(60);

			// act
			var magic = this._wizard.Create<Magic>(new
			{
				Name = nameRandom,
				TimePerTable = timeRandom
			}, instanceName2);

			// assert
			Assert.IsInstanceOfType(magic, typeof (TableHopping));
			Assert.AreEqual(1, this._wizard.Count);
			Assert.AreEqual(((TableHopping) magic).Name, nameRandom);
			Assert.AreEqual(((TableHopping) magic).TimePerTable, timeRandom);
		}

		[TestMethod, ExpectedException(typeof (InvalidOperationException))] public void
			Trying_To_Create_An_Unregistered_Magic_Test()
		{
			// act
			this._wizard.Create(typeof (object));
		}

		[TestMethod] public void Registering_Magics_In_Multithread_Context_Test()
		{
			// arrange
			this._wizard.UnregisterAll();
			var counter = 0;
			var threads = new List<Thread>();

			for (var i = 0; i < 100; i++)
			{
				threads.Add(new Thread(() =>
				{
					++counter;
					if (counter%2 == 0)
					{
						this._wizard.Register<Magic>();
					}
					else
					{
						this._wizard.Register<TableHopping>();
					}
				}));
			}

			// act
			Parallel.ForEach(threads, t => t.Start());
			var isMagicRegistered = this._wizard.IsRegistered<Magic>();
			var isTableHoppingRegistered = this._wizard.IsRegistered<Magic>();

			// assert
			Assert.IsTrue(isMagicRegistered);
			Assert.IsTrue(isTableHoppingRegistered);
			Assert.AreEqual(2, this._wizard.Count);

			// out
			for (var i = 0; i < 100; i++)
			{
				threads[i].Abort();
			}
			threads.Clear();
		}

		#endregion
	}
}