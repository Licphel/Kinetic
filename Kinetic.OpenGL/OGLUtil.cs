using System.Drawing;
using System.Numerics;
using Kinetic.Math;
using OpenTK.Mathematics;
using Vector2 = System.Numerics.Vector2;
using Vector4 = Kinetic.Math.Vector4;

namespace Kinetic.OpenGL;

public class OGLUtil
{

	public static Color4<Rgba> ToColor(Vector4 vec)
	{
		return new Color4<Rgba>((int) (vec.x * 255), (int) (vec.y * 255), (int) (vec.z * 255), (int) (vec.w * 255));
	}

	public static Matrix4x4 ToM4(Matrix mat)
	{
		return new Matrix4x4(
			mat.m00, mat.m01, mat.m02, mat.m03,
			mat.m10, mat.m11, mat.m12, mat.m13,
			mat.m20, mat.m21, mat.m22, mat.m23,
			mat.m30, mat.m31, mat.m32, mat.m33
		);
	}

	public static Math.Vector2 SysToVec2(Vector2i vec)
	{
		return new Math.Vector2(vec.X, vec.Y);
	}

	public static Vector2 ToSysVec2(Math.Vector2 vec)
	{
		return new Vector2(vec.x, vec.y);
	}

}
