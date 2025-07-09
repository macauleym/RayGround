using System.Text;
using RayGround.Core.Extensions;

namespace RayGround.Core;

public class RayMatrix(int rows, int columns) : ICloneable, IEquatable<RayMatrix>
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

    #region Indexers
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
    #endregion Indexers

    #region Custom Methods
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
    #endregion
    
    #region Implementations
    public object Clone()
    {
        var clone = new RayMatrix(Rows, Columns);
        for (var r = 0; r < Rows; r++)
        for (var c = 0; c < Columns; c++)
            clone[r, c] = matrix[r, c];

        return clone;
    }

    public RayMatrix CloneMatrix() =>
        (RayMatrix)Clone();
    
    public bool Equals(RayMatrix? other)
    {
        if (other is null) 
            return false;

        return ReferenceEquals(this, other) 
               || ValueEquals(other);
    }
    #endregion Implementations
    
    #region Overrides
    public override bool Equals(object? obj)
    {
        switch (obj)
        {
            case null:
                return false;
            case RayTuple tuple:
                obj = tuple.ToMatrix();
                break;
        }

        if (obj.GetType() != GetType()) 
            return false;
        
        return ReferenceEquals(this, obj)
               || Equals((RayMatrix)obj);
    }

    public override int GetHashCode() =>
        matrix.GetHashCode();

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
    #endregion Overrides
    
    #region Operators
    public static bool operator ==(RayMatrix left, RayMatrix right) =>
        left.Equals(right);

    public static bool operator ==(RayMatrix left, RayTuple right) =>
        // ReSharper disable once SuspiciousTypeConversion.Global
        // There is type checking within the Equals method for the RayTuple type.
        left.Equals(right);

    public static bool operator !=(RayMatrix left, RayMatrix right) =>
        !left.Equals(right);

    public static bool operator !=(RayMatrix left, RayTuple right) =>
        // ReSharper disable once SuspiciousTypeConversion.Global
        // There is type checking within the Equals method for the RayTuple type.
        !left.Equals(right);
    
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

    public static RayMatrix operator *(RayMatrix left, RayTuple right) =>
        left * right.ToMatrix();

    public static RayMatrix operator *(RayMatrix left, float scaler)
    {
        var result = new RayMatrix(left.Rows, left.Columns);
        for (var r = 0; r < left.Rows; r++)
        for (var c = 0; c < left.Columns; c++)
            result[r, c] = left[r, c] *= scaler;

        return result;
    }
    #endregion Operators
}
