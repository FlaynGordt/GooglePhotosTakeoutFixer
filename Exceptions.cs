namespace GooglePhotosTakeoutFixer
{
    public class BaseProcessingException : Exception
    {
        public BaseProcessingException(string originalFileName)
        {
            if (string.IsNullOrWhiteSpace(originalFileName))
            {
                throw new ArgumentException($"'{nameof(originalFileName)}' cannot be null or whitespace.", nameof(originalFileName));
            }

            OriginalFileName = originalFileName;
        }

        public string OriginalFileName { get; }
    }

    public class NotAnImageException : BaseProcessingException
    {
        public NotAnImageException(string originalFileName) : base(originalFileName)
        {
        }
    }

    public class MetaDataNotFoundException : BaseProcessingException
    {
        public MetaDataNotFoundException(string originalFileName, string metaData) : base(originalFileName)
        {
            MetaData = metaData;
        }

        public string MetaData { get; }
    }

    public class NoMetaFileFoundException : BaseProcessingException
    {
        public NoMetaFileFoundException(string originalFileName) : base(originalFileName)
        {
        }
    }
}
