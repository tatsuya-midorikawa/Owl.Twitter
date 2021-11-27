namespace Owl.Twitter

open System.Net.Http

[<AutoOpen>]
module ParameterUnit =
  [<Measure>]
  type counts

[<AutoOpen>]
module Functions = 
  let sync (t: System.Threading.Tasks.Task<'T>) = 
    System.Threading.Tasks.Task.WaitAll t
    t.Result

module Twitter =
  type Client = { http: HttpClient; bearer: string }
  let connect bearer = { http = new HttpClient(); bearer = bearer }
  let disconnect client = client.http.Dispose();