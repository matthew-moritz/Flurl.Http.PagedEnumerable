# Flurl.Http.PagedEnumerable

This package contains a collection of [Flurl](https://github.com/tmenier/Flurl) extensions to allow the handling of paged HTTP requests as an enumerable collection.

This example shows the asynchronous version, but the synchronous version is also supported.

```csharp
var responses = await "https://api.mysite.com"
    .AppendPathSegment("users")
    .SetQueryParams(new { limit = 50, offset = 0})
    .AsPagedAsyncEnumerable<UsersResponse>(r => r.HasNext ? r.Next : null);

// Requests are made for each page only when the collection is iterated. 
await foreach(var response in responses)
{
    var users = response.Users;

    dbContext.Users.AddAll(users);
    dbContext.SaveChanges();
}

// A sample HTTP response POCO.
public class UsersResponse
{
    [JsonPropertyName("next")]
    public string Next { get; set; }

    [JsonPropertyName("users")]
    public IReadOnlyCollection<Users> Users { get; set; }
}
```