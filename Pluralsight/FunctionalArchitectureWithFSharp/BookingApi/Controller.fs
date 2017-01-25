namespace BookingApi

open System.Net.Http
open System.Web.Http

type HomeController() =
    inherit ApiController()

    member this.Get() = new HttpResponseMessage()
