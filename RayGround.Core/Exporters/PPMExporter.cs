using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using RayGround.Core.Extensions;
using RayGround.Core.Interfaces;

namespace RayGround.Core.Exporters;

public class PPMExporter : IExportCanvas
{
    const short MAX_LINE_LENGTH = 70;
    const short SPACE_LENGTH    = 1;
    
    string PPMHeader(float width, float height) =>
$"""
P3
{width} {height}
255
""";

    static Task<string> FormatColorAsync(float amount) => Task.Run(() =>
        MathF.Round(
              Math.Clamp(amount * 255, 0, 255)
            , MidpointRounding.AwayFromZero
            ).ToString()
        );

    static Task<int> AppendColorAmountAsync(string formattedAmount, int currentLength, StringBuilder builder) => Task.Run(() =>
    {
        var newLength = currentLength;
        if (currentLength + formattedAmount.Length + SPACE_LENGTH > MAX_LINE_LENGTH)
        {
            // Remove trailing whitespace.
            builder.Remove(builder.Length - 1, SPACE_LENGTH);

            // Add a new line.
            builder.AppendLine();

            // Reset the length; we're on a new line.
            newLength = 0;
        }

        // Now add the formatted amount with a trailing space.
        builder.Append(formattedAmount);
        builder.Append(' ');

        // Update the length with the new amount that we added.
        newLength += formattedAmount.Length + SPACE_LENGTH;

        return newLength;
    });

    static async Task<KeyValuePair<int, StringBuilder>> ProcessDataRowAsync(RayCanvas canvas, int currentRow)
    {
        var rowLength  = 0;
        var rowBuilder = new StringBuilder();
        Console.WriteLine($"Building PPM Export for PixelData row {currentRow+1}...");
        for (var width = 0; width < canvas.Width; width++)
        {
            var pixel     = canvas.GetPixel(width, currentRow);
            
            var redTask   = FormatColorAsync(pixel.Color.Red);
            var greenTask = FormatColorAsync(pixel.Color.Green);
            var blueTask  = FormatColorAsync(pixel.Color.Blue);
            await Task.WhenAll(redTask, greenTask, blueTask);
            
            rowLength = await AppendColorAmountAsync(
                  redTask.Result
                , rowLength
                , rowBuilder
                );
            rowLength = await AppendColorAmountAsync(
                  greenTask.Result
                , rowLength
                , rowBuilder
                );
            rowLength = await AppendColorAmountAsync(
                  blueTask.Result
                , rowLength
                , rowBuilder
                );
        }

        return new KeyValuePair<int, StringBuilder>(currentRow, rowBuilder);
    }
    
    static async Task<string> PixelDataAsync(RayCanvas canvas)
    {
        var dataBuilder = new StringBuilder();
        var rowTasks = new List<Task<KeyValuePair<int, StringBuilder>>>();
        for (var height = 0; height < canvas.Height; height++)
            rowTasks.Add(ProcessDataRowAsync(canvas, height));

        await Task.WhenAll(rowTasks);
        var rowStrings =rowTasks
            .Select(t => t.Result)
            .ToImmutableSortedDictionary(kvp => kvp.Key, kvp => kvp.Value)
            .Select(kvp => kvp.Value.ToString().TrimEnd());
        dataBuilder.AppendJoin('\n', rowStrings);

        return dataBuilder.ToString();
    }

    public async Task<string> ExportAsync(RayCanvas canvas)
    {
        var exportTimer = Stopwatch.StartNew();
        var ppmBuilder  = new StringBuilder();
        
        ppmBuilder.Append(PPMHeader(canvas.Width, canvas.Height));
        ppmBuilder.AppendLine();

        var data = await PixelDataAsync(canvas);
        ppmBuilder.Append(data);
        ppmBuilder.AppendLine();
        
        exportTimer.Stop();
        Console.WriteLine($"Finished exporting in {exportTimer.Elapsed.Hours}:{exportTimer.Elapsed.Minutes}:{exportTimer.Elapsed.Milliseconds}.");

        return ppmBuilder.ToString();
    }
}
