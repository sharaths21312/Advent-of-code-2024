import Utils
import qualified Data.Text as T
import qualified Data.Text.IO as IO
import qualified Data.Map.Strict as M
import qualified Data.Vector as V

countxmas :: M.Map (Integer, Integer) Char -> [((Integer, Integer), Char)] -> Int
countxmas _ [] = 0
countxmas mp (((x, y), _):xs) = countxmas mp xs +
        length (filter id [mc up, mc down, mc left, mc right, 
        mc upleft, mc upright, mc downleft, mc downright])
    where
        up = [(x, y - diff) | diff <- [0..3]]
        down = [(x, y + diff) | diff <- [0..3]]
        left = [(x - diff, y) | diff <- [0..3]]
        right = [(x + diff, y) | diff <- [0..3]]
        upleft = [(x - diff, y - diff) | diff <- [0..3]]
        upright = [(x + diff, y - diff) | diff <- [0..3]]
        downleft = [(x - diff, y + diff) | diff <- [0..3]]
        downright = [(x + diff, y + diff) | diff <- [0..3]]
        mc = flip (matchListMap mp) "XMAS"

countxmas2 :: M.Map (Integer, Integer) Char -> [((Integer, Integer), Char)] -> Int
countxmas2 _ [] = 0
countxmas2 mp (((x, y), _):xs) = countxmas2 mp xs +
    fromEnum ((mc diag1 || mc' diag1) && (mc diag2 || mc' diag2))
    where
        diag1 = [(x - diff, y - diff) | diff <- [-1..1]]
        diag2 = [(x + diff, y - diff) | diff <- [-1..1]]
        mc = flip (matchListMap mp) "MAS"
        mc' = flip (matchListMap mp) "SAM"

main = do
    filedata <- T.strip <$> IO.readFile "../inputs/day4.txt"
    let c = textIntoCoords filedata
    print $ countxmas c $ filter (\(_, c) -> c == 'X') $ M.toList c
    print $ countxmas2 c $ filter (\(_, c) -> c == 'A') $ M.toList c
