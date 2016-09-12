module Madko.Samples.HttpApi.Tests

open System.Net
open System.Web.Http
open Xunit
open Swensen.Unquote
open Madko.Samples.HttpApi

let convertsTo<'a> candidate =
    match box candidate with
    | :? 'a as converted -> Some converted
    | _ -> None

[<Fact>]
let ``Post returns correct result`` () =
    use sut = new FooController ()
    let foo = { Id = "1234"; Text = "Bar" }

    let actual : IHttpActionResult = sut.Post foo

    test <@ 
            actual
            |> convertsTo<Results.StatusCodeResult>
            |> Option.map (fun x -> x.StatusCode)
            |> Option.exists ((=) HttpStatusCode.Accepted) @>