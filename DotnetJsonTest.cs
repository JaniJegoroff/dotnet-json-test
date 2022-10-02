namespace dotnet_json_test;

// NUnit Trace and Debug output is by default not sent to the console output.
[SetUpFixture, Description("Enables debug prints to console output")]
public class SetupTrace
{
    [OneTimeSetUp]
    public void StartTest()
    {
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    [OneTimeTearDown]
    public void EndTest()
    {
        Trace.Flush();
    }
}

[TestFixture]
[Author("Jani Jegoroff")]
[Parallelizable(scope: ParallelScope.All)]
public class Tests
{
    private JArray _albums = default!;
    private JArray _photos = default!;

    [OneTimeSetUp, Description("Fetches data from two sources and combines them to one dataset")]
    public void Setup()
    {
        const string ALBUMS_URL = "https://jsonplaceholder.typicode.com/albums";
        const string PHOTOS_URL = "https://jsonplaceholder.typicode.com/photos";

        var jsonAlbums = GetJsonData(ALBUMS_URL);
        _albums = JArray.Parse(jsonAlbums);

        var jsonPhotos = GetJsonData(PHOTOS_URL);
        _photos = JArray.Parse(jsonPhotos);

        _albums.Merge(_photos, new JsonMergeSettings
        {
            // union arrays, skipping items that already exist.
            MergeArrayHandling = MergeArrayHandling.Union
        });
    }

    [Test, Description("Combined dataset should have 5100 items")]
    public void TestNumberOfItemsAfterMerge()
    {
        const int ITEMS_MAX_COUNT = 5100;

        _albums.ShouldNotBeEmpty();
        _albums.Count().ShouldBeEquivalentTo(ITEMS_MAX_COUNT);
    }

    [Test, Description("Each user should have given amount of albums")]
    [TestCase(1, ExpectedResult=10)]
    [TestCase(2, ExpectedResult=10)]
    [TestCase(3, ExpectedResult=10)]
    [TestCase(4, ExpectedResult=10)]
    [TestCase(5, ExpectedResult=10)]
    [TestCase(6, ExpectedResult=10)]
    [TestCase(7, ExpectedResult=10)]
    [TestCase(8, ExpectedResult=10)]
    [TestCase(9, ExpectedResult=10)]
    [TestCase(10, ExpectedResult=10)]
    public int TestUserAlbumCount(int userId)
    {
        var users =
        from u in _albums.Select(x => x["userId"]).Where(x => x != null && (int)x == userId)
        select new { Count = u.Values() };

        return users.Count();
    }

    [Test, Description("Each album should have 50 photos")]
    public void TestAlbumPhotoCount(
        [Range(1, 100, 1)] int albumId,
        [Values(50)] int expectedPhotoCount)
    {
        var photos =
        from p in _albums.Select(x => x["albumId"]).Where(x => x != null && (int)x == albumId)
        select new { Count = p.Values() };

        photos.Count().ShouldBe(expectedPhotoCount);
    }

    [Test, Description("Each user should own given album")]
    [TestCase(1, 1)]
    [TestCase(2, 14)]
    [TestCase(3, 30)]
    [TestCase(4, 34)]
    [TestCase(5, 47)]
    [TestCase(6, 66)]
    [TestCase(7, 70)]
    [TestCase(8, 71)]
    [TestCase(9, 88)]
    [TestCase(10, 100)]
    public void TestUserOwnsAlbum(int userId, int albumId)
    {
        var albums =
        from item in _albums
        where item["userId"] != null && (int)item["userId"] == userId && (int)item["id"] == albumId
        select item["id"];

        foreach (var item in albums)
        {
            item.ShouldBe(albumId);
        }
    }

    // Helper method to fetch data from the given URL
    public string GetJsonData(string url) 
    {
        var httpClient = new HttpClient();
        Task<string> t = httpClient.GetStringAsync(url);
        string result = t.Result;
        httpClient.Dispose();
        return result;
    }
}
