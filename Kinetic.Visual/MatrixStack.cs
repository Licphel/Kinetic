using Kinetic.Math;

namespace Kinetic.Visual;

public class MatrixStack
{

	readonly Stack<Affine> Stack = new Stack<Affine>();

	public bool Changed;

	Affine[] TempMats;
	public Affine Top = new Affine();

	public MatrixStack()
	{
		RecreateMatsForLen(128);
		Stack.Push(Top);
	}

	public bool IsEmpty => Stack.Count == 1;//Top0 is never removed.

	public void RecreateMatsForLen(int len)
	{
		if(TempMats == null || TempMats.Length < len)
		{
			TempMats = new Affine[len];
			for(int i = 0; i < TempMats.Length; i++) TempMats[i] = new Affine();
		}
	}

	public void Push()
	{
		int take = Stack.Count;
		if(take < TempMats.Length)
		{
			Affine aff = TempMats[take];
			aff.Identity();
			Push(aff);
			return;
		}

		Push(new Affine());
	}

	void Push(Affine aff)
	{
		Top = aff;
		Top.Set(Stack.Peek());
		Stack.Push(Top);
		Changed = true;
	}

	public void Pop()
	{
		Stack.Pop();
		Top = Stack.Peek();
		Changed = true;
	}

	public void Load(Affine affine)
	{
		Top.Set(affine);
		Changed = true;
	}

	public void RotateRad(float f)
	{
		Top.Rotate(f);
		Changed = true;
	}

	public void RotateDeg(float f)
	{
		Top.Rotate(FloatMath.Rad(f));
		Changed = true;
	}

	public void RotateRad(float f, float x, float y)
	{
		Translate(x, y);
		RotateRad(f);
		Translate(-x, -y);
	}

	public void RotateDeg(float f, float x, float y)
	{
		Translate(x, y);
		RotateDeg(f);
		Translate(-x, -y);
	}

	public void Translate(float x, float y)
	{
		Top.Translate(x, y);
		Changed = true;
	}

	public void Scale(float x, float y)
	{
		Top.Scale(x, y);
		Changed = true;
	}

}
