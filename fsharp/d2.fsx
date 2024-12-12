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
            let diff = (x-y)
            diff < 0 = isinc && Math.Abs diff > 0 && Math.Abs diff < 4 && csint (y::xs) isinc
    csint lin <| (lin[0] < lin[1])

let checksafe2 lin =
    let rec cs2 prev lin =
        match lin with
        | [] -> false
        | (_::xs) when (checksafe (prev @ xs)) -> true
        | (x::xs) -> cs2 (prev@[x]) xs
    if checksafe lin
    then true
    else cs2 [] lin

let rec run ls f =
    match ls with
    | [] -> []
    | (x::xs) -> (if (f x) then 1 else 0)::run xs f

run file checksafe |> Seq.sum |> printfn "%d"
run file checksafe2 |> Seq.sum |> printfn "%d"