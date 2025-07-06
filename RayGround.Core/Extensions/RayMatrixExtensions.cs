namespace RayGround.Core.Extensions;

public static class RayMatrixExtensions
{
    public static RayMatrix Transpose(this RayMatrix source)
    {
        var result = new RayMatrix(source.Rows, source.Columns);
        for (var r = 0; r < source.Rows; r++)
        for (var c = 0; c < source.Columns; c++)
            result[c, r] = source[r, c];

        return result;
    }

    public static float Determinant(this RayMatrix source)
    {
        if (source.Columns == 2)
            return source[0, 0] * source[1, 1]
                - source[0, 1] * source[1, 0];
        
        var accumulator = source[0, 0] * source.Cofactor(0, 0);
        for (var col = 1; col < source.Columns; col++)
            accumulator += source[0, col] * source.Cofactor(0, col);

        return accumulator;
    }

    public static RayMatrix Submatrix(this RayMatrix source, int targetRow, int targetColumn)
    {
        var result = new RayMatrix(source.Rows - 1, source.Columns - 1);
        for (var r = 0; r < result.Rows; r++)
        for (var c = 0; c < result.Columns; c++)
        {
            var sourceRow    = r >= targetRow    ? r + 1 : r;
            var sourceColumn = c >= targetColumn ? c + 1 : c;
            
            result[r, c] = source[sourceRow, sourceColumn];
        }

        return result;
    }

    public static float Minor(this RayMatrix source, int targetRow, int targetColumn) =>
        source
        .Submatrix(targetRow, targetColumn)
        .Determinant();

    public static float Cofactor(this RayMatrix source, int targetRow, int targetColumn)
    {
        var result = 0f;
        var columnSum = targetRow + targetColumn;
        if (columnSum % 2 == 0)
            result = source.Minor(targetRow, targetColumn);
        else
            result = -source.Minor(targetRow, targetColumn);

        return result;
    }
    
    public static bool IsInvertible(this RayMatrix source) =>
        source.Determinant() != 0;

    public static RayMatrix Inverse(this RayMatrix source)
    {
        if (!source.IsInvertible())
            return source;

        var result      = new RayMatrix(source.Rows, source.Columns);
        var determinant = source.Determinant();
        for (var r = 0; r < result.Rows; r++)
        for (var c = 0; c < result.Columns; c++)
        {
            var cofactor = source.Cofactor(r, c);
            result[c, r] = cofactor / determinant;
        }

        return result;
    }
    
    public static RayTuple ToTuple(this RayMatrix source) =>
        new(
          source[0, 0]
        , source[1, 0]
        , source[2, 0]
        , source[3, 0]
        );
}
