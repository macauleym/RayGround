namespace RayGround.Core.Calculators;

public static class MatrixCalculator
{
    public static RayMatrix Transpose(RayMatrix of)
    {
        var result = new RayMatrix(of.Rows, of.Columns);
        for (var r = 0; r < of.Rows; r++)
        for (var c = 0; c < of.Columns; c++)
            result[c, r] = of[r, c];

        return result;
    }

    public static float Determinant(RayMatrix of)
    {
        if (of.Columns == 2)
            return of[0, 0] * of[1, 1]
                - of[0, 1] * of[1, 0];
        
        var accumulator = of[0, 0] * Cofactor(of, 0, 0);
        for (var col = 1; col < of.Columns; col++)
            accumulator += of[0, col] * Cofactor(of, 0, col);

        return accumulator;
    }

    public static RayMatrix Submatrix(RayMatrix of, int targetRow, int targetColumn)
    {
        var result = new RayMatrix(of.Rows - 1, of.Columns - 1);
        for (var r = 0; r < result.Rows; r++)
        for (var c = 0; c < result.Columns; c++)
        {
            var sourceRow    = r >= targetRow    ? r + 1 : r;
            var sourceColumn = c >= targetColumn ? c + 1 : c;
            
            result[r, c] = of[sourceRow, sourceColumn];
        }

        return result;
    }

    public static float Minor(RayMatrix of, int targetRow, int targetColumn) =>
        Determinant(
            Submatrix(of, targetRow, targetColumn));

    public static float Cofactor(RayMatrix of, int targetRow, int targetColumn)
    {
        var result = 0f;
        var columnSum = targetRow + targetColumn;
        if (columnSum % 2 == 0)
            result = Minor(of, targetRow, targetColumn);
        else
            result = -Minor(of, targetRow, targetColumn);

        return result;
    }
    
    public static bool IsInvertible(RayMatrix of) =>
        Determinant(of) != 0;

    public static RayMatrix Inverse(RayMatrix of)
    {
        if (!IsInvertible(of))
            return of;

        var result      = new RayMatrix(of.Rows, of.Columns);
        var determinant = Determinant(of);
        for (var r = 0; r < result.Rows; r++)
        for (var c = 0; c < result.Columns; c++)
        {
            var cofactor = Cofactor(of, r, c);
            result[c, r] = cofactor / determinant;
        }

        return result;
    }
}
