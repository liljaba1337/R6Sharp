# R6Sharp [![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FSergeantSerk%2FR6Sharp.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FSergeantSerk%2FR6Sharp?ref=badge_shield)
## Description
An (unofficial) API wrapper for Rainbow 6 Siege to get player stats from Ubisoft's website. Although Ubisoft does not provide an official API endpoint for developers to retrieve player data (therefore not explicitly sanctioned), their own pages retrieve it through web requests using JavaScript to specific endpoints.

## Usage
Please see the Example project to see how to use this wrapper. Alternatively, here's an example:
```csharp
public static class Program
{
    public static void Main (string[] args)
    {
        var api = new R6Api("email@email.com", "somepassword");

        // if you want to retrieve some specific uuids, enter like so
        var guids = new Guid[]
        {
            Guid.Parse("00000000-0000-0000-0000-000000000000"),
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Guid.Parse("44444444-4444-4444-4444-444444444444")
        };

        // Searches for a player with name and platform
        var search = api.SearchProfileAsync("PlayerName", R6Api.Platform.Uplay).Result;
        // Gets players' profile using specified uuids and platform
        var profile = api.GetProfileAsync(new Guid[] { search[0].ProfileId }, R6Api.Platform.Uplay).Result;
        // var profile = api.GetProfileAsync(guids, R6Api.Platform.Uplay).Result;
        // Gets players' ranked stats using specified uuids, platform and region
        var ranked = api.GetRankedAsync(guids, R6Api.Platform.Uplay, R6Api.Region.EMEA).Result;
    }
}
```

## Credits
Wrapper powered by Ubisoft Entertainment. All data belongs to Ubisoft and should be attributed to them unless notified otherwise by Ubisoft.

## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FSergeantSerk%2FR6Sharp.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FSergeantSerk%2FR6Sharp?ref=badge_large)
