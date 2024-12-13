module Utils where
import qualified Data.Text as T
import qualified Data.Map.Strict as M

readInt :: TextLike t => t -> Integer
readInt = read . toString

textIntoCoords :: TextLike t => t -> M.Map (Integer, Integer) Char
textIntoCoords txt = M.fromList $ zip coords (concat $ lines str)
    where
        str = toString txt
        lenx = length $ head $ lines str
        leny = length $ lines str
        coords = [(toInteger x, toInteger y) | x <- [0..lenx-1], y <- [0..leny-1]]

matchListMap :: (Ord k, Eq a) => M.Map k a -> [k] -> [a] -> Bool
matchListMap _ [] [] = True
matchListMap mp (k:ks) (v:vs) =
    case M.lookup k mp of
    Just val -> (v == val) && matchListMap mp ks vs
    Nothing -> False
matchListMap _ _ _ = False -- This only runs if the lists are of unequal length

class TextLike a where
    toText :: a -> T.Text
    toString :: a -> String
instance TextLike T.Text where
    toText = id
    toString = T.unpack
instance TextLike String where
    toText = T.pack
    toString = id
instance TextLike Char where
    toText = T.pack . (:[])
    toString = (:[])