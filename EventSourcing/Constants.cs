namespace LightestNight.System.EventSourcing
{
    public class Constants
    {
        public const string SystemStreamPrefix = "@";
        public const string VersionKey = "version";
        public const string CheckpointPrefix = "checkpoint";
        public const string CheckpointMessageType = "checkpoint";
        public const string GlobalCheckpointId = "global";
        public const string TimestampKey = "timestamp";
    }
}