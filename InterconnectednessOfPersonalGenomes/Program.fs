// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

let individuals = [| "gtctctcggtaggcctcttggcagctctatcggcgagtatctcggcacg";
                     "gtctcgtgacaggtatctcggtaactatctcggtagctaacgcggcgtg";
                     "gtcactcggtaggcctctcggtgagtatctcgataggtaactcggcgtc";
                     "atctcgcggtagccaacttggtaggtctatcggcaagtctctccgcgcc";
                     "gtctctcggcaggtatattcataggtctattgataggtcacttggcatg";
                     "gtcacgccgtaggtaacttcgtaggtctatcggtaggtctcgtgacatg";
                     "gcctatcggtgaccaactcgatgggtctatcggcaagccacgcgatgtg";
                     "gtctcttcgtagctatcttcataggtcacgcgatgggtcacgcggtgtc";
                     "gtatctcggtaggcatctcggtagctctatccgtaagtatctcgatgtc";
                     "atatcttggcaaccaactcgatgggtctatcggcaagccacgcgatatg";
                     "gtctctcggtaagtatctccgcgagtcaatcgacgggtctatcgacatc";
                     "atctctcggtaggcctctcggtgagtatctcgataggtctatcggtatg";
                     "gtcacgcgatagctctctcggtgactctcttggcaggtctatccacgtc";
                     "gcctctcggtaggcctctcggcaggtcaattggtgggtctctcgatatc";
                     "gtctcgcgataggtatctcgatgggtctcttgatgggtctctccgcatg" |]

[<EntryPoint>]
let main argv = 
    
    let compare firstIndex secondIndex (individuals : string []) =
        let first = individuals.[firstIndex - 1].ToCharArray()
        let second = individuals.[secondIndex - 1].ToCharArray()
        let compared = Array.map2 (fun x y -> x = y) first second

        compared

    let rec findIdentity starting index acc (comparedIndividuals : bool list) =
        match comparedIndividuals with
        | [] ->
                printfn "%i - %i" (fst acc) (snd acc - 1)
        | x::xs ->
                match x with
                | false -> 
                            let acc' = 
                                        match (index - starting) > (snd acc - fst acc) with
                                        | true -> (starting, index)
                                        | false -> acc
                            findIdentity (index + 1) (index + 1) acc' xs
                | true ->
                            findIdentity starting (index + 1) acc xs

    compare 3 12 individuals 
    |> Array.toList
    |> findIdentity 1 1 (1,1)

    0 // return an integer exit code
