open System
open System.IO
open System.Text.RegularExpressions
#load "utils.fsx"
open Utils

type vaildator = 
    | ToDo of bool
    | Value of int

let check = Regex(@"(?<do>do\(\))|(?<dont>don't\(\))|(?<mult>mul\((?<mul1>\d+),(?<mul2>\d+)\))")

let processmatch (mtc:Match) = 
    if mtc.Groups["mult"].Success
    then (Int32.Parse(mtc.Groups["mul1"].Value) * Int32.Parse(mtc.Groups["mul2"].Value))
    else 0

let runp1 (file:string) = 
    let mc = check.Matches(file) |> List.ofSeq
    let rec runin mtc =
        match mtc with
        | [] -> 0
        | (x::xs) -> (processmatch x) + runin xs
    runin mc

let file =
    using (new StreamReader("../inputs/day3.txt"))
        (fun f -> f.ReadToEnd())

runp1 file |> printfn "%d"