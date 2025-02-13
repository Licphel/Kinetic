using Kinetic.IO;

namespace Kinetic.App;

public class LoadingQueue
{

	public int AsyncCount;

	protected List<LoadingQueue> Children = new List<LoadingQueue>();
	protected bool DoneBasically;

	//filebase is used to generate relative resource file.
	//for example, a C:/s1/s2/s3.png is retargeted to s2/s3.png under a file base of C:/s1.
	public FileHandle Filebase = FileSystem.GetAbsolute("");

	public string Namespace;

	protected Queue<Response> PreLoadTasks = new Queue<Response>();
	public Response BeginTask = () => {};
	public Response EndTask = () => {};

	public float Progress;
	public float Run;
	protected Queue<Response> Tasks = new Queue<Response>();
	public float Total;
	public Dictionary<string, UploadQueueProcessor> Processors = new();

	public LoadingQueue(string namespc)
	{
		Namespace = namespc;
	}

	public virtual bool Done => DoneBasically && AsyncCount <= 0;

	public void SubLoader(LoadingQueue loader)
	{
		if(loader == null)
		{
			return;
		}

		Children.Add(loader);

		Total += loader.Total;

		FlushProgress();
	}

	void Enqueue0(Response task, bool preLoad)
	{
		if(!preLoad)
		{
			Tasks.Enqueue(task);
			++Total;
			DoneBasically = false;
		}
		else
		{
			PreLoadTasks.Enqueue(task);
		}
	}

	public void DoPreLoad()
	{
		while(PreLoadTasks.Count != 0)
		{
			PreLoadTasks.Dequeue().Invoke();
		}

		foreach(LoadingQueue c in Children)
		{
			c.DoPreLoad();
		}
	}

	public virtual void Next()
	{
		if(Tasks.Count == 0)
		{
			if(Children.Count == 0)
			{
				DoneBasically = true;
				Progress = 1;
				return;
			}
			LoadingQueue loader1 = Children[0];

			loader1.Next();
			++Run;
			Progress = Run / Total;

			if(loader1.Done)
			{
				Children.RemoveAt(0);
			}
		}
		else
		{
			Tasks.Dequeue().Invoke();
			++Run;
			Progress = Run / Total;
		}
	}

	public void FlushProgress()
	{
		Total = Tasks.Count;
		Run = 0;
		Progress = 0;
		DoneBasically = false;
	}

	public void Enqueue(Response task, bool preLoad)
	{
		Enqueue0(task, preLoad);
	}

	public void Enqueue(Response task)
	{
		Enqueue0(task, false);
	}

	public void Load(FileHandle file, bool preload = false)
	{
		string resName = file.Path.Replace(Filebase.Path + "/", "");
		AddLoadTask(resName, file, preload);
	}

	public void Scan(bool preload)
	{
		Scan(Filebase, preload);
	}

	public void Scan(FileHandle startPos, bool preload)
	{
		if(!startPos.Exists)
			return;
		FileHandle[] files = startPos.Directories;

		foreach(FileHandle file in files)
		{
			Scan(file, preload);
		}

		files = startPos.Files;

		foreach(FileHandle file in files)
		{
			Load(file, preload);
		}
	}

	void AddLoadTask(string resource, FileHandle file, bool preLoad)
	{
		Enqueue(() =>
		{
			foreach(var kv in Processors)
			{
				if(resource.StartsWith(kv.Key))
					kv.Value(new ID(Namespace, resource), file);
			}
		}, preLoad);
	}

}

public delegate void UploadQueueProcessor(ID resource, FileHandle file);
