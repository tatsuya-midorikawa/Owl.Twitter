namespace Owl.Twitter

open System
open System.Linq
open System.Net.Http
open Microsoft.FSharp.Core.CompilerServices

[<AutoOpen>]
module UsersLookup = 
  // https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-by
  [<Literal>]
  let private ep_users_by = "https://api.twitter.com/2/users/by"
  // https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-by-username-username
  let private ep_users_by_username name = $"https://api.twitter.com/2/users/by/username/%s{name}"

  type UsersByBuilder (client: Twitter.Client) =
    let mutable usernames = ListCollector<string>()
    let mutable expansions = ListCollector<string>()
    let mutable tweet'fields = ListCollector<string>()
    let mutable user'fields = ListCollector<string>()

    member __.Yield (_: unit) = ()
    member __.Zero() = ()
    
    // ■ usernames
    // https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-by
    [<CustomOperation("usernames")>]
    member __.Usernames(_: unit, names: string) = usernames.Add(names)
    [<CustomOperation("add")>]
    member __.Add(_: unit, names: string) = usernames.Add(names)
    [<CustomOperation("usernames")>]
    member __.Usernames(_: unit, names: #seq<string>) = usernames.AddMany(names)

    // ■ expansions
    // https://developer.twitter.com/en/docs/twitter-api/expansions
    [<CustomOperation("expansions")>]
    member __.Add(_: unit, exp: Expansions) = expansions.Add(exp.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, exp: Expansions) = expansions.Add(exp.value)
    [<CustomOperation("expansions")>]
    member __.AddMany(_: unit, exp: Expansions[]) = exp |> Array.map (fun e -> e.value) |> expansions.AddMany

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
      // required
      let usernames = usernames.Close() |> join
      if String.IsNullOrEmpty(usernames) then raise (ArgumentException("'usernames' must be called."))

      let mutable params' = ListCollector<string>()
      // usernames
      params'.Add $"usernames=%s{usernames}"
      // expansions
      let expansions = expansions.Close() |> join
      if String.IsNullOrEmpty(expansions) |> not then params'.Add $"expansions=%s{expansions}"
      // tweet.fields
      let tweets = tweet'fields.Close() |> join
      if String.IsNullOrEmpty(tweets) |> not then params'.Add $"tweets.fields=%s{tweets}"
      // user.fields
      let users = user'fields.Close() |> join
      if String.IsNullOrEmpty(users) |> not then params'.Add $"user.fields=%s{users}"
      
      let p =  ('&', params'.Close()) |> String.Join

      use request = new HttpRequestMessage(HttpMethod.Get, $"{ep_users_by}?{p}")
      request.Headers.Add("ContentType", "application/json")
      request.Headers.Add("Authorization", $"Bearer %s{client.bearer}")

      task {
        use! r = client.http.SendAsync(request)
        return! r.Content.ReadAsStringAsync()
      }
  
  type UsersByUsernameBuilder (client: Twitter.Client) =
    let mutable username = ""
    let mutable expansions = ListCollector<string>()
    let mutable tweet'fields = ListCollector<string>()
    let mutable user'fields = ListCollector<string>()

    member __.Yield (_: unit) = ()
    member __.Zero() = ()
    
    // ■ username
    // https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-by-username-username
    [<CustomOperation("username")>]
    member __.Usernames(_: unit, name: string) = username <- name

    // ■ expansions
    // https://developer.twitter.com/en/docs/twitter-api/expansions
    [<CustomOperation("expansions")>]
    member __.Add(_: unit, exp: Expansions) = expansions.Add(exp.value)
    [<CustomOperation("add")>]
    member __.And(_: unit, exp: Expansions) = expansions.Add(exp.value)
    [<CustomOperation("expansions")>]
    member __.AddMany(_: unit, exp: Expansions[]) = exp |> Array.map (fun e -> e.value) |> expansions.AddMany

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
      // required
      if String.IsNullOrEmpty(username) then raise (ArgumentException("'username' must be called."))

      let mutable params' = ListCollector<string>()
      // expansions
      let expansions = expansions.Close() |> join
      if String.IsNullOrEmpty(expansions) |> not then params'.Add $"expansions=%s{expansions}"
      // tweet.fields
      let tweets = tweet'fields.Close() |> join
      if String.IsNullOrEmpty(tweets) |> not then params'.Add $"tweets.fields=%s{tweets}"
      // user.fields
      let users = user'fields.Close() |> join
      if String.IsNullOrEmpty(users) |> not then params'.Add $"user.fields=%s{users}"
      
      let p =  ('&', params'.Close()) |> String.Join

      use request = new HttpRequestMessage(HttpMethod.Get, $"{ep_users_by_username username}?{p}")
      request.Headers.Add("ContentType", "application/json")
      request.Headers.Add("Authorization", $"Bearer %s{client.bearer}")

      task {
        use! r = client.http.SendAsync(request)
        return! r.Content.ReadAsStringAsync()
      }
      

  let users'by client = UsersByBuilder client
  let users'by'username client = UsersByUsernameBuilder client


  