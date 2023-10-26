namespace webapi.Services
{
    public sealed class TemporaryFile : IDisposable
    {
        public string FilePath { get; private set; }

        public TemporaryFile()
        {
            FilePath = Path.GetTempFileName();
        }

        public TemporaryFile(string filePath)
        {
            FilePath = Path.Combine(filePath, Path.GetRandomFileName());
        }

        ~TemporaryFile()
        {
            Delete();
        }

        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
        }

        private void Create()
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath);
            }
        }

        private void Delete()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                FilePath = string.Empty;
            }
        }
    }
}
