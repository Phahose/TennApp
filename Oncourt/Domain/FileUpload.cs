namespace OnCourt.Domain
{
    public class FileUpload
    {
       public int FileID {  get; set; }
       public int UserId { get; set; }
       public string FileName { get; set; } = string.Empty;
       public string FileType { get; set; } = string.Empty;
       public string FilePath { get; set; } = string.Empty;
       public DateTime UploadedAt { get; set; }

    } 
}
