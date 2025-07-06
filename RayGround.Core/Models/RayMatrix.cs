using System.Data;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace RayGround.Core;

public class RayMatrix(int rows, int columns) : IEquatable<RayMatrix>
{
    public const float EPSILON                = 0.00001f;
    public const short SIGNIFICANT_DIGITS     = 5;
    public static readonly RayMatrix Identity = new(4,4)
    { [0] = [ 1, 0, 0 ,0 ]
    , [1] = [ 0, 1, 0, 0 ]
    , [2] = [ 0, 0, 1, 0 ]
    , [3] = [ 0, 0, 0, 1 ]
    };

    public int Rows    = rows;
    public int Columns = columns;
    
    readonly float[,] matrix = new float[rows, columns];

    public float this[int r, int c]
    {
        get => matrix[r, c];
        set => matrix[r, c] = value;
    }

    public float[] this[int r]
    {
        get
        {
            var row = new float[columns];
            for (var col = 0; col < columns; col++)
                row[col] = matrix[r, col];

            return row;
        }
        set
        {
            for (var col = 0; col < columns; col++)
                matrix[r, col] = value[col];
        }
    }

    public T Traverse<T>(T seed, Func<T, int, int, T> traverser)
    {
        var acc = seed;
        for (var r = 0; r < Rows; r++)
        for (var c = 0; c < Columns; c++)
            acc = traverser(acc, r, c);

        return acc;
    }
    
    public bool Equals(RayMatrix? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        
        return matrix.Equals(other.matrix);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        
        return Equals((RayMatrix)obj);
    }

    bool ValueEquals(RayMatrix? other)
    {
        if (other is null)
            return false;
        
        for (var r = 0; r < rows; r++)
        for (var c = 0; c < columns; c++)
            if (Math.Abs(matrix[r, c] - other[r, c]) > EPSILON)
                return false;

        return true;
    }
    
    public static bool operator ==(RayMatrix left, RayMatrix right) =>
        left.Equals(right) || left.ValueEquals(right);

    public static bool operator !=(RayMatrix left, RayMatrix right) =>
        !left.Equals(right) || !left.ValueEquals(right);
    
    public static RayMatrix operator *(RayMatrix left, RayMatrix right)
    {
        var result = new RayMatrix(left.Rows, right.Columns);
        for (var r = 0; r < left.Rows; r++)
        for (var c = 0; c < right.Columns; c++)
            result[r, c] =
                  left[r, 0] * right[0, c]
                + left[r, 1] * right[1, c]
                + left[r, 2] * right[2, c]
                + left[r, 3] * right[3, c];

        return result;
    }
    
    public override int GetHashCode()
    {
        return matrix.GetHashCode();
    }

    public override string ToString()
    {
        var rowBuilder = new StringBuilder();
        var matrixBuilder = new StringBuilder();
        for (var r = 0; r < Rows; r++)
        {
            rowBuilder.Append('|');
            for (var c = 0; c < Columns; c++)
                rowBuilder.Append($" {matrix[r, c]} ");
            rowBuilder.Append('|');
            matrixBuilder.AppendLine(rowBuilder.ToString());
            rowBuilder.Clear();
        }
        
        return matrixBuilder.ToString();
    }
}
