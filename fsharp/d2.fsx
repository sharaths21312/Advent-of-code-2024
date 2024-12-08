open System
open System.IO
#load "utils.fsx"
open Utils

let file =
    using (new StreamReader("../inputs/day2.txt"))
        (fun f -> f.ReadToEnd() |> split "\n" |> List.map (split " " >> List.map Int32.Parse))

let checksafe lin =
    let rec csint (ls:list<int>) (isinc:bool) =
        match ls with
        | [] -> true
        | [x] -> true
        | x::(y::xs) -> 
            let diff = x-y in
            (diff > 0 = isinc) && Math.Abs(diff) > 0 && Math.Abs(diff) < 4 && csint (y::xs) isinc
    csint lin <| (lin[0] < lin[1])

printfn "%A" <| List.map checksafe file