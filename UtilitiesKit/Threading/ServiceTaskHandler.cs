namespace UtilitiesKit.Threading
{
	using System;
	using System.Threading;

	public enum ExcutionStatus { InProgress, Done, Interrupted, Errored }

	public abstract class ServiceTaskHandler<TInput, TOutput> : IDisposable
	{
		private object _ThreadSync = new object();

		private bool _Disposed;
		private AutoResetEvent _Semaphore = new AutoResetEvent(false);
		private TOutput _Result = default(TOutput);

		public TInput Argument { get; set; }
		public Exception Exception { get; private set; }

		public ExcutionStatus Status { get; private set; }

		/// <summary>
		/// Class constructor
		/// </summary>
		public ServiceTaskHandler()
		{
			Status = ExcutionStatus.InProgress;
		}

		#region Public members

		public virtual string Description
		{
			get { return ToString(); }
		}

		/// <summary>
		/// Sets the exception. Changes Status to Errored.
		/// </summary>
		/// <param name="exception"></param>
		public void SetException(Exception exception)
		{
			lock (_ThreadSync)
			{
				if (Status != ExcutionStatus.InProgress)
					throw new InvalidOperationException("Handler is not in InProgress state.");

				Exception = exception;
				Status = ExcutionStatus.Errored;
				_Semaphore.Set();
			}
		}

		/// <summary>
		/// Sets the calculation result. Changes Status to Done
		/// </summary>
		/// <param name="result"></param>
		public void SetResult(TOutput result)
		{
			lock (_ThreadSync)
			{
				if (Status != ExcutionStatus.InProgress)
					throw new InvalidOperationException("Handler is not in InProgress state.");

				_Result = result;
				Status = ExcutionStatus.Done;
				_Semaphore.Set();
			}
		}

		/// <summary>
		/// Interrupts the execution and sets status to 
		/// </summary>
		public void Interrupt()
		{
			lock (_ThreadSync)
			{
				if (Status != ExcutionStatus.InProgress)
					throw new InvalidOperationException("Handler is not in InProgress state.");

				Status = ExcutionStatus.Interrupted;
				_Semaphore.Set();
			}
		}

		/// <summary>
		/// Waits for finishing calculations and returns result.
		/// Throws an exception when Status is Errored.
		/// </summary>
		/// <returns></returns>
		public TOutput WaitForResult()
		{
			ExcutionStatus status;
			lock(_ThreadSync)
			{
				if (Exception != null)
					throw Exception;

				status = Status;
			}

			if (status == ExcutionStatus.InProgress)
				_Semaphore.WaitOne();

			Dispose();
			return _Result;
		}

		#endregion

		#region Dispose pattern

		public void Dispose()
		{
			_Semaphore.Dispose();
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (_Disposed)
				return;

			if (disposing)
			{
				OnDispose(disposing);
				_Semaphore.Dispose();
			}

			_Disposed = true;
		}

		protected virtual void OnDispose(bool disposing) { }

		#endregion
	}
}