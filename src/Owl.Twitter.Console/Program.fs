open Owl.Twitter

let run = Async.AwaitTask >> Async.RunSynchronously

[<Literal>]
let token = ""

[<EntryPoint>]
let main _ =
  let client = Twitter.connect token

  recent'search client {
    query "from:Twitter"
    max_results 10<counts>
    search
  }
  |> run
  |> printfn "%s"

  client |> Twitter.disconnect
  0