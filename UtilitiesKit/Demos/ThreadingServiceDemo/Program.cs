using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilitiesKit.Threading;

namespace ThreadingServiceDemo
{
	public class TestServiceHandler : ServiceTaskHandler<int, bool> { }

	public class TestService : ServiceBase<TestServiceHandler, int, bool>
	{
		public TestService()
			: base(3)
		{
		}

		protected override bool OnDoTask(TestServiceHandler handler)
		{
			Random random = new Random();

			Console.WriteLine("Task started: " + Thread.CurrentThread.ManagedThreadId + " " + handler.Argument);
			int sleepTime = random.Next(500, 3000);
			Thread.Sleep(sleepTime);
			Console.WriteLine("Task finished: " + Thread.CurrentThread.ManagedThreadId + " " + handler.Argument);
			return true;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			TestService service = new TestService();

			for (int index = 0; index < 10; index++ )
			{
				service.Execute(index);
			}

			Console.WriteLine("Waiting for finish.");
			
			// Stopping code.
			//Thread.Sleep(1000);
			//service.Stop();

			service.WaitForAllFinished();

			Console.WriteLine("Finished. Press any key to continue.");

			Console.ReadKey();
		}
	}
}
