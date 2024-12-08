open System;
open System.IO;
#load "utils.fsx"
open Utils

let file =
    using (new StreamReader("../inputs/day1.txt"))
        (fun f -> f.ReadToEnd() |> split "\n" |> List.map (split " "))

let rec sumdiff ls1 (ls2:list<int>) = 
    match (ls1, ls2) with
    | h1::r1, h2::r2 -> Math.Abs(h1-h2) + sumdiff r1 r2
    | _ -> 0

let rec sumprod ls1 lscheck =
    match ls1 with
    | [] -> 0
    | x::xs -> sumprod xs lscheck + x*count ((=)x) lscheck

let t = DateTime.Now
let (nums1, nums2) = 
    List.map (fun [h;t] -> (Int32.Parse h, Int32.Parse t)) file |> List.unzip
    |> (fun (ls1, ls2) -> (List.sort ls1, List.sort ls2))
printfn "%d" <| sumdiff nums1 nums2
printfn "%d" <| sumprod nums1 nums2
printfn "%f" <| DateTime.Now.Subtract(t).TotalMilliseconds