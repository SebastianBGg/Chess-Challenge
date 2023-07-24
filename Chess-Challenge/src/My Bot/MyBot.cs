using ChessChallenge.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        bool white = false;
        Move[] moves = board.GetLegalMoves();
        if (board.GetPiece(moves[0].StartSquare).IsWhite) { white = true; }
        foreach (Move move in moves)
        {

        }


        return moves[0];
    }

    // Took this from the evilbot code
    bool MoveIsCheckmate(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheckmate();
        board.UndoMove(move);
        return isMate;
    }

    Move BestMove(Board board, Move[] moves, bool black)
    {
        List<Move> worth = new List<Move>();
        int points;
        int afterpoint;

        if (black)
        {
            foreach (Move move in moves)
            {
                points = GetBoardPoints(board);
                board.MakeMove(move);
                afterpoint = GetBoardPoints(board);
                board.UndoMove(move);

                if (points > afterpoint)
                {
                    worth.Add(move);
                }
            }
        }
        else
        {
            foreach (Move move in moves)
            {
                points = -GetBoardPoints(board);
                board.MakeMove(move);
                afterpoint = -GetBoardPoints(board);
                board.UndoMove(move);

                if (points > afterpoint)
                {
                    worth.Add(move);
                }
            }
        }
        if (worth.Count > 0)
        {
            return worth[0];
        }
        return worth[0];
    }


    int GetTheTotalValue(Piece piece)
    {
        // Piece values: null, pawn, knight, bishop, rook, queen, king
        int[] pieceValues = { 0, 1, 3, 3, 5, 9, 100 };
        int one = pieceValues[(int)piece.PieceType];
        return one;
    }
    Move[] GetWhiteMoves(Board board, Move move)
    {
        Move[] moves = board.GetLegalMoves();
        if (board.GetPiece(moves[0].StartSquare).IsWhite)
        {
            return moves;
        }
        else
        {
            board.MakeMove(moves[0]);
            Move[] newmoves = board.GetLegalMoves();
            board.UndoMove(move);
            return newmoves;
        }

    }
    Move[] GetBlackeMoves(Board board, Move move)
    {
        Move[] moves = board.GetLegalMoves();
        if (!board.GetPiece(moves[0].StartSquare).IsWhite)
        {
            return moves;
        }
        else
        {
            board.MakeMove(moves[0]);
            Move[] newmoves = board.GetLegalMoves();
            board.UndoMove(move);
            return newmoves;
        }
    }
    int GetBoardPoints(Board board)
    {
        int[] pieceValues = { 0, 1, 3, 3, 5, 9, 100 };
        int whitepoi = 0;
        int blackpoi = 0;

        PieceList[] pieceLists = board.GetAllPieceLists();
        foreach (PieceList piece in pieceLists)
        {

            if (piece.IsWhitePieceList)
            {
                whitepoi += pieceValues[(int)piece.TypeOfPieceInList];
            }
            if (!piece.IsWhitePieceList)
            {
                blackpoi += pieceValues[(int)piece.TypeOfPieceInList];
            }
        }
        return whitepoi - blackpoi;
    }
    int minimax(Board board, Move move, int depth, bool maximizingPlayer)
    {
        if (depth == 0) return 0;
        if (maximizingPlayer)
        {
            int amaxEval = int.MinValue;
            Move[] aMoves = GetWhiteMoves(board, move);
            foreach (Move amove in aMoves)
            {
                int eval = minimax(board, amove, depth - 1, false);
                amaxEval = Math.Max(amaxEval, eval);
            }
            return amaxEval;

        }
        else
        {
            int minEval = int.MaxValue;
            Move[] bMoves = GetBlackeMoves(board, move);
            foreach (Move bamove in bMoves)
            {
                int eval = minimax(board, move, depth - 1, true);
                minEval = Math.Min(minEval, eval);
            }
            return minEval;
        }
    }
}
