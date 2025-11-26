using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src.AI_Play
{


    //public class AI_Run
    //{
    //    public AI_Run() { }

    //    public (int x, int y) Alpha_Beta_Prune(GameEngine gameEngine, int depth=4)
    //    {
    //        int bestValue = int.MinValue;
    //        (int x, int y) bestMove = (-1,-1);
    //        int alpha = int.MinValue;
    //        int beta = int.MaxValue;
    //        for (int i=0; i< 10; i++)
    //        {
    //            for(int j=0; j < 9; j++)
    //            {
    //                if(gameEngine.board.grid[i,j] != null && gameEngine.board.grid[i,j].Color == gameEngine.CurrentTurn)
    //                {
    //                    foreach (var move in gameEngine.board.grid[i, j].GetValidMoves(gameEngine.board))
    //                    {
    //                        if (gameEngine.MakeMove(i, j, move.y, move.x))
    //                        {
    //                            int value = Min_Value(gameEngine, alpha, beta, depth - 1);
    //                            gameEngine.UndoMove();
    //                            if (value > bestValue)
    //                            {
    //                                bestValue = value;
    //                                bestMove = move;
    //                            }
    //                            alpha = Math.Max(alpha, bestValue);
    //                            if (beta <= alpha)
    //                            {
    //                                break; // Beta cut-off
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        return bestMove;
    //    }
    //    public int Max_Value(GameEngine gameEngine, int alpha, int beta, int depth)
    //    {

    //        if (gameEngine.IsCheckmate(gameEngine.CurrentTurn) && depth > 0)
    //        {
    //            return -2000 + depth;
    //        }
    //        if (depth == 0)
    //        {
    //            return -gameEngine.board.EvaluateBoard(gameEngine.CurrentTurn);
    //        }
    //        int maxEval = int.MinValue;
    //        for (int i = 0; i < 10; i++)
    //        {
    //            for (int j = 0; j < 9; j++)
    //            {
    //                if (gameEngine.board.grid[i, j] != null && gameEngine.board.grid[i, j].Color != gameEngine.CurrentTurn)
    //                {
    //                    foreach (var move in gameEngine.board.grid[i, j].GetValidMoves(gameEngine.board))
    //                    {
    //                        if (gameEngine.MakeMove(i, j, move.x, move.y)){
    //                            int eval = Min_Value(gameEngine, alpha, beta, depth - 1);
    //                            gameEngine.UndoMove();
    //                            maxEval = Math.Max(maxEval, eval);
    //                            alpha = Math.Max(maxEval, alpha);
    //                            if (beta <= alpha)
    //                            {
    //                                return maxEval; // Alpha cut-off
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        return maxEval;

    //    }
    //    public int Min_Value(GameEngine gameEngine, int alpha, int beta, int depth)
    //    {
    //        if (gameEngine.IsCheckmate(gameEngine.CurrentTurn == PieceColor.Red ? PieceColor.Black : PieceColor.Red))
    //        {
    //            return 2000 - depth;
    //        }
    //        if (depth == 0)
    //        {
    //            return  gameEngine.board.EvaluateBoard(gameEngine.CurrentTurn);
    //        }
    //        int minEval = int.MaxValue;
    //        for (int i=0; i< 10; i++)
    //        {
    //            for (int j=0; j<9; j++)
    //            {
    //                if(gameEngine.board.grid[i, j] != null && gameEngine.board.grid[i, j].Color == gameEngine.CurrentTurn)
    //                {
    //                    foreach(var move in gameEngine.board.grid[i, j].GetValidMoves(gameEngine.board))
    //                    {
    //                        if (gameEngine.MakeMove(i, j, move.x, move.y))
    //                        {
    //                            int eval = Max_Value(gameEngine, alpha, beta, depth-1);
    //                            gameEngine.UndoMove();
    //                            minEval = Math.Min(minEval, eval);
    //                            beta = Math.Min(minEval, beta);    
    //                            if (beta <= alpha)
    //                            {
    //                                return minEval; // Beta cut-off
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        return minEval;

    //    }
    //}


    public class AI_Run
    {
        PieceColor aiColor;
        public AI_Run(PieceColor color)
        {
            aiColor = color;
        }
        private const int WIN_SCORE = 10000;
        private const int LOSE_SCORE = -10000;

        public (int fromX, int fromY, int toX, int toY)? FindBestMove(GameEngine gameEngine, int depth)
        {
            int bestValue = int.MinValue;
            (int fromX, int fromY, int toX, int toY)? bestMove = null;

            int alpha = int.MinValue;
            int beta = int.MaxValue;

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    var piece = gameEngine.board.GetPiece(x, y);
                    if (piece == null || !piece.IsAlive || piece.Color != aiColor)
                        continue;

                    var moves = piece.GetValidMoves(gameEngine.board);
                    foreach (var move in moves)
                    {
                        if (!gameEngine.MakeMove(x, y, move.x, move.y))
                            continue;

                        int value = AlphaBeta(gameEngine, depth - 1, alpha, beta);
               
                        if (gameEngine.UndoMove()) { }

                        if (value > bestValue)
                        {
                            bestValue = value;
                            bestMove = (x, y, move.x, move.y);
                        }

                        alpha = Math.Max(alpha, bestValue);
                        if (alpha >= beta)
                            return bestMove;
                    }
                }
            }

            return bestMove;
        }

        private int AlphaBeta(GameEngine ge, int depth, int alpha, int beta)
        {
            // Kiểm tra chiếu bí trước
            if (ge.IsCheckmate(ge.CurrentTurn))
            {
                PieceColor loser = ge.CurrentTurn;
                if (loser == aiColor)
                    return LOSE_SCORE - depth;
                else
                    return WIN_SCORE + depth;
            }

            // Đạt độ sâu tối đa
            if (depth == 0)
            {
                return ge.board.EvaluateBoard(aiColor);
            }

            bool maximizingPlayer = (ge.CurrentTurn == aiColor);

            if (maximizingPlayer)
            {
                int value = int.MinValue;
                bool hasMove = false;

                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        var piece = ge.board.grid[y, x];
                        if (piece == null || !piece.IsAlive || piece.Color != ge.CurrentTurn)
                            continue;

                        var moves = piece.GetValidMoves(ge.board);
                        foreach (var move in moves)
                        {
                            if (!ge.MakeMove(x, y, move.x, move.y))
                                continue;

                            hasMove = true;
                            int eval = AlphaBeta(ge, depth - 1, alpha, beta);
                            if (ge.UndoMove() == false)
                            {
                                MessageBox.Show($"Depth: {depth}, Piece name: {piece.Name}, To {move.x}-{move.y}, current: {ge.CurrentTurn.ToString()}");
                                BasePiece basePiece = ge.board.GetPiece(move.x, move.y);
                            }

                            value = Math.Max(value, eval);
                            alpha = Math.Max(alpha, value);

                            if (alpha >= beta)
                                return value;
                        }
                    }
                }

                // Không có nước đi hợp lệ
                if (!hasMove)
                    return LOSE_SCORE - depth;

                return value;
            }
            else
            {
                int value = int.MaxValue;
                bool hasMove = false;

                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        var piece = ge.board.grid[y, x];
                        if (piece == null || !piece.IsAlive || piece.Color != ge.CurrentTurn)
                            continue;

                        var moves = piece.GetValidMoves(ge.board);
                        foreach (var move in moves)
                        {
                            if (!ge.MakeMove(x, y, move.x, move.y))
                                continue;

                            hasMove = true;
                            int eval = AlphaBeta(ge, depth - 1, alpha, beta);
                            if (ge.UndoMove() == false)
                            {
                                MessageBox.Show($"Depth: {depth}, Piece name: {piece.Name}, To {move.x}-{move.y}, current: {ge.CurrentTurn.ToString()}");
                            }

                            value = Math.Min(value, eval);
                            beta = Math.Min(beta, value);

                            if (alpha >= beta)
                                return value;
                        }
                    }
                }

                // Không có nước đi hợp lệ
                if (!hasMove)
                    return WIN_SCORE + depth;

                return value;
            }
        }
    }

}
