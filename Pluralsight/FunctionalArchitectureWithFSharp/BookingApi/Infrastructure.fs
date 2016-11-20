module Booking.HttpApi.HttpHost

open System.Web.Http

type HttpRouteDefaults = { Controller: string; Id: obj }

let ConfigureRoutes (config : HttpConfiguration) =
    config.Routes.MapHttpRoute(
            "DefaultApi", // Route name
            "api/{controller}/{id}", // URL with parameters
            { Controller = "Home"; Id = RouteParameter.Optional } // Parameter defaults
            ) |> ignore

let Configure = ConfigureRoutes