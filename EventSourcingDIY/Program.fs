// EventSourcing by The Dev-Owl https://www.youtube.com/channel/UCOX5DkLyqctM-wkOAU_mUpA

open System

module EventStore =

    type EventProducer<'Event> =
        'Event list -> 'Event list

    type EventStore<'Event> =
        {
            Get : unit -> 'Event list
            Append : 'Event list -> unit
            Evolve : EventProducer<'Event> -> unit
        }

    // what can the MB do
    type Msg<'Event> =
    | Append of 'Event list
    | Get of AsyncReplyChannel<'Event list> // what kind of reply do we expect
    | Evolve of EventProducer<'Event>

    let initialize () : EventStore<'Event> =

        let agent =
            MailboxProcessor.Start(fun inbox ->
                // state can be any state the agent should store
                let rec loop history =
                    async {
                        // let! is like await
                        let! msg = inbox.Receive()

                        match msg with
                        | Append events ->
                            // call the recursive functon to let the agent live
                            return! loop (history @ events)
                        | Get reply -> 
                            // reply on the given channel
                            reply.Reply history

                            // call the recursive functon to let the agent live
                            return! loop history
                        | Evolve eventProducer ->
                            let newEvents =
                                eventProducer history

                            return! loop (history @ newEvents)
                    }
                loop []
                )
        
        let append events =
            agent.Post (Append events)

        let get () =
            agent.PostAndReply Get

        let evolve eventProducer =
            agent.Post (Evolve eventProducer)
        
        {
            Get = get
            Append = append
            Evolve = evolve
        }

module Domain =
    type Flavour =
        | Strawberry
        | Vanilla
    
    type Event =
        | Flavour_sold of Flavour
        | Flavour_restocked of Flavour * int
        | Flavour_went_out_of_stock of Flavour
        | Flavour_was_not_in_stock of Flavour

module Projections =

    open Domain

    type Projection<'State,'Event> =
        {
            Init : 'State
            Update : 'State -> 'Event -> 'State
        }

    let project (projection : Projection<_,_>) events =
        events |> List.fold projection.Update projection.Init

    let soldOfFlavour flavour state =
        state
        |> Map.tryFind flavour
        |> Option.defaultValue 0

    let updateSoldFlavours state event =
        match event with
        | Flavour_sold flavour ->
            state
            |> soldOfFlavour flavour
            |> fun portions -> state |> Map.add flavour (portions + 1)
        | _ -> state

    let soldFlavour : Projection<Map<Flavour,int>,Event> =
        {
            Init = Map.empty
            Update = updateSoldFlavours
        }

    let restock flavour number stock =
        stock
            |> Map.tryFind flavour
            |> Option.map (fun portions -> stock |> Map.add flavour (portions + number))
            |> Option.defaultValue stock

    let updateFlavourInStock stock event =
        match event with
        | Flavour_sold flavour ->
            stock |> restock flavour -1

        | Flavour_restocked (flavour,number) ->
            stock |> restock flavour number
            
        | _ -> stock

    let flavourInStock : Projection<Map<Flavour,int>,Event> =
        {
            Init = Map.empty
            Update = updateFlavourInStock
        }

    let stockOf flavour stock =
        stock
        |> Map.tryFind flavour
        |> Option.defaultValue 0

module Behaviour =

    open Domain
    open Projections

    let sellFlavour flavour (events : Event list) =

        // get stock for specific flavour
        let stock = 
            events
            |> project flavourInStock
            |> stockOf flavour

        // check constraints for flavour sold
        match stock with
        | 0 -> [Flavour_was_not_in_stock flavour]
        | 1 -> [Flavour_sold flavour; Flavour_went_out_of_stock flavour]
        | _ -> [Flavour_sold flavour]

    let restock flavour number events=
        [Flavour_restocked (flavour,number)]    

module Helper =

    open Projections

    let printUl list =
        list
        |> List.iteri (fun i item -> printfn " %i : %A" (i+1) item)

    let printEvents events =
        events
        |> List.length
        |> printfn "History (Length %i)"

        events |> printUl

    let printSoldFlavour flavour state =
        state
        |> soldOfFlavour flavour
        |> printfn "Sold %A: %i" flavour   

open Projections
open EventStore
open Domain
open Helper

[<EntryPoint>]
let main argv =

    let eventStore : EventStore<Event> = EventStore.initialize()

    // eventStore.Append [Flavour_restocked (Vanilla,3)]
    // eventStore.Append [Flavour_sold Vanilla]
    // eventStore.Append [Flavour_sold Vanilla]
    // eventStore.Append [Flavour_sold Strawberry]
    // eventStore.Append [Flavour_sold Vanilla; Flavour_went_out_of_stock Vanilla]

    eventStore.Evolve (Behaviour.sellFlavour Vanilla)
    eventStore.Evolve (Behaviour.sellFlavour Strawberry)

    eventStore.Evolve (Behaviour.restock Vanilla 3)   
    eventStore.Evolve (Behaviour.sellFlavour Vanilla)

    let events = eventStore.Get()
    
    events |> printEvents

    let sold : Map<Flavour,int> =
        events
        |> project soldFlavour

    printSoldFlavour Vanilla sold
    printSoldFlavour Strawberry sold

    0 // return an integer exit code
