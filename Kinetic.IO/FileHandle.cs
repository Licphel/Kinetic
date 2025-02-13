namespace Kinetic.IO;

//Control files and folders without "/" contained.
//You should consider the file type when using because it has no handler for error.
public interface FileHandle
{

	public string Path { get; }
	bool IsDirectory { get; }
	bool IsFile { get; }
	bool Exists { get; }
	FileHandle[] Directories { get; }
	FileHandle[] Files { get; }
	string Format { get; }
	string Name { get; }

	FileHandle Goto(string name);

	FileHandle Exit();
	
	void Mkdirs();

	void Mkfile();

	void Delete();

}