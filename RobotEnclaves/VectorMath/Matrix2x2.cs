using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorMath
{
    public class Matrix2x2
    {
        public Vector2[] Columns { get; }

        public Matrix2x2(Vector2 firstColumn, Vector2 secondColumn)
        {
            Columns = new Vector2[2];

            Columns[0] = new Vector2(firstColumn);
            Columns[1] = new Vector2(secondColumn);
        }

        public static Matrix2x2 Identity()
        {
            return new Matrix2x2(new Vector2(1, 0), new Vector2(0, 1));
        }

        public static Vector2 operator *(Matrix2x2 matrix, Vector2 vector)
        {
            return new Vector2(Vector2.Dot(matrix.Columns[0], vector), Vector2.Dot(matrix.Columns[1], vector));
        }

    }
}
