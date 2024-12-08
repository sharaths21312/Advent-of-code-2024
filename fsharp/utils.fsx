open System
open System.Linq

let split (s1:string) (s2:string):list<string> = List.ofArray <| s2.Split(s1, StringSplitOptions.RemoveEmptyEntries)
let exceptidx (idx:int) (ls:list<'a>) = 
    match idx with
    | 0 -> List.tail ls
    | i -> List.append ls[..idx-1] ls[idx+1..]
let count f xs = Enumerable.Count(xs, f)
let isbwabs (it:int) x y = Math.Abs(it) < x && Math.Abs(it) < y