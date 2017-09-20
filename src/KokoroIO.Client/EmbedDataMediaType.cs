using System.Runtime.Serialization;

namespace KokoroIO
{
    /// <summary>
    /// Represents a type of of <see cref="EmbedDataMedia"/>.
    /// </summary>
    [DataContract]
    public enum EmbedDataMediaType
    {
        /// <summary>
        /// The media is an image.
        /// </summary>
        [EnumMember]
        Image,

        /// <summary>
        /// The media is a video.
        /// </summary>
        [EnumMember]
        Video,

        /// <summary>
        /// The media is an audio.
        /// </summary>
        [EnumMember]
        Audio
    }
}