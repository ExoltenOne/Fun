// Learn more about F# at http://fsharp.org

open System
open System.Threading.Tasks

let getRandomNumbers count max = 
    let rnd = new System.Random()
    List.init count (fun _ -> rnd.Next() % max)

let rec quicksortSequential aList =
    match aList with
    | [] -> []
    | firstElement :: restOfList ->
        let smaller, larger =
            List.partition (fun number -> number < firstElement) restOfList
        quicksortSequential smaller @ (firstElement :: quicksortSequential larger)

let rec quicksortParallel aList =
    match aList with
    | [] -> []
    | firstElement :: restOfList ->
        let smaller, larger = 
            List.partition (fun number -> number < firstElement) restOfList
        let left = Task.Run(fun () -> quicksortParallel smaller)
        let right = Task.Run(fun () -> quicksortParallel larger)
        left.Result @ (firstElement :: right.Result)

let rec quicksortParallelWithDepth depth aList =
    match aList with
    | [] -> []
    | firstElement :: restOfList ->
        let smaller, larger =
            List.partition (fun number -> number < firstElement) restOfList
        if depth < 0 then
            let left = quicksortParallelWithDepth depth smaller
            let right = quicksortParallelWithDepth depth larger
            left @ (firstElement :: right)
        else
            let left = Task.Run(fun () -> quicksortParallelWithDepth (depth - 1) smaller)
            let right = Task.Run(fun () -> quicksortParallelWithDepth (depth - 1) larger)
            left.Result @ (firstElement :: right.Result)
        


[<EntryPoint>]
let main argv =

    let elements = getRandomNumbers 100 100

    printfn "%A" elements

    printfn "quicksortSequential\n\n"
    let sorted = quicksortSequential elements

    printfn "%A" sorted

    printfn "quicksortParallel\n\n"
    let sortedParallel = quicksortParallel elements

    printfn "%A" sortedParallel

    printfn "quicksortParallelWithDepth\n\n"
    let depth = int (Math.Log(float System.Environment.ProcessorCount, 2.) + 4.)
    let sortedParallelWithDepth = quicksortParallelWithDepth depth elements

    printfn "%A" sortedParallelWithDepth

    0 // return an integer exit code
