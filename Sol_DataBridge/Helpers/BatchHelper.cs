namespace Sol_DataBridge.Helpers;

public static class BatchHelper
{
    public static IEnumerable<List<T>>
        CreateBatch<T>(IEnumerable<T> source,
                       int batchSize)
    {
        List<T> batch = new();

        foreach (var item in source)
        {
            batch.Add(item);

            if (batch.Count == batchSize)
            {
                yield return batch;

                batch = new();
            }
        }

        if (batch.Count > 0)
            yield return batch;
    }
}