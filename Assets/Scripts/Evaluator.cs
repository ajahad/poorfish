public static class Evaluator
{
    // source chessprogramming wiki SEF for values
    private const int ValPawn   = 100;
    private const int ValKnight = 320;
    private const int ValBishop = 330;
    private const int ValRook   = 500;
    private const int ValQueen  = 900;
    private const int ValKing   = 0;

    private const int EndgameThreshold = 1500;

    public static bool IsEndgame(BoardState state)
    {
        int totalMaterial = 0;

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece piece = state.GetPiece(col, row);
                if (piece == null || piece.Type == PieceType.King) continue;

                totalMaterial += GetMaterialValue(piece.Type);
            }
        }

        return totalMaterial < EndgameThreshold;
    }

    public static int Evaluate(BoardState state)
    {
        bool endgame = IsEndgame(state);
        int score = 0;

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece piece = state.GetPiece(col, row);
                if (piece == null) continue;

                int material = GetMaterialValue(piece.Type);
                int pst = PieceSquareTables.GetPST(piece.Type, piece.Color, col, row, endgame);

                if (piece.Color == PieceColor.White)
                    score += material + pst;
                else
                    score -= material + pst;
            }
        }

        return score;
    }

    private static int GetMaterialValue(PieceType type)
    {
        return type switch
        {
            PieceType.Pawn   => ValPawn,
            PieceType.Knight => ValKnight,
            PieceType.Bishop => ValBishop,
            PieceType.Rook   => ValRook,
            PieceType.Queen  => ValQueen,
            PieceType.King   => ValKing,
            _                => 0
        };
    }
}