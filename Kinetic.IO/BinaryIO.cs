namespace Kinetic.IO;

public class BinaryIO
{

	const byte
	COMPOUND = 16,
	LIST = 64,
	BYTE = 128,
	INT = 129,
	LONG = 130,
	FLOAT = 131,
	DOUBLE = 132,
	BOOL = 133,
	STR = 134,
	BYTE_ARR = 200,
	INT_ARR = 201,
	FLOAT_ARR = 202;

	public static byte GetId(object o)
	{
		switch(o)
		{
			case IBinaryCompound:
				return COMPOUND;
			case IBinaryList:
				return LIST;
			case byte:
				return BYTE;
			case int:
				return INT;
			case long:
				return LONG;
			case float:
				return FLOAT;
			case double:
				return DOUBLE;
			case bool:
				return BOOL;
			case string:
				return STR;
			case byte[]:
				return BYTE_ARR;
			case int[]:
				return INT_ARR;
			case float[]:
				return FLOAT_ARR;
		}

		return 0;
	}

	public static void Encode(IBinaryCompound compound, ByteBuffer output)
	{
		foreach(KeyValuePair<string, object> pair in compound)
		{
			string key = pair.Key;
			object val = pair.Value;

			output.WriteByte(GetId(val));
			output.WriteString(key);
			EncodePrimitive(val, output);
		}

		output.WriteByte(0);//Exit
	}

	public static IBinaryCompound Decode(ByteBuffer input)
	{
		IBinaryCompound compound = IBinaryCompound.New();

		while(true)
		{
			byte id = input.ReadByte();

			if(id == 0)
			{
				break;
			}

			string key = input.ReadString();
			object data = DecodePrimitive(input, id);
			compound.Set(key, data);
		}

		return compound;
	}

	static object DecodePrimitive(ByteBuffer input, byte id)
	{
		switch(id)
		{
			case COMPOUND:
				return Decode(input);
			case LIST:
				IBinaryList lst = IBinaryList.New();
				byte type = input.ReadByte();
				int size = input.ReadInt();

				if(type == 0 && size > 0)
				{
					throw new Exception("Find no type mark in BinaryList! Is the saving broken?");
				}

				for(int i = 0; i < size; i++)
				{
					object data = DecodePrimitive(input, type);
					lst.Insert(data);
				}

				return lst;
			case BYTE:
				return input.ReadByte();
			case INT:
				return input.ReadInt();
			case LONG:
				return input.ReadLong();
			case FLOAT:
				return input.ReadFloat();
			case DOUBLE:
				return input.ReadDouble();
			case BOOL:
				return input.ReadBoolean();
			case STR:
				return input.ReadString();
			case BYTE_ARR:
				int len = input.ReadInt();
				byte[] bytes = new byte[len];
				input.ReadBytes(bytes, len);
				return bytes;
			case INT_ARR:
				len = input.ReadInt();
				int[] ints = new int[len];
				for(int i = 0; i < len; i++)
				{
					ints[i] = input.ReadInt();
				}
				return ints;
			case FLOAT_ARR:
				len = input.ReadInt();
				float[] fs = new float[len];
				for(int i = 0; i < len; i++)
				{
					fs[i] = input.ReadFloat();
				}
				return fs;
		}

		return null;
	}

	static void EncodePrimitive(object o, ByteBuffer output)
	{
		switch(o)
		{
			case IBinaryCompound:
				Encode((IBinaryCompound) o, output);
				break;
			case IBinaryList:
				IBinaryList lst = (IBinaryList) o;
				output.WriteByte(lst.Type);
				output.WriteInt(lst.Count);
				foreach(object v in lst)
				{
					EncodePrimitive(v, output);
				}

				break;
			case byte:
				output.WriteByte((byte) o);
				break;
			case int:
				output.WriteInt((int) o);
				break;
			case long:
				output.WriteLong((long) o);
				break;
			case float:
				output.WriteFloat((float) o);
				break;
			case double:
				output.WriteDouble((double) o);
				break;
			case bool:
				output.WriteBoolean((bool) o);
				break;
			case string:
				output.WriteString((string) o);
				break;
			case byte[]:
				byte[] bytes = (byte[]) o;
				output.WriteInt(bytes.Length);
				output.WriteBytes(bytes);
				break;
			case int[]:
				int[] ints = (int[]) o;
				output.WriteInt(ints.Length);
				foreach(int i in ints)
				{
					output.WriteInt(i);
				}
				break;
			case float[]:
				float[] fs = (float[]) o;
				output.WriteInt(fs.Length);
				foreach(float i in fs)
				{
					output.WriteFloat(i);
				}
				break;
			default:
				break;
		}
	}

	//Packed ops:

	public static void Write(IBinaryCompound compound, FileHandle file)
	{
		if(!file.Exists) file.Mkfile();

		ByteBuffer buffer = new ByteBuffer();
		Encode(compound, buffer);
		PrimitiveIO.Write(file, buffer.Output());
	}

	public static IBinaryCompound Read(FileHandle file)
	{
		if(!file.Exists)
		{
			throw new FileNotFoundException($"Cannot find compound coded file at {file.Path}");
		}

		byte[] bytes = PrimitiveIO.Read(file);
		ByteBuffer buffer = new ByteBuffer(bytes);
		return Decode(buffer);
	}

}
