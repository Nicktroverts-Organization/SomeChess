namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public enum ChessPieceType
    {
        Knight,
        Queen,
        King,
        Bishop,
        Pawn,
        Rook,
        None
    }

    public enum Team
    {
        White,
        Black,
    }

    public enum ChessState
    {
        WhiteWin,
        BlackWin,
        Draw,
        Playing,
        None
    }
}
