[<AutoOpen>]
module Dev

type Builder () =
  member _.Yield v = 
    printfn "yield"
    v
  member _.For (v, f) =
    printfn "for"
    f v
  member _.Return v =
    printfn "return"
    v
  [<CustomOperation("custom", AllowIntoPattern=true)>]
  member _.Custom (v, [<ProjectionParameter>] f) = f v

let sample = Builder()
