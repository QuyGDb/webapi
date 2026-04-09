namespace MusicShop.API.Infrastructure;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public MetaData? Meta { get; set; }

    public static ApiResponse<T> SuccessResult(T data, MetaData? meta = null) => new()
    {
        Success = true,
        Data = data,
        Meta = meta
    };
}

public class MetaData
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
}
