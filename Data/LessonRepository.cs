using Devhunt.Models;

namespace Devhunt.Data;

public class LessonRepository : ILessonRepository
{
    //Dependency Injection
    private readonly AppDbContext _context;
    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }
    public Lesson CreateLesson(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        lesson.LessonID = _context.SaveChanges();
        return lesson;
    }
}