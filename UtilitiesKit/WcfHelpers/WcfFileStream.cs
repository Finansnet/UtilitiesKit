namespace UtilitiesKit.WcfHelpers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IWcfFileStreamDataProvider
	{
		byte[] GetBinaryData(long offset, int length);
		long GetFileSize();
	}

	public class WcfFileStream : Stream
	{
		private IWcfFileStreamDataProvider _DataProvider;

		private int _BufferPointer = 0;
		private int _NetworkPackageLength;
		private byte[] _Buffer;
		private bool _EndOfFile;

		public override bool CanRead { get { return true; } }
		public override bool CanSeek { get { return false; } }
		public override bool CanWrite { get { return false; } }

		public override void Flush()
		{
			// Do nothing 
		}

		public override long Length
		{
			get
			{
				if (_Length == -1)
					_Length = _DataProvider.GetFileSize();
				return _Length;
			}
		}
		private long _Length = -1;

		public override long Position
		{
			get { return _Position; }
			set { throw new NotImplementedException(); }
		}
		private long _Position;

		/// <summary>
		/// Initializes a new instance of the <see cref="WcfFileStream"/> class.
		/// </summary>
		/// <param name="guid">The unique identifier.</param>
		/// <param name="getFileSizeDelegate">The get file size delegate.</param>
		/// <param name="getBinaryDataDelegate">The get binary data delegate.</param>
		/// <param name="networkPackageLength">Length of the network package.</param>
		public WcfFileStream(IWcfFileStreamDataProvider dataProvider, int networkPackageLength = 2048)
		{
			_DataProvider = dataProvider;
			_NetworkPackageLength = networkPackageLength;
		}

		public override long Seek(long offset, SeekOrigin origin) { throw new NotImplementedException(); }
		public override void SetLength(long value) { throw new NotImplementedException(); }
		public override void Write(byte[] buffer, int offset, int count) { throw new NotImplementedException(); }

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (_EndOfFile)
				return 0;

			for (int index = 0; index < count; index++)
			{
				byte? readByte = ReadNextByte();
				if (readByte.HasValue)
				{
					buffer[offset + index] = readByte.Value;
					_Position++;
				}
				else
				{
					_Position++;
					_EndOfFile = true;
					return index;
				}
			}
			return count;
		}

		private byte? ReadNextByte()
		{
			if (_Buffer == null || _BufferPointer >= _Buffer.Length)
			{
				_BufferPointer = 0;
				_Buffer = _DataProvider.GetBinaryData(_Position, (int)_NetworkPackageLength);
				if (_Buffer == null || _Buffer.Length == 0)
					return null;
			}
			return _Buffer[_BufferPointer++];
		}
	}
}
