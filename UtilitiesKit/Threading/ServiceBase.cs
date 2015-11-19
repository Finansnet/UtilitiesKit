namespace UtilitiesKit.Threading
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	public interface IServiceFacade
	{
		string ServiceName { get; }
		List<string> RunningTasks { get; }
		List<string> QueueTasks { get; }
	}

	public abstract class ServiceBase<THandler, TInput, TOutput> : IServiceFacade where THandler : ServiceTaskHandler<TInput, TOutput>, new()
	{
		private class TaskArgument
		{
			public THandler Handler { get; private set; }
			public Task Task { get; set; }

			public TaskArgument(THandler handler)
			{
				Handler = handler;
			}
		}

		public event EventHandler<THandler> TaskFinished;

		private readonly int _MaxThreadsCount;
		private readonly bool _DebugMode;

		private readonly List<THandler> _Tasks = new List<THandler>();
		private readonly Queue<THandler> _Queue = new Queue<THandler>();
		private readonly List<Task> _Threads = new List<Task>();

		private readonly object _Lock = new object();
		private readonly AutoResetEvent _TaskEnqueuedSemaphore = new AutoResetEvent(false);
		private readonly AutoResetEvent _TaskFinishedSemaphore = new AutoResetEvent(false);

		public virtual string ServiceName { get { return GetType().Name; } }

		public List<string> RunningTasks
		{
			get
			{
				lock (_Lock) { return _Tasks.Select(item => item.Description).ToList(); }
			}
		}

		public List<string> QueueTasks
		{
			get
			{
				lock (_Lock) { return _Queue.Select(item => item.Description).ToList(); }
			}
		}

		public bool IsClosing
		{
			get { return _IsClosing; }
		}
		private volatile bool _IsClosing;

		/// <summary>
		/// Class constructor.
		/// </summary>
		protected ServiceBase(int threadsCount, bool debugMode = false)
		{
			if (threadsCount <= 0)
				throw new ArgumentException("threadsCount cannot be less than 1");

			_DebugMode = debugMode;
			_MaxThreadsCount = threadsCount;
		}

		#region Protected methods

		protected virtual void HandleException(Exception exception) { }

		protected abstract TOutput OnDoTask(THandler handler);

		#endregion

		#region Public methods

		/// <summary>
		/// Stops execution of all tasks.
		/// </summary>
		public void Stop()
		{
			lock (_Lock)
			{
				_IsClosing = true;
				foreach (THandler handler in _Queue)
					handler.Interrupt();
				_Queue.Clear();
			}
		}

		/// <summary>
		/// Stops execution until all tasks pending and waiting in the queue are done.
		/// </summary>
		public void WaitForAllFinished()
		{
			bool wait = true;
			while (wait)
			{
				lock (_Lock)
				{
					wait = _Queue.Count > 0 || _Tasks.Count > 0;
				}

				if (wait)
				{
					_TaskEnqueuedSemaphore.Set();
					_TaskFinishedSemaphore.WaitOne();
				}
			}
		}

		/// <summary>
		/// Starts execution of the task.
		/// Returns null handler when service closing is pending.
		/// </summary>
		/// <param name="argument"></param>
		/// <returns>Task handler or null if service is closing.</returns>
		public THandler Execute(TInput argument)
		{
			THandler handler = new THandler();
			handler.Argument = argument;

			lock (_Lock)
			{
				_Queue.Enqueue(handler);
			}

			TryStartNextTask();
			return handler;
		}

		#endregion

		#region Private helpers

		private void TryStartNextTask()
		{
			lock (_Lock)
			{
				if (_IsClosing)
					return;

				THandler handler = null;
				if (_Queue.Count > 0 && _Tasks.Count < _MaxThreadsCount)
				{
					handler = _Queue.Dequeue();
					_Tasks.Add(handler);

					ThreadPool.QueueUserWorkItem(TaskMethod, handler);
				}
			}
		}

		private void TaskMethod(object argument)
		{
			THandler handler = (THandler)argument;

			TaskArgument taskArgument = new TaskArgument(handler);
			if (_DebugMode)
				DoJobWithDebug(taskArgument);
			else
				DoJob(taskArgument);

			EventHandler<THandler> eventHandler = TaskFinished;
			if (eventHandler != null)
				eventHandler(this, handler);

			lock (_Lock)
			{
				_Tasks.Remove(handler);
			}

			TryStartNextTask();
			_TaskFinishedSemaphore.Set();
		}

		private void DoJob(TaskArgument taskArgument)
		{
			try
			{
				TOutput result = OnDoTask(taskArgument.Handler);
				taskArgument.Handler.SetResult(result);
			}
			catch (Exception exception)
			{
				HandleException(exception);
				taskArgument.Handler.SetException(exception);
			}
		}

		private void DoJobWithDebug(TaskArgument taskArgument)
		{
			TOutput result = OnDoTask(taskArgument.Handler);
			taskArgument.Handler.SetResult(result);
		}

		#endregion
	}
}
