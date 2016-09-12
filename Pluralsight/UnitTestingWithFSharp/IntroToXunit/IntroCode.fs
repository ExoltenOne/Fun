namespace Madko.Samples

open System

module Messaging =
    type Envelope<'a> = {
        Id: Guid
        Created : DateTimeOffset
        Item : 'a }

    let envelop getId getTime item = {
        Id = getId ()
        Created = getTime ()
        Item = item }

module MessagingTests =
    open Xunit
    open Swensen.Unquote
    open Messaging

    type Foo = { Text : string; Number : int }

    [<Theory>]
    [<InlineData("F71882D3-30B8-4EF1-BC5B-31BF1ACEA9C4", 636092737648600550L, 1., "Bar", 42)>]
    [<InlineData("02330996-764C-45AF-AA3F-E77A5209BBA2", 636092737648600950L, 0., "Bazz", 1367)>]
    let ``envelop return the correct result`` (id : string) (ticks : int64) (offset : float) (text : string) (number : int) =
        let getId _ = Guid id
        let getTime _ = DateTimeOffset(ticks, TimeSpan.FromHours offset)
        let item = { Text = text; Number = number }

        let actual = Messaging.envelop getId getTime item

        test <@ (Guid id) = actual.Id @>
        test <@ DateTimeOffset(ticks, TimeSpan.FromHours offset) = actual.Created @>
        test <@ item = actual.Item @>
