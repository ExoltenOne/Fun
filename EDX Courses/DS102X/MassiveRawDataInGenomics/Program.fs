// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    let AB0BloodType = "ggccgcctcc cgcgcccctc tgtcccctcc cgtgttcggc ctcgggaagt cggggcggcg ggcggcgcgg gccgggaggg gtcgcctcgg gctcaccccg ccccagggcc gccgggcgga aggcggaggc cgagaccaga cgcggagcca tggccgaggt gttgcggacg ctggccg".Replace(" ", "")

    let chop (input : string) len = 
        Array.init (input.Length - len) (fun index ->
            let start = index
            input.[start..start + len - 1])

    let allReads = chop AB0BloodType 14 |> Array.toList

    let reads = [| "ccggcctcgggaag" ; "ttgcggacgctagc" ; "tcgggctccccccg" ; "ggggggaaggcgga" ; "tctgtccccccccg" |]

    let compareReads ignoreMistake (original:string) (expectedToFind:string) =

        let compare (pair:char*char) =
            match pair with
            | (x,y) -> if x.Equals(y) then 1 else 0

        let rec compareChars acc (pairs:(char*char) list) =
            match pairs with
            | [] -> acc
            | head::tail  -> compareChars (acc + (compare head)) tail

        let explode (s:string) =
            [for c in s -> c]
        
        let t = List.map2 (fun x y -> (x,y)) (explode original) (explode expectedToFind)
                |> compareChars 0

        t = original.Length - ignoreMistake

    reads 
        |> Array.iter (fun x -> 
            try
                (List.findIndex (fun elem -> compareReads 1 elem x) allReads) + 1 // because zero-based numeration
            with
              | :? System.Collections.Generic.KeyNotFoundException  -> -1
            
            |> printfn "%A - %A" x)

    printfn "%A" argv
    0 // return an integer exit code
