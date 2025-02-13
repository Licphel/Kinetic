namespace Kinetic.Math;

public class Affine
{

	public float m00 = 1.0f;
	public float m01;
	public float m02;
	public float m10;
	public float m11 = 1.0f;
	public float m12;

	public void Identity()
	{
		m00 = 1.0f;
		m01 = 0.0f;
		m02 = 0.0f;
		m10 = 0.0f;
		m11 = 1.0f;
		m12 = 0.0f;
	}

	public void Set(Affine other)
	{
		m00 = other.m00;
		m01 = other.m01;
		m02 = other.m02;
		m10 = other.m10;
		m11 = other.m11;
		m12 = other.m12;
	}

	public Affine Set(Matrix matrix)
	{
		m00 = matrix.m00;
		m01 = matrix.m01;
		m02 = matrix.m03;
		m10 = matrix.m10;
		m11 = matrix.m11;
		m12 = matrix.m13;
		return this;
	}

	public void Set(float x, float y, float deg, float scaleX, float scaleY)
	{
		m02 = x;
		m12 = y;
		if(deg == 0.0f)
		{
			m00 = scaleX;
			m01 = 0.0f;
			m10 = 0.0f;
			m11 = scaleY;
		}
		else
		{
			float sin = FloatMath.SinRad(deg);
			float cos = FloatMath.CosRad(deg);
			m00 = cos * scaleX;
			m01 = -sin * scaleY;
			m10 = sin * scaleX;
			m11 = cos * scaleY;
		}
	}

	public void ToOrtho(float left, float right, float bottom, float top)
	{
		float xOrtho = 2.0f / (right - left);
		float yOrtho = 2.0f / (top - bottom);
		m00 = xOrtho;
		m10 = 0.0f;
		m01 = 0.0f;
		m11 = yOrtho;
		m02 = 0.0f;
		m12 = 0.0f;
	}

	public void Mul(Affine other)
	{
		float tmp00 = m00 * other.m00 + m01 * other.m10;
		float tmp01 = m00 * other.m01 + m01 * other.m11;
		float tmp02 = m00 * other.m02 + m01 * other.m12 + m02;
		float tmp10 = m10 * other.m00 + m11 * other.m10;
		float tmp11 = m10 * other.m01 + m11 * other.m11;
		float tmp12 = m10 * other.m02 + m11 * other.m12 + m12;
		m00 = tmp00;
		m01 = tmp01;
		m02 = tmp02;
		m10 = tmp10;
		m11 = tmp11;
		m12 = tmp12;
	}

	public float Determinant()
	{
		return m00 * m11 - m01 * m10;
	}

	public void Invert()
	{
		float det = Determinant();
		float invDet = 1.0f / det;
		float tmp00 = m11;
		float tmp01 = -m01;
		float tmp02 = m01 * m12 - m11 * m02;
		float tmp10 = -m10;
		float tmp11 = m00;
		float tmp12 = m10 * m02 - m00 * m12;
		m00 = invDet * tmp00;
		m01 = invDet * tmp01;
		m02 = invDet * tmp02;
		m10 = invDet * tmp10;
		m11 = invDet * tmp11;
		m12 = invDet * tmp12;
	}

	public VaryingVector2 ApplyTo(VaryingVector2 vec)
	{
		float x = vec.x;
		float y = vec.y;
		vec.x = m00 * x + m01 * y + m02;
		vec.y = m10 * x + m11 * y + m12;
		return vec;
	}

	public Vector2 ApplyTo(ref Vector2 vec)
	{
		float x = vec.x;
		float y = vec.y;
		vec.x = m00 * x + m01 * y + m02;
		vec.y = m10 * x + m11 * y + m12;
		return vec;
	}

	public Affine Translate(float x, float y)
	{
		m02 += m00 * x + m01 * y;
		m12 += m10 * x + m11 * y;
		return this;
	}

	public Affine PreTranslate(float x, float y)
	{
		m02 += x;
		m12 += y;
		return this;
	}

	public Affine Scale(float scaleX, float scaleY)
	{
		m00 *= scaleX;
		m01 *= scaleY;
		m10 *= scaleX;
		m11 *= scaleY;
		return this;
	}

	public Affine PreScale(float scaleX, float scaleY)
	{
		m00 *= scaleX;
		m01 *= scaleX;
		m02 *= scaleX;
		m10 *= scaleY;
		m11 *= scaleY;
		m12 *= scaleY;
		return this;
	}

	public Affine Rotate(float radians)
	{
		if(radians == 0)
		{
			return this;
		}

		float cos = FloatMath.CosRad(radians);
		float sin = FloatMath.SinRad(radians);

		float tmp00 = m00 * cos + m01 * sin;
		float tmp01 = m00 * -sin + m01 * cos;
		float tmp10 = m10 * cos + m11 * sin;
		float tmp11 = m10 * -sin + m11 * cos;

		m00 = tmp00;
		m01 = tmp01;
		m10 = tmp10;
		m11 = tmp11;
		return this;
	}

}
