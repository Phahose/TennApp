namespace OnCourt.Domain
{
    public class FileUpload
    {
       private int FileID {  get; set; }
       private int UserId { get; set; }
       private string FileName { get; set; } = string.Empty;
       private string FileType { get; set; } = string.Empty;
       private string FilePath { get; set; } = string.Empty;
       private DateTime UploadedAt { get; set; }

    } 
}
