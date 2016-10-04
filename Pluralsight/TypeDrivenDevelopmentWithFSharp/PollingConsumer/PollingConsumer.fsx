open System

type Timed<'a> =
    {
        Started : DateTimeOffset
        Stopped : DateTimeOffset
        Result : 'a
    }
    member this.Duration = this.Stopped - this.Started

module Untimed =
    let map f x =
        { Started = x.Started; Stopped = x.Stopped; Result = f x.Result }

    let withResult newResult x = map (fun _ -> newResult) x

module Timed =
    let capture clock x =
        let now = clock()
        { Started = now; Stopped = now; Result = x}

    let map clock f x =
        let result = f x.Result
        let stopped = clock ()
        { Started = x.Started; Stopped = stopped; Result = result }

    let timeOn clock f x = x |> capture clock |> map clock f

module Clocks =
    let machineClock () = DateTimeOffset.Now

    let acclock (start : DateTimeOffset) rate () =
        let now = DateTimeOffset.Now
        let elapsed = now - start
        start.AddTicks (elapsed.Ticks * rate)

    open System.Collections.Generic

    let qlock (q : Queue<DateTimeOffset>) = q.Dequeue

    let seqlock (l : DateTimeOffset seq) = Queue<DateTimeOffset> l |> qlock

// Auxiliary types
type MessageHandler = unit -> Timed<unit>

// Temporary placeholder
type todo = unit
let todo() = ()

/// State data

type ReadyData = Timed<TimeSpan list>

type ReceivedMessageData = Timed<TimeSpan list * MessageHandler>

type NoMessageData = Timed<TimeSpan list>

// States

type PollingConsumer =
    | ReadyState of ReadyData
    | ReceivedMessageState of ReceivedMessageData
    | NoMessageState of NoMessageData
    | StoppedState

// Transitions

let transisionFromStopped = StoppedState

let transitionFromNoMessage shoulIdle idle (nm : NoMessageData) =
    if shoulIdle nm
    then idle () |> Untimed.withResult nm.Result |> ReadyState
    else StoppedState

let transitionsFromReady shouldPoll poll (r : ReadyData) =
    if shouldPoll r
    then
        let msg = poll ()
        match msg.Result with
        | Some h-> msg |> Untimed.withResult (r.Result, h) |> ReceivedMessageState
        | None -> msg |> Untimed.withResult r.Result |> NoMessageState
    else StoppedState

let transitionFromReceived (rm : ReceivedMessageData) : PollingConsumer =
    let durations, handleMessage = rm.Result
    let t = handleMessage ()
    let pollDuration = rm.Duration
    let handleDuration = t.Duration
    let totalDuration = pollDuration + handleDuration
    t |> Untimed.withResult (totalDuration :: durations) |> ReadyState