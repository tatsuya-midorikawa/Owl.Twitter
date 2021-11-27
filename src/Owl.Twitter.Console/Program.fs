open System
open Owl.Twitter

[<Literal>]
let token = ""

let client = Twitter.connect token

search'recent client {
  query "from:Twitter"
  end_time (DateTime.Now.AddSeconds -30)
  
  expansions Expansions.author'id
  add Expansions.attachments'media'keys
  add Expansions.geo'place'id

  media'fields MediaFields.alt'text
  add MediaFields.duration'ms
  add MediaFields.url

  place'fields PlaceFields.country
  add PlaceFields.name
  add PlaceFields.id

  poll'fields PollFields.duration'minutes
  add PollFields.options
  add PollFields.id

  max_results 10<counts>
  search
}
|> sync
|> printfn "%s"

printfn "----------"

search'all client {
  query "from:Twitter"
  end_time (DateTime.Now.AddSeconds -30)
  
  expansions Expansions.author'id
  add Expansions.attachments'media'keys
  add Expansions.geo'place'id

  media'fields MediaFields.alt'text
  add MediaFields.duration'ms
  add MediaFields.url

  place'fields PlaceFields.country
  add PlaceFields.name
  add PlaceFields.id

  poll'fields PollFields.duration'minutes
  add PollFields.options
  add PollFields.id

  max_results 10<counts>
  search
  sync
}
|> printfn "%s"

printfn "----------"

timelines'tweets client {
  id ""
  end_time (DateTime.Now.AddSeconds -30)
  
  expansions Expansions.author'id
  add Expansions.attachments'media'keys
  add Expansions.geo'place'id

  media'fields MediaFields.alt'text
  add MediaFields.duration'ms
  add MediaFields.url

  place'fields PlaceFields.country
  add PlaceFields.name
  add PlaceFields.id

  poll'fields PollFields.duration'minutes
  add PollFields.options
  add PollFields.id

  max_results 10<counts>
  search
  sync
}
|> printfn "%s"

printfn "----------"

timelines'mentions client {
  id ""
  end_time (DateTime.Now.AddSeconds -30)
  
  expansions Expansions.author'id
  add Expansions.attachments'media'keys
  add Expansions.geo'place'id

  media'fields MediaFields.alt'text
  add MediaFields.duration'ms
  add MediaFields.url

  place'fields PlaceFields.country
  add PlaceFields.name
  add PlaceFields.id

  poll'fields PollFields.duration'minutes
  add PollFields.options
  add PollFields.id

  max_results 10<counts>
  search
  sync
}
|> printfn "%s"

printfn "----------"

users'by  client {
  usernames "_midoliy_"
  search
  sync
}
|> printfn "%s"

printfn "----------"

users'by'username  client {
  username "_midoliy_"
  search
  sync
}
|> printfn "%s"

client |> Twitter.disconnect
