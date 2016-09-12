namespace Madko.Samples.HttpApi

open System.Net
open System.Web.Http

[<CLIMutableAttribute>]
type FooRendition = { Id : string; Text : string }

type FooController() =
    inherit ApiController()

    member this.Post (rendition : FooRendition) : IHttpActionResult =
        this.StatusCode HttpStatusCode.Accepted :> _