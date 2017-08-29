using System.Runtime.Serialization;

namespace Shipwreck.KokoroIO
{
    /// <summary>
    /// Represents a type of of <see cref="EmbedData"/>.
    /// </summary>
    [DataContract]
    public enum EmbedDataType
    {
        /// <summary>
        /// The location contains mixed contents.
        /// </summary>
        [EnumMember]
        MixedContent,

        /// <summary>
        /// The location contains an image.
        /// </summary>
        [EnumMember]
        SingleImage,

        /// <summary>
        /// The location contains a video.
        /// </summary>
        [EnumMember]
        SingleVideo,

        /// <summary>
        /// The location contains an audio.
        /// </summary>
        [EnumMember]
        SingleAudio
    }
}