open System
open Owl.Twitter

[<Literal>]
let token = ""

let client = Twitter.connect token

recent'search client {
  query "from:Twitter"
  end_time (DateTime.Now.AddSeconds -30)
  
  expansions Expansions.author'id
  add Expansions.attachments'media'keys
  add Expansions.geo'place'id

  max_results 10<counts>
  search
}
|> sync
|> printfn "%s"

printfn "----------"

recent'search client {
  query "from:Twitter"
  end_time (DateTime.Now.AddSeconds -30)
  
  expansions Expansions.author'id
  add Expansions.attachments'media'keys
  add Expansions.geo'place'id

  max_results 10<counts>
  search
  sync
}
|> printfn "%s"

client |> Twitter.disconnect