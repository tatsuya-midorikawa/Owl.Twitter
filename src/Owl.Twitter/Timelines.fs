namespace Owl.Twitter

open System
open System.Linq
open System.Net.Http
open Microsoft.FSharp.Core.CompilerServices

[<AutoOpen>]
module Timelines = 
  // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
  let private ep_tweets id = $"https://api.twitter.com/2/users/%s{id}/tweets"
  // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-mentions
  let private ep_mentions id = $"https://api.twitter.com/2/users/%s{id}/mentions"

  type TimelinesTweetsBuilder (client: Twitter.Client) =
    let mutable id = ""
    let mutable since'id = Option<string>.None
    let mutable until'id = Option<string>.None
    let mutable start'time = Option<DateTime>.None
    let mutable end'time = Option<DateTime>.None
    let mutable exclude = Option<string>.None
    let mutable pagination'token = Option<string>.None

    let mutable max'results = Option<int<counts>>.None
    let mutable expansions = ListCollector<string>()
    let mutable media'fields = ListCollector<string>()
    let mutable place'fields = ListCollector<string>()
    let mutable poll'fields = ListCollector<string>()
    let mutable tweet'fields = ListCollector<string>()    
    let mutable user'fields = ListCollector<string>()

    member __.Yield (_: unit) = ()
    member __.Zero() = ()
    
    // ■ id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("id")>]
    member __.Id(_: unit, id': string) = id <- id'
    
    // ■ since_id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("since_id")>]
    member __.SinceId(_: unit, id: string) = since'id <- Option.Some(id)

    // ■ until_id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("until_id")>]
    member __.UntileId(_: unit, id: string) = until'id <- Option.Some(id)
    
    // ■ start_time
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("start_time")>]
    member __.SetStartTime(_: unit, st: DateTime) = start'time <- Option.Some(st)

    // ■ end_time
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("end_time")>]
    member __.SetEndTime(_: unit, et: DateTime) = end'time <- Option.Some(et)

    // ■ exclude
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("exclude")>]
    member __.Set(_: unit, exclude': Exclude) = exclude <- Option.Some(exclude'.value)

    // ■ pagination_token
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("pagination_token")>]
    member __.Set(_: unit, pagination'token': PaginationToken) = pagination'token <- Option.Some(pagination'token'.value)

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
    
    [<CustomOperation("sync")>]
    member __.Sync(task: System.Threading.Tasks.Task<'T>) =
      System.Threading.Tasks.Task.WaitAll task
      task.Result

    [<CustomOperation("search")>]
    member __.Search(_: unit) =
      if String.IsNullOrEmpty(id) then raise (ArgumentException("'id' must be called."))

      let ep = ep_tweets id
      let mutable params' = ListCollector<string>()

      // since_id
      since'id |> Option.iter (fun id -> params'.Add $"since_id={id}")
      // until_id
      until'id |> Option.iter (fun id -> params'.Add $"until_id={id}")
      // start_time
      start'time |> Option.iter (fun time -> params'.Add$"""start_time={time |> to_string}""")
      // end_time
      end'time |> Option.iter (fun time -> params'.Add$"""end_time={time |> to_string}""")
      // exclude
      exclude |> Option.iter (fun e -> params'.Add$"""exclude={e}""")
      // pagination_token
      pagination'token |> Option.iter (fun p -> params'.Add$"""pagination_token={p}""")
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
      if String.IsNullOrEmpty(polls) |> not then params'.Add $"place.fields=%s{polls}"
      // tweet.fields
      let tweets = tweet'fields.Close() |> join
      if String.IsNullOrEmpty(tweets) |> not then params'.Add $"tweets.fields=%s{tweets}"
      // user.fields
      let users = user'fields.Close() |> join
      if String.IsNullOrEmpty(users) |> not then params'.Add $"user.fields=%s{users}"
      // max_results
      if max'results.IsSome then params'.Add $"max_results=%d{max'results.Value}"
      
      let p =  ('&', params'.Close()) |> String.Join

      use request = new HttpRequestMessage(HttpMethod.Get, $"{ep}?{p}")
      request.Headers.Add("ContentType", "application/json")
      request.Headers.Add("Authorization", $"Bearer %s{client.bearer}")

      task {
        use! r = client.http.SendAsync(request)
        return! r.Content.ReadAsStringAsync()
      }
      
  type TimelinesMentionsBuilder (client: Twitter.Client) =
    let mutable id = ""
    let mutable since'id = Option<string>.None
    let mutable until'id = Option<string>.None
    let mutable start'time = Option<DateTime>.None
    let mutable end'time = Option<DateTime>.None
    let mutable exclude = Option<string>.None
    let mutable pagination'token = Option<string>.None

    let mutable max'results = Option<int<counts>>.None
    let mutable expansions = ListCollector<string>()
    let mutable media'fields = ListCollector<string>()
    let mutable place'fields = ListCollector<string>()
    let mutable poll'fields = ListCollector<string>()
    let mutable tweet'fields = ListCollector<string>()    
    let mutable user'fields = ListCollector<string>()

    member __.Yield (_: unit) = ()
    member __.Zero() = ()
    
    // ■ id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("id")>]
    member __.Id(_: unit, id': string) = id <- id'
    
    // ■ since_id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("since_id")>]
    member __.SinceId(_: unit, id: string) = since'id <- Option.Some(id)

    // ■ until_id
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("until_id")>]
    member __.UntileId(_: unit, id: string) = until'id <- Option.Some(id)
    
    // ■ start_time
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("start_time")>]
    member __.SetStartTime(_: unit, st: DateTime) = start'time <- Option.Some(st)

    // ■ end_time
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("end_time")>]
    member __.SetEndTime(_: unit, et: DateTime) = end'time <- Option.Some(et)

    // ■ exclude
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("exclude")>]
    member __.Set(_: unit, exclude': Exclude) = exclude <- Option.Some(exclude'.value)

    // ■ pagination_token
    // https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets
    [<CustomOperation("pagination_token")>]
    member __.Set(_: unit, pagination'token': PaginationToken) = pagination'token <- Option.Some(pagination'token'.value)

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
    
    [<CustomOperation("sync")>]
    member __.Sync(task: System.Threading.Tasks.Task<'T>) =
      System.Threading.Tasks.Task.WaitAll task
      task.Result

    [<CustomOperation("search")>]
    member __.Search(_: unit) =
      if String.IsNullOrEmpty(id) then raise (ArgumentException("'id' must be called."))

      let ep = ep_mentions id
      let mutable params' = ListCollector<string>()

      // since_id
      since'id |> Option.iter (fun id -> params'.Add $"since_id={id}")
      // until_id
      until'id |> Option.iter (fun id -> params'.Add $"until_id={id}")
      // start_time
      start'time |> Option.iter (fun time -> params'.Add$"""start_time={time |> to_string}""")
      // end_time
      end'time |> Option.iter (fun time -> params'.Add$"""end_time={time |> to_string}""")
      // exclude
      exclude |> Option.iter (fun e -> params'.Add$"""exclude={e}""")
      // pagination_token
      pagination'token |> Option.iter (fun p -> params'.Add$"""pagination_token={p}""")
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
      if String.IsNullOrEmpty(polls) |> not then params'.Add $"place.fields=%s{polls}"
      // tweet.fields
      let tweets = tweet'fields.Close() |> join
      if String.IsNullOrEmpty(tweets) |> not then params'.Add $"tweets.fields=%s{tweets}"
      // user.fields
      let users = user'fields.Close() |> join
      if String.IsNullOrEmpty(users) |> not then params'.Add $"user.fields=%s{users}"
      // max_results
      if max'results.IsSome then params'.Add $"max_results=%d{max'results.Value}"
      
      let p =  ('&', params'.Close()) |> String.Join

      use request = new HttpRequestMessage(HttpMethod.Get, $"{ep}?{p}")
      request.Headers.Add("ContentType", "application/json")
      request.Headers.Add("Authorization", $"Bearer %s{client.bearer}")

      task {
        use! r = client.http.SendAsync(request)
        return! r.Content.ReadAsStringAsync()
      }

  let timelines'tweets client = TimelinesTweetsBuilder client
  let timelines'mentions client = TimelinesMentionsBuilder client


  