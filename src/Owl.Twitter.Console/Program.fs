open System
open Owl.Twitter
open System.Net.Http
open System.Threading.Tasks
open System.IO
open Microsoft.FSharp.Core.CompilerServices

[<Literal>]
let token = "AAAAAAAAAAAAAAAAAAAAACC0WAEAAAAA1BMJcmpLqBTT%2FezRJEE%2FxyswS8I%3D5wsfqgH1Fu8jECBwpAzQGIctab3xQtCVm7pfIgWIbdYdOlsYz0"

[<EntryPoint>]
let main argv =
  sample {
    custom 20
  }
  |> ignore


  let client = Twitter.connect token
  // https://github.com/Xwilarg/TwitterSharp/blob/master/TwitterSharp/Client/TwitterClient.cs#L222

  //let rule'url = "https://api.twitter.com/2/tweets/search/stream/rules"
  //use content = new StringContent("""
  //{
  //  "add": [
  //    {"value": "cat has:images", "tag": "cats with images"}
  //  ]
  //}""")
  
  //use request = new HttpRequestMessage(HttpMethod.Post, rule'url)
  //request.Content <- content
  //request.Headers.Add("ContentType", "application/json")
  //request.Headers.Add("Authorization", $"Bearer %s{client.bearer}")
  //let r = client.http.SendAsync(request).Result
  ////let r = client.http.PostAsync(rule'url, content).Result
  //r.Content.ReadAsStringAsync().Result
  //|> printfn "%s"
  
  //let stream'url = "https://api.twitter.com/2/tweets/search/stream?tweet.fields=created_at&expansions=author_id&user.fields=created_at"
  //use stream = client.http.GetStreamAsync(stream'url).Result
  //use reader = new StreamReader(stream)
  //while not reader.EndOfStream do
  //  let line = reader.ReadLine()
  //  if not (String.IsNullOrWhiteSpace line) then
  //    printfn $"%s{line}"





  //search'recent client {
  //  query "from:Twitter"
  //  end_time (DateTime.Now.AddSeconds -30)
  
  //  expansions Expansions.author'id
  //  add Expansions.attachments'media'keys
  //  add Expansions.geo'place'id

  //  media'fields MediaFields.alt'text
  //  add MediaFields.duration'ms
  //  add MediaFields.url

  //  place'fields PlaceFields.country
  //  add PlaceFields.name
  //  add PlaceFields.id

  //  poll'fields PollFields.duration'minutes
  //  add PollFields.options
  //  add PollFields.id

  //  max_results 10<counts>
  //  search
  //}
  //|> sync
  //|> printfn "%s"

  //printfn "----------"

  //search'all client {
  //  query "from:Twitter"
  //  end_time (DateTime.Now.AddSeconds -30)
  
  //  expansions Expansions.author'id
  //  add Expansions.attachments'media'keys
  //  add Expansions.geo'place'id

  //  media'fields MediaFields.alt'text
  //  add MediaFields.duration'ms
  //  add MediaFields.url

  //  place'fields PlaceFields.country
  //  add PlaceFields.name
  //  add PlaceFields.id

  //  poll'fields PollFields.duration'minutes
  //  add PollFields.options
  //  add PollFields.id

  //  max_results 10<counts>
  //  search
  //  sync
  //}
  //|> printfn "%s"

  //printfn "----------"

  //timelines'tweets client {
  //  id "909795210481119232"

  //  start_time (DateTime.Now.AddDays -1)
  //  end_time (DateTime.Now.AddMinutes -30)

  //  exclude Exclude.retweets
  
  //  expansions Expansions.author'id
  //  add Expansions.attachments'media'keys
  //  add Expansions.attachments'poll'ids
  //  add Expansions.geo'place'id

  //  media'fields MediaFields.alt'text
  //  add MediaFields.duration'ms
  //  add MediaFields.url

  //  place'fields PlaceFields.country
  //  add PlaceFields.name
  //  add PlaceFields.id
  //  add PlaceFields.geo

  //  poll'fields PollFields.duration'minutes
  //  add PollFields.options
  //  add PollFields.id
  //  add PollFields.voting'status

  //  max_results 10<counts>
  //  search
  //  sync
  //}
  //|> printfn "%s"

  //printfn "----------"

  //timelines'mentions client {
  //  id "909795210481119232"
  //  end_time (DateTime.Now.AddSeconds -30)
  
  //  expansions Expansions.author'id
  //  add Expansions.attachments'media'keys
  //  add Expansions.geo'place'id

  //  media'fields MediaFields.alt'text
  //  add MediaFields.duration'ms
  //  add MediaFields.url

  //  place'fields PlaceFields.country
  //  add PlaceFields.name
  //  add PlaceFields.id

  //  poll'fields PollFields.duration'minutes
  //  add PollFields.options
  //  add PollFields.id

  //  max_results 10<counts>
  //  search
  //  sync
  //}
  //|> printfn "%s"

  //printfn "----------"

  //users'by client {
  //  usernames "_midoliy_"
  //  search
  //  sync
  //}
  //|> printfn "%s"

  //printfn "----------"

  //users'by'username client {
  //  username "_midoliy_"
  //  search
  //  sync
  //}
  //|> printfn "%s"

  //printfn "----------"

  //users client {
  //  ids "909795210481119232"
  //  search
  //  sync
  //}
  //|> printfn "%s"

  //printfn "----------"

  //users'id client {
  //  id "909795210481119232"
  //  search
  //  sync
  //}
  //|> printfn "%s"

  client |> Twitter.disconnect
  0
