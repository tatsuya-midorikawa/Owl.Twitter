namespace Owl.Twitter

open System.Net.Http

[<AutoOpen>]
module ParameterUnit =
  [<Measure>]
  type counts

module Twitter =
  type Client = { http: HttpClient; bearer: string }
  let connect bearer = { http = new HttpClient(); bearer = bearer }
  let disconnect client = client.http.Dispose();