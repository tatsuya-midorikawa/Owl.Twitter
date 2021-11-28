namespace Owl.Twitter

open System
open System.Linq
open System.Net.Http
open Microsoft.FSharp.Core.CompilerServices

[<AutoOpen>]
module SearchTweets = 
  // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
  [<Literal>]
  let private ep_recent = "https://api.twitter.com/2/tweets/search/recent"
  // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-all
  [<Literal>]
  let private ep_all = "https://api.twitter.com/2/tweets/search/all"

  type SearchRecentTweetsBuilder (client: Twitter.Client) =
    let mutable query = ""
    let mutable since'id = Option<string>.None
    let mutable until'id = Option<string>.None
    let mutable start'time = Option<DateTime>.None
    let mutable end'time = Option<DateTime>.None
    let mutable max'results = Option<int<counts>>.None
    let mutable expansions = ListCollector<string>()
    let mutable media'fields = ListCollector<string>()
    let mutable place'fields = ListCollector<string>()
    let mutable poll'fields = ListCollector<string>()
    let mutable tweet'fields = ListCollector<string>()    
    let mutable user'fields = ListCollector<string>()

    member __.Yield (_: unit) = ()
    member __.Zero() = ()
    
    // ■ List of Query operators
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/integrate/build-a-query#list
    [<CustomOperation("query")>]
    member __.Query(_: unit, q: string) = query <- q
    
    // ■ since_id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("since_id")>]
    member __.SinceId(_: unit, id: string) = since'id <- Option.Some(id)

    // ■ until_id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("until_id")>]
    member __.UntileId(_: unit, id: string) = until'id <- Option.Some(id)
    
    // ■ start_time
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("start_time")>]
    member __.SetStartTime(_: unit, st: DateTime) = start'time <- Option.Some(st)

    // ■ end_time
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("end_time")>]
    member __.SetEndTime(_: unit, et: DateTime) = end'time <- Option.Some(et)

    // ■ expansions
    // https://developer.twitter.com/en/docs/twitter-api/expansions
    [<CustomOperation("expansions")>]
    member __.Add(_: unit, exp: Expansions) = expansions.Add(exp.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, exp: Expansions) = expansions.Add(exp.value)
    [<CustomOperation("expansions")>]
    member __.AddMany(_: unit, exp: Expansions[]) = exp |> Array.map (fun e -> e.value) |> expansions.AddMany
    
    // ■ max_results
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("max_results")>]
    member __.Set(_: unit, count: int<counts>) = max'results <- Option.Some(count)

    // ■ media.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/media
    [<CustomOperation("media'fields")>]
    member __.Add(_: unit, mf: MediaFields) = media'fields.Add(mf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, mf: MediaFields) = media'fields.Add(mf.value)
    [<CustomOperation("media'fields")>]
    member __.AddMany(_: unit, mf: MediaFields[]) = mf |> Array.map (fun e -> e.value) |> media'fields.AddMany

    // ■ place.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/place
    [<CustomOperation("place'fields")>]
    member __.Add(_: unit, pf: PlaceFields) = place'fields.Add(pf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, pf: PlaceFields) = place'fields.Add(pf.value)
    [<CustomOperation("place'fields")>]
    member __.AddMany(_: unit, pf: PlaceFields[]) = pf |> Array.map (fun e -> e.value) |> place'fields.AddMany

    // ■ poll.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/poll
    [<CustomOperation("poll'fields")>]
    member __.Add(_: unit, pf: PollFields) = poll'fields.Add(pf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, pf: PollFields) = poll'fields.Add(pf.value)
    [<CustomOperation("poll'fields")>]
    member __.AddMany(_: unit, pf: PollFields[]) = pf |> Array.map (fun e -> e.value) |> poll'fields.AddMany

    // ■ tweet.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/tweet
    [<CustomOperation("tweet'fields")>]
    member __.Add(_: unit, tf: TweetFields) = tweet'fields.Add(tf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, tf: TweetFields) = tweet'fields.Add(tf.value)
    [<CustomOperation("tweet'fields")>]
    member __.AddMany(_: unit, tf: TweetFields[]) = tf |> Array.map (fun e -> e.value) |> tweet'fields.AddMany

    // ■ user.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/user
    [<CustomOperation("user'fields")>]
    member __.Add(_: unit, uf: UserFields) = user'fields.Add(uf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, uf: UserFields) = user'fields.Add(uf.value)
    [<CustomOperation("user'fields")>]
    member __.AddMany(_: unit, uf: UserFields[]) = uf |> Array.map (fun e -> e.value) |> user'fields.AddMany

    [<CustomOperation("search")>]
    member __.Search(_: unit) =
      if String.IsNullOrEmpty(query) then raise (ArgumentException("'query' must be called."))

      let mutable params' = ListCollector<string>()

      // query
      params'.Add($"query={query}")
      // since_id
      since'id |> Option.iter (fun id -> params'.Add $"since_id={id}")
      // until_id
      until'id |> Option.iter (fun id -> params'.Add $"until_id={id}")
      // start_time
      start'time |> Option.iter (fun time -> params'.Add$"""start_time={time |> to_string}""")
      // end_time
      end'time |> Option.iter (fun time -> params'.Add$"""end_time={time |> to_string}""")
      // expansions
      let expansions = (',', expansions.Close()) |> String.Join
      if String.IsNullOrEmpty(expansions) |> not then params'.Add $"expansions=%s{expansions}"
      // media.fields
      let mf = media'fields.Close() |> join
      if String.IsNullOrEmpty(mf) |> not then params'.Add $"media.fields=%s{mf}"
      // place.fields
      let places = place'fields.Close() |> join
      if String.IsNullOrEmpty(places) |> not then params'.Add $"place.fields=%s{places}"
      // poll.fields
      let polls = poll'fields.Close() |> join
      if String.IsNullOrEmpty(polls) |> not then params'.Add $"poll.fields=%s{polls}"
      // tweet.fields
      let tweets = tweet'fields.Close() |> join
      if String.IsNullOrEmpty(tweets) |> not then params'.Add $"tweets.fields=%s{tweets}"
      // user.fields
      let users = user'fields.Close() |> join
      if String.IsNullOrEmpty(users) |> not then params'.Add $"user.fields=%s{users}"
      // max_results
      if max'results.IsSome then params'.Add $"max_results=%d{max'results.Value}"
      
      let p =  ('&', params'.Close()) |> String.Join

      use request = new HttpRequestMessage(HttpMethod.Get, $"{ep_recent}?{p}")
      request.Headers.Add("ContentType", "application/json")
      request.Headers.Add("Authorization", $"Bearer %s{client.bearer}")

      task {
        use! r = client.http.SendAsync(request)
        return! r.Content.ReadAsStringAsync()
      }
      
    [<CustomOperation("sync")>]
    member __.Sync(task: System.Threading.Tasks.Task<'T>) =
      System.Threading.Tasks.Task.WaitAll task
      task.Result

  type SearchAllTweetsBuilder (client: Twitter.Client) =
    let mutable query = ""
    let mutable since'id = Option<string>.None
    let mutable until'id = Option<string>.None
    let mutable start'time = Option<DateTime>.None
    let mutable end'time = Option<DateTime>.None
    let mutable max'results = Option<int<counts>>.None
    let mutable expansions = ListCollector<string>()
    let mutable media'fields = ListCollector<string>()
    let mutable place'fields = ListCollector<string>()
    let mutable poll'fields = ListCollector<string>()
    let mutable tweet'fields = ListCollector<string>()    
    let mutable user'fields = ListCollector<string>()

    member __.Yield (_: unit) = ()
    member __.Zero() = ()
    
    // ■ List of Query operators
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/integrate/build-a-query#list
    [<CustomOperation("query")>]
    member __.Query(_: unit, q: string) = query <- q
    
    // ■ since_id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("since_id")>]
    member __.SinceId(_: unit, id: string) = since'id <- Option.Some(id)

    // ■ until_id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("until_id")>]
    member __.UntileId(_: unit, id: string) = until'id <- Option.Some(id)
    
    // ■ start_time
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("start_time")>]
    member __.SetStartTime(_: unit, st: DateTime) = start'time <- Option.Some(st)

    // ■ end_time
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("end_time")>]
    member __.SetEndTime(_: unit, et: DateTime) = end'time <- Option.Some(et)

    // ■ expansions
    // https://developer.twitter.com/en/docs/twitter-api/expansions
    [<CustomOperation("expansions")>]
    member __.Add(_: unit, exp: Expansions) = expansions.Add(exp.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, exp: Expansions) = expansions.Add(exp.value)
    [<CustomOperation("expansions")>]
    member __.AddMany(_: unit, exp: Expansions[]) = exp |> Array.map (fun e -> e.value) |> expansions.AddMany
    
    // ■ max_results
    // https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
    [<CustomOperation("max_results")>]
    member __.Set(_: unit, count: int<counts>) = max'results <- Option.Some(count)

    // ■ media.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/media
    [<CustomOperation("media'fields")>]
    member __.Add(_: unit, mf: MediaFields) = media'fields.Add(mf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, mf: MediaFields) = media'fields.Add(mf.value)
    [<CustomOperation("media'fields")>]
    member __.AddMany(_: unit, mf: MediaFields[]) = mf |> Array.map (fun e -> e.value) |> media'fields.AddMany

    // ■ place.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/place
    [<CustomOperation("place'fields")>]
    member __.Add(_: unit, pf: PlaceFields) = place'fields.Add(pf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, pf: PlaceFields) = place'fields.Add(pf.value)
    [<CustomOperation("place'fields")>]
    member __.AddMany(_: unit, pf: PlaceFields[]) = pf |> Array.map (fun e -> e.value) |> place'fields.AddMany

    // ■ poll.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/poll
    [<CustomOperation("poll'fields")>]
    member __.Add(_: unit, pf: PollFields) = poll'fields.Add(pf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, pf: PollFields) = poll'fields.Add(pf.value)
    [<CustomOperation("poll'fields")>]
    member __.AddMany(_: unit, pf: PollFields[]) = pf |> Array.map (fun e -> e.value) |> poll'fields.AddMany

    // ■ tweet.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/tweet
    [<CustomOperation("tweet'fields")>]
    member __.Add(_: unit, tf: TweetFields) = tweet'fields.Add(tf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, tf: TweetFields) = tweet'fields.Add(tf.value)
    [<CustomOperation("tweet'fields")>]
    member __.AddMany(_: unit, tf: TweetFields[]) = tf |> Array.map (fun e -> e.value) |> tweet'fields.AddMany

    // ■ user.fields
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/user
    [<CustomOperation("user'fields")>]
    member __.Add(_: unit, uf: UserFields) = user'fields.Add(uf.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, uf: UserFields) = user'fields.Add(uf.value)
    [<CustomOperation("user'fields")>]
    member __.AddMany(_: unit, uf: UserFields[]) = uf |> Array.map (fun e -> e.value) |> user'fields.AddMany

    [<CustomOperation("search")>]
    member __.Search(_: unit) =
      if String.IsNullOrEmpty(query) then raise (ArgumentException("'query' must be called."))

      let mutable params' = ListCollector<string>()

      // query
      params'.Add($"query={query}")
      // since_id
      since'id |> Option.iter (fun id -> params'.Add $"since_id={id}")
      // until_id
      until'id |> Option.iter (fun id -> params'.Add $"until_id={id}")
      // start_time
      start'time |> Option.iter (fun time -> params'.Add$"""start_time={time |> to_string}""")
      // end_time
      end'time |> Option.iter (fun time -> params'.Add$"""end_time={time |> to_string}""")
      // expansions
      let expansions = (',', expansions.Close()) |> String.Join
      if String.IsNullOrEmpty(expansions) |> not then params'.Add $"expansions=%s{expansions}"
      // media.fields
      let mf = media'fields.Close() |> join
      if String.IsNullOrEmpty(mf) |> not then params'.Add $"media.fields=%s{mf}"
      // place.fields
      let places = place'fields.Close() |> join
      if String.IsNullOrEmpty(places) |> not then params'.Add $"place.fields=%s{places}"
      // poll.fields
      let polls = poll'fields.Close() |> join
      if String.IsNullOrEmpty(polls) |> not then params'.Add $"poll.fields=%s{polls}"
      // tweet.fields
      let tweets = tweet'fields.Close() |> join
      if String.IsNullOrEmpty(tweets) |> not then params'.Add $"tweets.fields=%s{tweets}"
      // user.fields
      let users = user'fields.Close() |> join
      if String.IsNullOrEmpty(users) |> not then params'.Add $"user.fields=%s{users}"
      // max_results
      if max'results.IsSome then params'.Add $"max_results=%d{max'results.Value}"
      
      let p =  ('&', params'.Close()) |> String.Join

      use request = new HttpRequestMessage(HttpMethod.Get, $"{ep_all}?{p}")
      request.Headers.Add("ContentType", "application/json")
      request.Headers.Add("Authorization", $"Bearer %s{client.bearer}")

      task {
        use! r = client.http.SendAsync(request)
        return! r.Content.ReadAsStringAsync()
      }
      
    [<CustomOperation("sync")>]
    member __.Sync(task: System.Threading.Tasks.Task<'T>) =
      System.Threading.Tasks.Task.WaitAll task
      task.Result

  let search'recent client = SearchRecentTweetsBuilder client
  let search'all client = SearchAllTweetsBuilder client


  