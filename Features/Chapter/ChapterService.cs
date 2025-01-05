using WebApplication3.Clients.BackblazeService;
using WebApplication3.Features.Manga;
using WebApplication3.Models;

namespace WebApplication3.Features.Chapter;

public class ChapterService(TodoContext _context, IUploadToBackBlaze _uploadUtility)
{
    public async Task<Models.Chapter> AddChapter(ChapterDtos chapterDtos )
    {
        if (chapterDtos.PageFiles != null && chapterDtos.PageFiles.Any())
        {
            var fileUrls = new List<string>();
            foreach (var file in chapterDtos.PageFiles)
            {
                var fileUrl = await _uploadUtility.Upload(file);
                fileUrls.Add(fileUrl);
            }
            var chapter = new Models.Chapter
            {
                MangaId = chapterDtos.MangaId,
                Title = chapterDtos.Title,
                VolumeNumber = chapterDtos.VolumeNumber,
                ChapterNumber = chapterDtos.ChapterNumber,
                ReleaseDate = chapterDtos.ReleaseDate,
                Language = chapterDtos.Language,
                PageUrls = fileUrls,
                TotalPages = fileUrls.Count
            };
            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();
            return chapter;

            
        }
        return null;
    }
}