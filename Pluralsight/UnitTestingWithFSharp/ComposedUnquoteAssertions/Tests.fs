module Madko.Samples.HttpApi.Tests

open System.Net
open System.Web.Http
open Xunit
open Swensen.Unquote
open Madko.Samples.HttpApi

[<Fact>]
let ``Post returns correct result`` () =
    use sut = new FooController ()
    let foo = { Id = "1234"; Text = "Bar" }

    let actual : IHttpActionResult = sut.Post foo

    test <@ actual :? Results.StatusCodeResult @>
    test <@ (actual :?> Results.StatusCodeResult).StatusCode = HttpStatusCode.Accepted @>