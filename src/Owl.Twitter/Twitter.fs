namespace Owl.Twitter

open System
open System.Net.Http
open System.Net.Http.Headers
open Microsoft.FSharp.Core.CompilerServices

[<AutoOpen>]
module ParameterUnit =
  [<Measure>]
  type counts

[<AutoOpen>]
module Functions = 
  let sync (t: System.Threading.Tasks.Task<'T>) = 
    System.Threading.Tasks.Task.WaitAll t
    t.Result

module String = 
  let internal iter (f: string -> unit) (s: string) = if not (String.IsNullOrWhiteSpace s) then f s
  let internal join (separator: char) (xs: string list) = String.Join(separator, xs)

[<AutoOpen>]
module Types = 
  let internal to_string (dt: DateTime) = dt.ToString("yyyy-MM-ddTHH:mm:ssZ")

  type Expansions = 
    { value: string }
    static member attachments'poll'ids = { value = "attachments.poll_ids" }
    static member attachments'media'keys = { value = "attachments.media_keys" }
    static member author'id = { value = "author_id" }
    static member entities'mentions'username = { value = "entities.mentions.username" }
    static member geo'place'id = { value = "geo.place_id" }
    static member in'reply'to'user'id = { value = "in_reply_to_user_id" }
    static member referenced'tweets'id = { value = "referenced_tweets.id" }
    static member referenced'tweets'id'author'id = { value = "referenced_tweets.id.author_id" }

  type Exclude = 
    { value: string }
    static member retweets = { value = "retweets" }
    static member replies = { value = "replies" }
    
  type PaginationToken = 
    { value: string }
    static member next'token = { value = "next_token" }
    static member previous'token = { value = "previous_token" }

  type MediaFields =
    { value: string }
    static member duration'ms = { value = "duration_ms" }
    static member height = { value = "height" }
    static member media'key = { value = "media_key" }
    static member preview'image'url = { value = "preview_image_url" }
    static member type' = { value = "type" }
    static member url = { value = "url" }
    static member width = { value = "width" }
    static member public'metrics = { value = "public_metrics" }
    static member non'public'metrics = { value = "non_public_metrics" }
    static member organic'metrics = { value = "organic_metrics" }
    static member promoted'metrics = { value = "promoted_metrics" }
    static member alt'text = { value = "alt_text" }

  type PlaceFields = 
    { value: string }
    static member contained'within = { value = "contained_within" }
    static member country = { value = "country" }
    static member country'code = { value = "country_code" }
    static member full'name = { value = "full_name" }
    static member geo = { value = "geo" }
    static member id = { value = "id" }
    static member name = { value = "name" }
    static member place'type = { value = "place_type" }

  type PollFields =
    { value: string }
    static member duration'minutes = { value = "duration_minutes" }
    static member end'datetime = { value = "end_datetime" }
    static member id = { value = "id" }
    static member options = { value = "options" }
    static member voting'status = { value = "voting_status" }

  type TweetFields =
    { value: string }
    static member attachments = { value = "attachments" }
    static member author'id = { value = "tweet_author_id" }
    static member context'annotations = { value = "context_annotations" }
    static member conversation'id = { value = "conversation_id" }
    static member created'at = { value = "created_at" }
    static member entities = { value = "entities" }
    static member geo = { value = "geo" }
    static member id = { value = "id" }
    static member in'reply'to'user'id = { value = "in_reply_to_user_id" }
    static member lang = { value = "lang" }
    static member non'public'metrics = { value = "non_public_metrics" }
    static member public'metrics = { value = "public_metrics" }
    static member organic'metrics = { value = "organic_metrics" }
    static member promoted'metrics = { value = "promoted_metrics" }
    static member possibly'sensitive = { value = "possibly_sensitive" }
    static member referenced'tweets = { value = "referenced_tweets" }
    static member reply'settings = { value = "reply_settings" }
    static member source = { value = "source" }
    static member text = { value = "text" }
    static member withheld = { value = "withheld" }
  
  type UserFields =
    { value: string }
    static member created'at = { value = "created_at" }
    static member description = { value = "description" }
    static member entities = { value = "entities" }
    static member id = { value = "id" }
    static member location = { value = "location" }
    static member name = { value = "name" }
    static member pinned_tweet_id = { value = "pinned_tweet_id" }
    static member profile_image_url = { value = "profile_image_url" }
    static member protected' = { value = "desprotectedcription" }
    static member public'metrics = { value = "public_metrics" }
    static member url = { value = "url" }
    static member username = { value = "username" }
    static member verified = { value = "verified" }
    static member withheld = { value = "withheld" }

  type internal Parameter = {
    mutable exclude: Option<string>
    mutable since'id: Option<string>
    mutable until'id: Option<string>
    mutable start'time: Option<DateTime>
    mutable end'time: Option<DateTime>
    mutable max'results: Option<int<counts>>
    mutable pagination'token: Option<string>
    mutable expansions: ListCollector<string>
    mutable media'fields: ListCollector<string>
    mutable place'fields: ListCollector<string>
    mutable poll'fields: ListCollector<string>
    mutable tweet'fields: ListCollector<string>
    mutable user'fields: ListCollector<string>
  }
  with
    member __.build () = 
      let mutable params' = ListCollector<string>()

      __.exclude |> Option.iter (fun x -> params'.Add $"exclude=%s{x}")
      __.since'id |> Option.iter (fun x -> params'.Add $"since_id=%s{x}")
      __.until'id |> Option.iter (fun x -> params'.Add $"until_id=%s{x}")
      __.start'time |> Option.iter (fun x -> params'.Add $"start_time=%s{to_string x}")
      __.end'time |> Option.iter (fun x -> params'.Add $"end_time=%s{to_string x}")
      __.max'results |> Option.iter (fun x -> params'.Add $"max_results=%d{x}")
      __.pagination'token |> Option.iter (fun x -> params'.Add $"pagination_token=%s{x}")
      __.expansions.Close() |> String.join ',' |> String.iter (fun x -> params'.Add $"expansions=%s{x}")
      __.media'fields.Close() |> String.join ',' |> String.iter (fun x -> params'.Add $"media.fields=%s{x}")
      __.place'fields.Close() |> String.join ',' |> String.iter (fun x -> params'.Add $"place.fields=%s{x}")
      __.poll'fields.Close() |> String.join ',' |> String.iter (fun x -> params'.Add $"poll.fields=%s{x}")
      __.tweet'fields.Close() |> String.join ',' |> String.iter (fun x -> params'.Add $"tweets.fields=%s{x}")
      __.user'fields.Close() |> String.join ',' |> String.iter (fun x -> params'.Add $"user.fields=%s{x}")
      
      params'.Close() |> String.join '&'

module Twitter =
  type Client = { mutable http: HttpClient; bearer: string }
  let connect bearer = 
    let client = { http = new HttpClient(); bearer = bearer }
    client.http.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Bearer", "bearer")
    client
  let disconnect client = client.http.Dispose();

