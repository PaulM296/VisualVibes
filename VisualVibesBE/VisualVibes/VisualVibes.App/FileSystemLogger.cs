namespace VisualVibes.App
{
    public class FileSystemLogger
    {
        private readonly string _loggingDirectory;

        public FileSystemLogger(string loggingDirectory)
        {
            _loggingDirectory = loggingDirectory;
            if(!Directory.Exists(loggingDirectory))
            {
                Directory.CreateDirectory(loggingDirectory);
            }
        }

        public async Task LogAsync(string methodName, bool isSuccess)
        {
            string loggingFileName = $"Logs_{DateTime.Now:dd-MM-yyyy}.txt";
            string loggingFilePath = Path.Combine(_loggingDirectory, loggingFileName);

            string loggingMessage = $"{DateTime.Now:dd-MM-yyyy HH-mm-ss}\tMethod: {methodName}\t\t" +
                $"Outcome: {(isSuccess ? "Success" : "Failure")}{Environment.NewLine}";

            await WriteLogAsync(loggingFilePath, loggingMessage);
        }

        public async Task WriteLogAsync(string loggingFilePath, string loggingMessage)
        {
            using (StreamWriter streamWriter = File.AppendText(loggingFilePath))
            {
                await streamWriter.WriteAsync(loggingMessage);

            }
        }

        public async Task<string> ReadLogFileAsync(string loggingFilePath)
        {
            string loggingContent = string.Empty;
            try
            {
                using (StreamReader streamReader = new StreamReader(loggingFilePath))
                {
                    loggingContent = await streamReader.ReadToEndAsync();
                }
            }
            catch (FileNotFoundException) 
            {
                loggingContent = "The logging file has not been found.";
            }
            catch (Exception ex)
            {
                loggingContent = $"There was an error while reading the file: {ex.Message}";
            }

            return loggingContent;
        }
    }
}
