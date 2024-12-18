import Utils
import qualified Data.Text as T
import qualified Data.Text.IO as IO
import qualified Data.Map.Strict as M
import qualified Data.Set as S
import Data.Maybe (fromMaybe)
import Data.List (groupBy)
import System.CPUTime (getCPUTime)

main = do
    file <- T.strip <$> IO.readFile "../inputs/day5.txt"
    let splitfile = splitText "\n\n" file
    let part1 = [(readInt $ head x, readInt $ last x) | x <- map (splitText "|") $ lines $ head splitfile]
    let part2 = [map readInt x | x <- map (splitText ",") $ lines $ last splitfile]
    let invalids = consInvalidMap part1
    putStrLn ""

-- checkCorrect :: M.Map Integer (S.Set Integer) -> [Integer] -> Bool
-- checkCorrect _ [] = True
checkCorrect mp (x:xs) = not . flip any xs
    where invalids = fromMaybe S.empty $ M.lookup x mp
          checkinvalid = S.member invalids


consInvalidMap :: [(Integer, Integer)] -> M.Map Integer (S.Set Integer)
consInvalidMap ls = M.fromList zippeddata
    where zippeddata = [(snd $ head item, S.fromList $ map fst item) | item <- groupBy (\(_, x) (_, y) -> x == y) ls]
