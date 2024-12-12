import Utils
import qualified Data.Text.IO as IO
import qualified Data.Text as T
import System.CPUTime (getCPUTime)

checksafe::[Integer] -> Bool -> Bool
checksafe [] _ = True
checksafe [_] _ = True
checksafe (x:y:ys) isInc = let diff = x-y in
    (diff < 0) == isInc && (4 > abs diff) && (0 < abs diff) && checksafe (y:ys) isInc

checksafewrap::[Integer] -> Bool
checksafewrap ls@(x:y:xs) = checksafe ls (x < y)

checksaferemoving::[Integer] -> [Integer] -> Bool
checksaferemoving [] _ = False
checksaferemoving (x:xs) prev =
    checksafewrap (prev ++ xs) || checksaferemoving xs (prev ++ [x])

countsafe::[[Integer]] -> Integer
countsafe [] = 0
countsafe xs = sum $ map (\ls@(x:y:ys) -> if checksafe ls (x < y) then 1 else 0) xs

countsafe2::[[Integer]] -> Integer
countsafe2 [] = 0
countsafe2 xs = sum $ map (\ls@(x:y:ys) -> if cond ls then 1 else 0) xs
    where cond ls = checksafewrap ls || checksaferemoving ls []

main = do
    text <- IO.readFile "../inputs/day2.txt"
    t <- getCPUTime
    let nums = [map readInt (T.words line) | line <- T.lines text]
    let soln1 = countsafe nums
    let soln2 = countsafe nums
    print $ countsafe nums
    print $ countsafe2 nums
    t2 <- getCPUTime
    print $ fromIntegral (t2 - t)/(10^9) -- ms