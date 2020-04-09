using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public interface IGameLogger<in TStaticState, in TTickState>
    {
        IGameAiLogger GetAiLogger(int playerIndex);
        void LogStart(TStaticState staticState);
        void LogTick(TTickState tickState);
        void LogEnd(RaceState tickState);
    }

    public interface IGameAiLogger
    {
        void LogLine(IList<V> points, double intense);
        void LogText(string s);
    }

    public class JsonGameLogger : IGameLogger<RaceTrack, RaceState>
    {
        private readonly Dictionary<int, JsonGameAiLogger> loggers = new Dictionary<int, JsonGameAiLogger>();
        private readonly StreamWriter writer;

        public JsonGameLogger(StreamWriter writer)
        {
            this.writer = writer;
        }

        public IGameAiLogger GetAiLogger(int playerIndex)
        {
            return loggers.GetOrCreate(playerIndex, p => new JsonGameAiLogger());
        }

        public void LogStart(RaceTrack staticState)
        {
            writer.Write(
                $"let " +
                $"originalLog" +
                $" = [{staticState.RaceDuration},[{staticState.Flags.StrJoin(",")}],[{staticState.Obstacles.StrJoin(",")}],[\n");
        }

        public void LogEnd(RaceState tickState)
        {
            var logger = loggers[0];
            writer.Write(
                $"[{tickState.Time}, {(tickState.IsFinished ? 1 : 0)}, {Car2Json(tickState.Car, logger.DebugOutput, logger.DebugLines)}]\n");
            writer.Write("]];");
        }

        public void LogTick(RaceState tickState)
        {
            var logger = loggers[0];
            writer.Write(
                $"[{tickState.Time}, {(tickState.IsFinished ? 1 : 0)}, {Car2Json(tickState.Car, logger.DebugOutput, logger.DebugLines)}],\n");
            logger.DebugLines.Clear();
            logger.DebugOutput = "";
        }

        private string Car2Json(Car car, string output, List<Line> lines)
        {
            var carJson = string.Join(
                ",", 
                car.Pos, car.V, car.Radius, 
                car.FlagsTaken, 
                car.IsAlive ? 1 : 0, 
                car.NextCommand,
                $"\"{EscapeJsonString(output)}\"\n",
                $"[{lines.StrJoin(",", Line2Json)}]");
            return $"[{carJson}]";
        }

        private string Line2Json(Line line)
        {
            FormattableString s = $"[{line.Intense:0.##},{line.Points.StrJoin(",")}]";
            return s.ToString(CultureInfo.InvariantCulture);
        }

        private static string EscapeJsonString(string output)
        {
            if (output == null) return "";
            return output.Replace("\"", "\\\"").Replace("'", "\\'").Replace("\n", "\\n");
        }
    }

    public class JsonGameAiLogger : IGameAiLogger
    {
        public string DebugOutput { get; set; }
        public List<Line> DebugLines { get; } = new List<Line>();

        public void LogLine(IList<V> points, double intense)
        {
            DebugLines.Add(new Line(intense, points));
        }

        public void LogText(string s)
        {
            DebugOutput += s;
        }
    }

    public class Line
    {
        public double Intense;

        public IList<V> Points;

        public Line(double intense, IList<V> points)
        {
            Intense = intense;
            Points = points;
        }
    }
}