module Utils where
import qualified Data.Text as T

readInt :: TextLike t => t -> Integer
readInt = read . toString

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