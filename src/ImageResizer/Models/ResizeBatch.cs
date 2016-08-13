using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageResizer.Properties;

namespace ImageResizer.Models
{
    public class ResizeBatch
    {
        public string DestinationDirectory { get; set; }
        public ICollection<string> Files { get; } = new List<string>();

        public static ResizeBatch FromCommandLine(TextReader standardInput, string[] args)
        {
            var batch = new ResizeBatch();

            string file;
            while ((file = standardInput.ReadLine()) != null)
                batch.Files.Add(file);

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] == "/d")
                {
                    batch.DestinationDirectory = args[++i];
                    continue;
                }

                batch.Files.Add(args[i]);
            }

            return batch;
        }

        public IEnumerable<ResizeError> Process(
            CancellationToken cancellationToken,
            Action<int, double> reportProgress)
        {
            double total = Files.Count;
            var completed = 0;
            var errors = new ConcurrentBag<ResizeError>();

            Parallel.ForEach(
                Files,
                new ParallelOptions
                {
                    CancellationToken = cancellationToken,
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                },
                (file, state, i) =>
                {
                    var operation = new ResizeOperation(file, DestinationDirectory, Settings.Default);

                    try
                    {
                        operation.Execute();
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ResizeError { File = Path.GetFileName(file), Error = ex.Message });
                    }

                    Interlocked.Increment(ref completed);

                    reportProgress(completed, total);
                });

            return errors;
        }
    }
}
