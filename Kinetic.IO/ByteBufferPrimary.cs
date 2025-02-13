namespace Kinetic.IO;

public partial class ByteBuffer
{

	public const int DefaultCapacity = 64;
	public const int TempSize = 64;

	//The Temp arrays for #Get.
	static readonly List<byte[]> TmpBytes = new List<byte[]>();

	public byte[] Buf;
	public int Capacity;
	protected int MarkReadIndex;
	protected int MarkWirteIndex;
	protected int ReadIndex;
	protected int WriteIndex;

	static ByteBuffer()
	{
		for(int i = 0; i <= TempSize; i++)
		{
			TmpBytes.Add(new byte[i]);
		}
	}

	public ByteBuffer(int cap = DefaultCapacity)
	{
		Buf = new byte[cap];
		Capacity = cap;
		ReadIndex = 0;
		WriteIndex = 0;
		MarkReadIndex = MarkWirteIndex = 0;
	}

	public ByteBuffer(byte[] bytes)
	{
		Buf = new byte[bytes.Length];
		Array.Copy(bytes, 0, Buf, 0, Buf.Length);
		Capacity = Buf.Length;
		ReadIndex = 0;
		WriteIndex = bytes.Length + 1;
		MarkReadIndex = MarkWirteIndex = 0;
	}

	public int ReadableBytes => WriteIndex - ReadIndex;

	protected static int GetPower2Len(int value)
	{
		if(value == 0)
		{
			return 1;
		}

		value--;
		value |= value >> 1;
		value |= value >> 2;
		value |= value >> 4;
		value |= value >> 8;
		value |= value >> 16;
		return value + 1;
	}

	protected static byte[] Flip(byte[] bytes)
	{
		if(BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}

		return bytes;
	}

	protected int FixSizeAndReset(int currLen, int futureLen)
	{
		if(futureLen > currLen)
		{
			int size = GetPower2Len(currLen) * 2;
			if(futureLen > size)
			{
				size = GetPower2Len(futureLen) * 2;
			}

			byte[] newbuf = new byte[size];
			Array.Copy(Buf, 0, newbuf, 0, currLen);
			Buf = newbuf;
			Capacity = size;
		}

		return futureLen;
	}

	public void WriteBytes(byte[] bytes, int startIndex, int length)
	{
		int offset = length - startIndex;
		if(offset <= 0) return;
		int total = offset + WriteIndex;
		int len = Buf.Length;
		FixSizeAndReset(len, total);
		for(int i = WriteIndex, j = startIndex; i < total; i++, j++)
		{
			Buf[i] = bytes[j];
		}

		WriteIndex = total;
	}

	public void MarkReaderIndex()
	{
		MarkReadIndex = ReadIndex;
	}

	public void MarkWriterIndex()
	{
		MarkWirteIndex = WriteIndex;
	}

	public void ResetReaderIndex()
	{
		ReadIndex = MarkReadIndex;
	}

	public void ResetWriterIndex()
	{
		WriteIndex = MarkWirteIndex;
	}

	public ByteChunk Output()
	{
		return new ByteChunk(Buf, 0, WriteIndex);
	}

	public void Clear()
	{
		for(int i = 0; i < Buf.Length; i++)
		{
			Buf[i] = 0;
		}

		ReadIndex = 0;
		WriteIndex = 0;
		MarkReadIndex = 0;
		MarkWirteIndex = 0;
		Capacity = Buf.Length;
	}

	public void Dispose()
	{
		ReadIndex = 0;
		WriteIndex = 0;
		MarkReadIndex = 0;
		MarkWirteIndex = 0;
		Capacity = 0;
		Buf = null;
	}

	public byte ReadByte()
	{
		byte b = Buf[ReadIndex];
		ReadIndex++;
		return b;
	}

	byte[] Get(int index, int len)
	{
		byte[] bytes = len <= TempSize ? TmpBytes[len] : new byte[len];
		Array.Copy(Buf, index, bytes, 0, len);
		return Flip(bytes);
	}

	protected byte[] Read(int len)
	{
		byte[] bytes = Get(ReadIndex, len);
		ReadIndex += len;
		return bytes;
	}

}
