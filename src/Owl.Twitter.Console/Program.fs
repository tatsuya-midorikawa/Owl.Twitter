open System.Net.Http

let toAsync = Async.AwaitTask
let run = Async.RunSynchronously

[<Literal>]
let token = ""

[<EntryPoint>]
let main _ =
  use client = new HttpClient()
  use request = new HttpRequestMessage(HttpMethod.Get, "https://api.twitter.com/2/tweets/search/recent?max_results=10&query=from:Twitter")
  request.Headers.Add("ContentType", "application/json")
  request.Headers.Add("Authorization", $"Bearer %s{token}")

  task {
    use! r = client.SendAsync(request)
    return! r.Content.ReadAsStringAsync()
  }
  |> (toAsync >> run)
  |> printfn "%s"
  
  0