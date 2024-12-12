import Utils
import qualified Data.Text.IO as IO
import qualified Data.Text as T
import Data.List (sort)
import Text.Printf (printf)

sumdiffs [] _ = 0
sumdiffs _ [] = 0
sumdiffs (x:xs) (y:ys) = abs (x-y) + sumdiffs xs ys

proddiff [] _ = 0
proddiff _ [] = 0
proddiff (x:xs) (y:ys)
    | x == y = x + proddiff (x:xs) ys
    | x < y = proddiff xs (y:ys)
    | x > y = proddiff (x:xs) ys
-- There are no duplicates so this implementation works

countdupes [] = 0
countdupes [x] = 0
countdupes (x:y:xs)
    | x == y = 1 + countdupes (y:xs)
    | otherwise = countdupes (y:xs)

main = do
    file <- IO.readFile "../inputs/day1.txt"
    let (first, second) = unzip [(readInt x, readInt y) | (x:y:_) <- map T.words $ T.lines file]
    let sfirst = sort first
    let ssecond = sort second
    print $ countdupes sfirst
    print $ sumdiffs sfirst ssecond
    print $ proddiff sfirst ssecond